namespace RedPandaTCD_Web.Game;

public enum BotDifficulty
{
    Easy,
    Medium,
    Hard
}

public static class BotManager
{
    // Selected difficulty for training matches.
    public static BotDifficulty Difficulty { get; set; }
        = BotDifficulty.Medium;

    // Tutorial matches use the Medium decision logic.
    public static bool IsTutorialMode { get; set; } = false;

    private static BotDifficulty EffectiveDifficulty =>
        IsTutorialMode
            ? BotDifficulty.Medium
            : Difficulty;

    public static void RunUtility(
        Player player,
        Player opponent)
    {
        foreach (HandSlot slot in player.HandSlots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.CardInSlot!.Type != CardType.Utility)
                continue;

            Card utilityCard = slot.CardInSlot;

            if (utilityCard.Name == "Discard For Energy")
            {
                if (EffectiveDifficulty == BotDifficulty.Easy)
                    continue;

                foreach (HandSlot targetSlot in player.HandSlots)
                {
                    if (targetSlot.IsEmpty)
                        continue;

                    if (targetSlot.SlotNumber == slot.SlotNumber)
                        continue;

                    if (UtilityManager.DiscardForEnergy(
                            player,
                            slot.SlotNumber,
                            targetSlot.SlotNumber))
                    {
                        return;
                    }
                }

                continue;
            }

            if (!ShouldPlayUtility(player, opponent, utilityCard))
                continue;

            if (UtilityManager.BotPlayUtility(
                    player,
                    opponent,
                    slot.SlotNumber))
            {
                return;
            }
        }
    }

    public static void RunPlacement(Player player)
    {
        PlacementManager.BotResolveFatigue(player);

        if (player.ActiveCharacter == null)
        {
            foreach (HandSlot slot in player.HandSlots)
            {
                if (slot.IsEmpty)
                    continue;

                if (slot.CardInSlot!.Type != CardType.Character)
                    continue;

                if (PlacementManager.BotDeployCharacter(
                        player,
                        slot.SlotNumber))
                {
                    break;
                }
            }
        }

        foreach (HandSlot slot in player.HandSlots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.CardInSlot!.Type != CardType.Attack)
                continue;

            if (player.AttackSlot1 == null)
            {
                PlacementManager.BotDeployAttack(
                    player,
                    slot.SlotNumber,
                    1);

                continue;
            }

            if (player.AttackSlot2 == null)
            {
                PlacementManager.BotDeployAttack(
                    player,
                    slot.SlotNumber,
                    2);
            }
        }
    }

    public static void RunAttack(
        Player attacker,
        Player defender)
    {
        if (EffectiveDifficulty != BotDifficulty.Easy &&
            ShouldUseCharacterAbility(attacker, defender))
        {
            AttackManager.UseCharacterAbility(attacker);
        }

        // Re-evaluate after each attack because the board may change.
        int maxActions =
            EffectiveDifficulty == BotDifficulty.Easy ? 1 : 2;

        for (int action = 0; action < maxActions; action++)
        {
            if (MatchManager.IsMatchOver(attacker, defender))
                return;

            int chosen;

            if (EffectiveDifficulty == BotDifficulty.Hard)
            {
                // Hard evaluates both legal attacks against the current board,
                // so it can prioritize lethal damage, character knockouts,
                // shield pressure, and energy efficiency.
                double slot1Value = GetAttackValue(attacker, defender, 1);
                double slot2Value = GetAttackValue(attacker, defender, 2);

                chosen = slot1Value >= slot2Value ? 1 : 2;
                if (Math.Max(slot1Value, slot2Value) <= 0)
                    return;
            }
            else
            {
                // Easy and Medium use a simpler predictable slot-order policy.
                // Easy is additionally limited to one attack per phase above.
                chosen = CanUseAttack(attacker, 1) ? 1 : 2;
                if (!CanUseAttack(attacker, chosen) ||
                    GetAttackValue(attacker, defender, chosen) <= 0)
                {
                    chosen = chosen == 1 ? 2 : 1;
                    if (!CanUseAttack(attacker, chosen) ||
                        GetAttackValue(attacker, defender, chosen) <= 0)
                        return;
                }
            }

            if (!CanUseAttack(attacker, chosen))
                return;

            AttackManager.BotUseAttack(
                attacker,
                defender,
                chosen);
        }
    }

    private static bool CanUseAttack(
        Player player,
        int slotNumber)
    {
        Card? attack =
            slotNumber == 1
                ? player.AttackSlot1
                : player.AttackSlot2;

        if (attack == null)
            return false;

        bool fatigued =
            slotNumber == 1
                ? player.IsFatiguedSlot1
                : player.IsFatiguedSlot2;

        bool resting =
            slotNumber == 1
                ? player.IsRestingSlot1
                : player.IsRestingSlot2;

        bool used =
            slotNumber == 1
                ? player.AttackSlot1UsedThisPhase
                : player.AttackSlot2UsedThisPhase;

        int cost =
            slotNumber == 1
                ? player.AttackSlot1TotalUses
                : player.AttackSlot2TotalUses;

        // Use the same discounted cost as AttackManager so the bot does not
        // incorrectly skip attacks while Discount Coupon is active.
        int effectiveCost = player.GetEnergyCost(cost);

        return !fatigued &&
               !resting &&
               !used &&
               player.Energy >= effectiveCost;
    }

    // Estimates the result of an attack against the current board.
    // AttackManager remains responsible for resolving the actual attack.
    private static double GetAttackValue(
        Player attacker,
        Player defender,
        int slotNumber)
    {
        if (!CanUseAttack(attacker, slotNumber))
            return double.NegativeInfinity;

        Card attack =
            (slotNumber == 1
                ? attacker.AttackSlot1
                : attacker.AttackSlot2)!;

        int useCost =
            slotNumber == 1
                ? attacker.AttackSlot1TotalUses
                : attacker.AttackSlot2TotalUses;

        int effectiveCost = attacker.GetEnergyCost(useCost);

        // Preserve a little energy when possible, but allow a finishing attack.
        // The old check used the undiscounted cost and could reject legal attacks.
        if (attacker.Energy - effectiveCost <= 0 &&
            defender.ActiveCharacter != null &&
            defender.CurrentCharacterHp > 0)
            return -10000;

        int damage =
            AttackManager.GetEffectiveDamage(attacker, attack);

        int shield = defender.Shield;
        int hp = defender.CurrentCharacterHp;
        int energy = defender.Energy;

        bool hasCharacter = defender.ActiveCharacter != null;

        bool sageEntry =
            defender.CharacterNewlyDeployed &&
            !defender.EntryDefenseUsed &&
            defender.ActiveCharacter?.Name == "Sage Red Panda";

        bool deflect =
            CharacterManager.HasStatusEffect(
                defender,
                "DeflectNextHit");

        int landedHits = 0;
        int energyDamage = 0;

        bool characterKilled = false;
        bool lethal = false;

        for (int hit = 0; hit < Math.Max(1, attack.Hits); hit++)
        {
            int hitDamage = damage;

            if (sageEntry)
            {
                hitDamage = Math.Min(hitDamage, 2);
                sageEntry = false;
            }

            if (CharacterManager.HasPassive(
                    defender,
                    "MultiHitResistance") &&
                attack.Hits > 1)
            {
                hitDamage = Math.Max(1, hitDamage - 1);
            }

            if (deflect)
            {
                deflect = false;
                continue;
            }

            landedHits++;

            if (attack.IsPiercing)
            {
                int shieldDamage = hitDamage / 2;

                shield = Math.Max(
                    0,
                    shield - shieldDamage);

                int directDamage =
                    hitDamage - shieldDamage;

                if (hasCharacter)
                {
                    if (directDamage >= hp)
                    {
                        int overflow = directDamage - hp;

                        hp = 0;
                        hasCharacter = false;
                        characterKilled = true;
                        shield = 0;

                        energyDamage += overflow;
                    }
                    else
                    {
                        hp -= directDamage;
                    }
                }
                else
                {
                    energyDamage += directDamage;
                }
            }
            else if (shield > 0)
            {
                bool fortified =
                    defender.CharacterNewlyDeployed &&
                    !defender.EntryDefenseUsed &&
                    defender.ActiveCharacter?.Name ==
                        "Guardian Red Panda";

                if (fortified)
                {
                    shield -= Math.Min(
                        hitDamage,
                        Math.Max(0, shield - 1));
                }
                else
                {
                    shield = Math.Max(
                        0,
                        shield - hitDamage);
                }
            }
            else if (hasCharacter)
            {
                if (hitDamage >= hp)
                {
                    int overflow = hitDamage - hp;

                    hp = 0;
                    hasCharacter = false;
                    characterKilled = true;
                    shield = 0;

                    energyDamage += overflow;
                }
                else
                {
                    hp -= hitDamage;
                }
            }
            else
            {
                energyDamage += hitDamage;
            }

            if (energy - energyDamage <= 0)
            {
                lethal = true;
                break;
            }
        }

        if (CharacterManager.HasPassive(
                defender,
                "ShieldRecovery"))
        {
            shield += Math.Min(2, landedHits);
        }

        if (lethal)
            return 10000 + energyDamage;

        int actualHpDamage =
            defender.ActiveCharacter == null
                ? 0
                : Math.Max(
                    0,
                    defender.CurrentCharacterHp - hp);

        double value = energyDamage * 1.8;

        value += actualHpDamage * 3.0;

        if (characterKilled)
            value += 25.0;

        int netShieldLoss =
            Math.Max(0, defender.Shield - shield);

        value += netShieldLoss * 0.65;

        value -= effectiveCost *
                 (attacker.Energy <= 8 ? 2.0 : 0.8);

        return value;
    }

    // Avoid spending a utility action on an effect that has no meaningful
    // target or benefit. Keep this conservative: UtilityManager remains the
    // authority for whether the play is legal.
    private static bool ShouldPlayUtility(
        Player player,
        Player opponent,
        Card card)
    {
        switch (card.Name)
        {
            case "Energy Potion":
                return player.Energy < player.MaxEnergy &&
                       player.MaxEnergy - player.Energy >= 2;

            case "Energy Drain":
                return opponent.Energy > 1 &&
                       player.Energy < player.MaxEnergy;

            case "Attack Amplifier":
                return player.AttackSlot1 != null ||
                       player.AttackSlot2 != null;

            case "Shield Breaker":
                return opponent.Shield > 0;

            case "Shield Booster":
                return player.Shield <= 2;

            case "Defensive Stance":
                // Only prepare to deflect when the opponent has a deployed attack.
                return opponent.AttackSlot1 != null ||
                       opponent.AttackSlot2 != null;

            case "Discount Coupon":
                return HasAffordablePotentialDiscount(player);

            default:
                return true;
        }
    }

    private static bool HasAffordablePotentialDiscount(Player player)
    {
        if (player.AttackSlot1 != null &&
            !player.IsFatiguedSlot1 && !player.IsRestingSlot1 &&
            !player.AttackSlot1UsedThisPhase &&
            player.AttackSlot1TotalUses > 0)
            return true;

        if (player.AttackSlot2 != null &&
            !player.IsFatiguedSlot2 && !player.IsRestingSlot2 &&
            !player.AttackSlot2UsedThisPhase &&
            player.AttackSlot2TotalUses > 0)
            return true;

        return player.Energy < player.MaxEnergy;
    }

    private static bool ShouldUseCharacterAbility(
        Player player,
        Player opponent)
    {
        if (player.ActiveCharacter == null ||
            player.CharacterNewlyDeployed ||
            player.AbilityUsedThisPhase ||
            player.AbilityCooldownRemaining > 0)
            return false;

        string ability = player.ActiveCharacter.ActiveAbility;
        int cost = ability switch
        {
            "ShieldGain" => player.GetEnergyCost(2),
            "MagicBoost" => player.GetEnergyCost(1),
            "FieldShield" => player.GetEnergyCost(3),
            "PowerSurge" => player.GetEnergyCost(3),
            "AttackBoost" => player.GetEnergyCost(2),
            "Heal" => player.GetEnergyCost(2),
            _ => int.MaxValue
        };

        if (player.Energy < cost)
            return false;

        switch (ability)
        {
            case "ShieldGain":
                return player.Shield < 3 &&
                       (opponent.AttackSlot1 != null || opponent.AttackSlot2 != null);

            case "FieldShield":
                return player.Shield < 4 &&
                       (player.AttackSlot1 != null || player.AttackSlot2 != null);

            case "Heal":
                return player.CurrentCharacterHp < player.ActiveCharacter.HpValue;

            case "MagicBoost":
            case "PowerSurge":
            case "AttackBoost":
                return player.AttackSlot1 != null || player.AttackSlot2 != null;

            default:
                return false;
        }
    }

    public static bool RunNextUtilityAction(
        Player player,
        Player opponent)
    {
        foreach (HandSlot slot in player.HandSlots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.CardInSlot!.Type != CardType.Utility)
                continue;

            Card utilityCard = slot.CardInSlot;
            if (!ShouldPlayUtility(player, opponent, utilityCard))
                continue;

            // Keep Easy bots from using the advanced energy-conversion play.
            if (EffectiveDifficulty == BotDifficulty.Easy &&
                slot.CardInSlot.Name == "Discard For Energy")
            {
                continue;
            }

            if (UtilityManager.BotPlayUtility(
                    player,
                    opponent,
                    slot.SlotNumber))
            {
                return true;
            }
        }

        return false;
    }

    public static bool RunNextPlacementAction(
        Player player,
        int step)
    {
        switch (step)
        {
            case 0:
                return PlacementManager.BotResolveFatiguedSlot(
                    player,
                    1);

            case 1:
                return PlacementManager.BotResolveFatiguedSlot(
                    player,
                    2);

            case 2:
                if (player.ActiveCharacter != null)
                    return false;

                foreach (HandSlot slot in player.HandSlots)
                {
                    if (slot.IsEmpty)
                        continue;

                    if (slot.CardInSlot!.Type != CardType.Character)
                        continue;

                    if (PlacementManager.BotDeployCharacter(
                            player,
                            slot.SlotNumber))
                    {
                        return true;
                    }
                }

                return false;

            case 3:
                if (player.AttackSlot1 != null)
                    return false;

                foreach (HandSlot slot in player.HandSlots)
                {
                    if (slot.IsEmpty)
                        continue;

                    if (slot.CardInSlot!.Type != CardType.Attack)
                        continue;

                    if (PlacementManager.BotDeployAttack(
                            player,
                            slot.SlotNumber,
                            1))
                    {
                        return true;
                    }
                }

                return false;

            case 4:
                if (EffectiveDifficulty == BotDifficulty.Easy)
                    return false;

                if (player.AttackSlot2 != null)
                    return false;

                foreach (HandSlot slot in player.HandSlots)
                {
                    if (slot.IsEmpty)
                        continue;

                    if (slot.CardInSlot!.Type != CardType.Attack)
                        continue;

                    if (PlacementManager.BotDeployAttack(
                            player,
                            slot.SlotNumber,
                            2))
                    {
                        return true;
                    }
                }

                return false;

            default:
                return false;
        }
    }

    public static bool RunNextAttackAction(
        Player attacker,
        Player defender,
        int step)
    {
        switch (step)
        {
            case 0:
                if (EffectiveDifficulty == BotDifficulty.Easy ||
                    !ShouldUseCharacterAbility(attacker, defender))
                {
                    return false;
                }

                return AttackManager.UseCharacterAbility(attacker);

            case 1:
            case 2:
                if (EffectiveDifficulty == BotDifficulty.Easy &&
                    step == 2)
                {
                    return false;
                }

                int slotToUse;

                if (EffectiveDifficulty == BotDifficulty.Hard)
                {
                    // Hard evaluates both attacks and chooses the
                    // more valuable legal option.
                    double slot1Value =
                        GetAttackValue(attacker, defender, 1);

                    double slot2Value =
                        GetAttackValue(attacker, defender, 2);

                    slotToUse =
                        slot1Value >= slot2Value ? 1 : 2;

                    if (Math.Max(slot1Value, slot2Value) <= 0)
                        return false;
                }
                else
                {
                    slotToUse = step;

                    if (GetAttackValue(
                            attacker,
                            defender,
                            slotToUse) <= 0)
                    {
                        return false;
                    }
                }

                return AttackManager.BotUseAttack(
                    attacker,
                    defender,
                    slotToUse);

            default:
                return false;
        }
    }
}