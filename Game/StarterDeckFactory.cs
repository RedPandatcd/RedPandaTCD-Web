namespace RedPandaTCD_Web.Game;

public static class StarterDeckFactory
{

    public static Deck CreateDsaTutorialDeck(DeckStructure structure)
    {
        Deck deck = new()
        {
            Name = "DSA Tutorial Deck",
            Structure = structure
        };

        Card character = CardFactory.CreateSageRedPanda();
        Card firstAttack = CardFactory.CreateFireball();
        Card secondAttack = CardFactory.CreateStab();
        Card sampleOne = CardFactory.CreateIceShards();
        Card sampleTwo = structure == DeckStructure.LinkedList
            ? CardFactory.CreateDoubleClaw()
            : CardFactory.CreateWandererRedPanda();

        // Queue, Stack, and Priority Queue share an Attack (priority 2)
        // and a Character (priority 5) as their two demonstration cards.
        // Linked List keeps its original two-Attack nodes for reordering.
        // Priority Queue temporarily enqueues the sample Character at
        // priority 1 so it cannot replace an Attack in the opening hand;
        // the tutorial's scripted discard restores it to priority 5.
        character.Priority = 5;
        firstAttack.Priority = 4;
        secondAttack.Priority = 4;
        sampleOne.Priority = 2;
        sampleTwo.Priority = sampleTwo.Type == CardType.Character ? 5 : 1;

        switch (structure)
        {
            case DeckStructure.Stack:
                // Stack pops the last card added, so reverse the desired
                // opening order (Character, first attack, second attack).
                deck.Cards.Add(sampleOne);
                deck.Cards.Add(sampleTwo);
                deck.Cards.Add(secondAttack);
                deck.Cards.Add(firstAttack);
                deck.Cards.Add(character);
                break;

            case DeckStructure.LinkedList:
            case DeckStructure.Queue:
            case DeckStructure.PriorityQueue:
                deck.Cards.Add(character);
                deck.Cards.Add(firstAttack);
                deck.Cards.Add(secondAttack);
                deck.Cards.Add(sampleOne);
                deck.Cards.Add(sampleTwo);
                break;

            case DeckStructure.RandomList:
                // Keep every card after the opening Character an Attack,
                // so the opening hand always contains a Character and two Attacks.
                deck.Cards.Add(character);
                deck.Cards.Add(firstAttack);
                deck.Cards.Add(secondAttack);
                deck.Cards.Add(CardFactory.CreateIceShards());
                deck.Cards.Add(CardFactory.CreateMysticSpike());
                deck.Cards.Add(CardFactory.CreateArcaneBullets());
                break;
        }

        return deck;
    }

    public static Deck CreateQueueStarter()
    {
        Deck deck = new Deck
        {
            Name = "Blueprint Queue",
            Structure = DeckStructure.Queue
        };

        deck.Cards.Add(CardFactory.CreateSageRedPanda());
        deck.Cards.Add(CardFactory.CreateManaPulses());
        deck.Cards.Add(CardFactory.CreateEnergyPotion());
        deck.Cards.Add(CardFactory.CreateIceShards());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateSorcererRedPanda());
        deck.Cards.Add(CardFactory.CreateArcaneBullets());
        deck.Cards.Add(CardFactory.CreateMysticSpike());
        deck.Cards.Add(CardFactory.CreateManaBurst());
        deck.Cards.Add(CardFactory.CreateEnergyDrain());
        deck.Cards.Add(CardFactory.CreateDoubleClaw());
        deck.Cards.Add(CardFactory.CreateManaPulses());

        return deck;
    }

    public static Deck CreateStackStarter()
    {
        Deck deck = new Deck
        {
            Name = "The Stack Has Decided",
            Structure = DeckStructure.Stack
        };

        deck.Cards.Add(CardFactory.CreateBerserkerRedPanda());
        deck.Cards.Add(CardFactory.CreateStab());
        deck.Cards.Add(CardFactory.CreateDoubleClaw());
        deck.Cards.Add(CardFactory.CreateDoubleThrust());
        deck.Cards.Add(CardFactory.CreatePiercingStrike());
        deck.Cards.Add(CardFactory.CreateHeavyStrike());
        deck.Cards.Add(CardFactory.CreateStab());
        deck.Cards.Add(CardFactory.CreateAttackAmplifier());
        deck.Cards.Add(CardFactory.CreateDefensiveStance());
        deck.Cards.Add(CardFactory.CreateGuardianRedPanda());
        deck.Cards.Add(CardFactory.CreatePiercingStrike());
        deck.Cards.Add(CardFactory.CreateGuardBreak());

        return deck;
    }

    public static Deck CreatePriorityStarter()
    {
        Deck deck = new Deck
        {
            Name = "Priority Protocol",
            Structure = DeckStructure.PriorityQueue
        };

        Card sorcerer =
            CardFactory.CreateSorcererRedPanda();
        sorcerer.Priority = 5;

        Card wanderer =
            CardFactory.CreateWandererRedPanda();
        wanderer.Priority = 5;

        Card fireball =
            CardFactory.CreateFireball();
        fireball.Priority = 4;

        Card manaBurst =
            CardFactory.CreateManaBurst();
        manaBurst.Priority = 4;

        Card guardBreak =
            CardFactory.CreateGuardBreak();
        guardBreak.Priority = 3;

        Card mysticSpike =
            CardFactory.CreateMysticSpike();
        mysticSpike.Priority = 3;

        Card pounce =
            CardFactory.CreateFireball();
        pounce.Priority = 2;

        Card chainLightning =
            CardFactory.CreateChainLightning();
        chainLightning.Priority = 2;

        Card shieldBooster =
            CardFactory.CreateShieldBooster();
        shieldBooster.Priority = 1;

        Card discountCoupon =
            CardFactory.CreateDiscountCoupon();
        discountCoupon.Priority = 1;

        Card slash = CardFactory.CreateStab();
        slash.Priority = 2;

        Card doubleClaw = CardFactory.CreateDoubleClaw();
        doubleClaw.Priority = 2;

        deck.Cards.Add(sorcerer);
        deck.Cards.Add(wanderer);
        deck.Cards.Add(fireball);
        deck.Cards.Add(manaBurst);
        deck.Cards.Add(guardBreak);
        deck.Cards.Add(mysticSpike);
        deck.Cards.Add(pounce);
        deck.Cards.Add(chainLightning);
        deck.Cards.Add(shieldBooster);
        deck.Cards.Add(discountCoupon);
        deck.Cards.Add(slash);
        deck.Cards.Add(doubleClaw);

        return deck;
    }

    public static Deck CreateRandomStarter()
    {
        Deck deck = new Deck
        {
            Name = "Chaos Draw",
            Structure = DeckStructure.RandomList
        };

        deck.Cards.Add(CardFactory.CreateNomadRedPanda());
        deck.Cards.Add(CardFactory.CreateGuardianRedPanda());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateStab());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateIceShards());
        deck.Cards.Add(CardFactory.CreateChainLightning());
        deck.Cards.Add(CardFactory.CreateManaBurst());
        deck.Cards.Add(CardFactory.CreateShieldBooster());
        deck.Cards.Add(CardFactory.CreateShieldBreaker() );
        deck.Cards.Add(CardFactory.CreateManaPulses());
        deck.Cards.Add(CardFactory.CreateChainLightning());

        return deck;
    }

    public static Deck CreateLinkedStarter()
    {
        Deck deck = new Deck
        {
            Name = "Deck Engineer",
            Structure = DeckStructure.LinkedList
        };

        deck.Cards.Add(CardFactory.CreateWandererRedPanda());
        deck.Cards.Add(CardFactory.CreateNomadRedPanda());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateManaPulses());
        deck.Cards.Add(CardFactory.CreateGuardBreak());
        deck.Cards.Add(CardFactory.CreateArcaneBullets());
        deck.Cards.Add(CardFactory.CreateManaPulses());
        deck.Cards.Add(CardFactory.CreateDiscountCoupon());
        deck.Cards.Add(CardFactory.CreateDiscardForEnergy());
        deck.Cards.Add(CardFactory.CreateStab());
        deck.Cards.Add(CardFactory.CreateDoubleThrust());

        return deck;
    }
}