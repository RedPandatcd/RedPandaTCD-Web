namespace RedPandaTCD_Web.Game;

public static class MatchManager
{
    public static void InitializePlayer(
    Player player,
    int playerIndex)
    {
        player.Energy = 20;
        player.MaxEnergy = 25;
        player.Shield = 0;

        player.ActiveCharacter = null;
        player.CurrentCharacterHp = 0;

        player.AttackSlot1 = null;
        player.AttackSlot2 = null;

        player.IsFatiguedSlot1 = false;
        player.IsFatiguedSlot2 = false;

        player.IsRestingSlot1 = false;
        player.IsRestingSlot2 = false;

        player.AttackSlot1UsesRemaining = 2;
        player.AttackSlot2UsesRemaining = 2;

        player.AttackSlot1TotalUses = 0;
        player.AttackSlot2TotalUses = 0;

        player.CharacterNewlyDeployed = false;
        player.EntryDefenseUsed = false;
        player.CharacterDefeatedSinceLastPlacement = false;

        player.TemporaryAttackBonus = 0;
        player.DiscountActive = false;

        player.StatusEffects.Clear();

        player.UtilityActionsRemaining = 1;
        player.EnergyDrainedThisPhaseCount = 0;
        player.AbilityUsedThisPhase = false;
        player.AttackSlot1UsedThisPhase = false;
        player.AttackSlot2UsedThisPhase = false;

        player.HitDuringPreviousTurn = false;
        player.WasHitThisTurn = false;

        player.HasSurrendered = false;

        HandManager.ClearHand(player);

        DeckStructureManager.PrepareDeckForMatch(player);

        // For the Priority Queue tutorial, keep the sample Character out of
        // the opening hand while the runtime priority queue is initialized.
        // The queue stores its priority key at Add-time, so we can restore
        // the card's real priority (5) before gameplay and still demonstrate
        // that it is drawn first after the tutorial recycles it.
        Card? priorityQueueSampleCharacter = null;
        bool isPriorityQueueTutorial =
            player.Deck.Name.StartsWith("DSA Tutorial", StringComparison.Ordinal) &&
            player.Deck.Structure == DeckStructure.PriorityQueue;

        if (isPriorityQueueTutorial)
        {
            priorityQueueSampleCharacter = player.Deck.Cards
                .LastOrDefault(card => card.Type == CardType.Character);

            if (priorityQueueSampleCharacter != null)
            {
                priorityQueueSampleCharacter.Priority = 1;
            }
        }

        player.Deck.InitializeRuntimeStorage();

        if (priorityQueueSampleCharacter != null)
        {
            priorityQueueSampleCharacter.Priority = 5;
        }

       HandManager.DrawOpeningHand(
    player,
    playerIndex);

        player.AbilityCooldownRemaining = 0;
    }

    public static bool IsMatchOver(
        Player p1,
        Player p2)
    {
        return p1.Energy <= 0 ||
               p2.Energy <= 0 ||
               p1.HasSurrendered ||
               p2.HasSurrendered;
    }

public static bool IsDraw(
    Player p1,
    Player p2)
{
    return !p1.HasSurrendered &&
           !p2.HasSurrendered &&
           p1.Energy <= 0 &&
           p2.Energy <= 0;
}

    public static Player? GetWinner(
        Player p1,
        Player p2)
    {
        if (p1.HasSurrendered)
            return p2;

        if (p2.HasSurrendered)
            return p1;

        if (p1.Energy > 0 &&
            p2.Energy <= 0)
        {
            return p1;
        }

        if (p2.Energy > 0 &&
            p1.Energy <= 0)
        {
            return p2;
        }

        return null;
    }
}