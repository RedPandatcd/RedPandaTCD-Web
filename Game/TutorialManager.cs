namespace RedPandaTCD_Web.Game;
public enum TutorialAction
{
    None,
    Acknowledge,
    SelectUtility,
    PlayUtility,
    DiscardAndDraw,
    PeekDeck,
    ViewDrawPool,
    StartReorderNodes,
    MoveReorderNode,
    FinishReorderNodes,
    DeployCharacter,
    DeployAttack,
    ReplaceAttack,
    RestAttack,
    BeginCharacterReplacement,
    ReplaceCharacter,
    ResolveWandererEntry,
    ObserveAutomaticPlacement,
    UseAttack,
    UseCharacterAbility,
    ContinuePhase,
    PrepareRecycle,
    ObserveRecycle,
    ViewDiscardPile,
    Surrender,
    ObserveBoard,
    ReturnToTutorial,
    FinishTutorial
}
public sealed class TutorialStep
{
    public int Number { get; init; }
    public string Title { get; init; } = "";
    public string Message { get; init; } = "";
    public TutorialAction RequiredAction { get; init; }
    public string HighlightTarget { get; init; } = "";
    public bool RestrictInput { get; init; }
    public bool IsInformational =>
        RequiredAction ==
        TutorialAction.Acknowledge;
}
public static class TutorialManager
{
    /*
        LEARN TO PLAY DECK
        These are the 12 cards the player must use
        for the Learn to Play Tutorial deck.
    */
    public static readonly IReadOnlyList<string>
        LearnToPlayDeckCardIds =
        new List<string>
        {
            // Characters
            "CM1",
            "CN1",
            "AM3",
            "AM6",
            "AP4",
            "AP7",
            "AM7",
"UM1",
"UN1",
"UN4",
"UM2",
"UN3"
        };
public static readonly IReadOnlyList<string>
    LearnToPlayDeckOrder =
    new List<string>
    {
        "UM1",
        "CM1",
        "AM6",
        "AP4",
        "AM3",
        "AP7",
        "CN1",
        "UN4",
        "UN1",
        "AM7",
        "UM2",
        "UN3"
    };

private static readonly IReadOnlyList<TutorialStep>
    LearnToPlaySteps =
    new List<TutorialStep>
    {
        new()
        {
            Number = 0,
            Title = "Welcome to the Battlefield",
            Message =
                "Welcome to the battlefield! Your goal is to reduce your opponent's Energy " +
                "to 0. You'll learn the rules by playing through a real match.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-board",
            RestrictInput =
                true
        },
        new()
        {
            Number = 1,
            Title = "Your Opponent's Field",
            Message =
                "Your opponent's Character and deployed Attacks appear here. Watch this " +
                "area to see their current setup.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "enemy-field",
            RestrictInput =
                true
        },
        new()
        {
            Number = 2,
            Title = "Your Battlefield",
            Message =
                "Your Character and deployed Attacks appear here. This is where you'll " +
                "build your own field.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-field",
            RestrictInput =
                true
        },
        new()
        {
            Number = 3,
            Title = "Managing Energy",
            Message =
                "Cards and abilities can cost Energy. Keep an eye on your supply—if your " +
                "Energy reaches 0, you can lose the battle.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-energy",
            RestrictInput =
                true
        },
        new()
        {
            Number = 4,
            Title = "Your Hand of Cards",
            Message =
                "Cards you draw appear in your hand. From here, you can play Utility cards, " +
                "deploy Characters or Attacks, and discard cards when an action allows it.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-hand",
            RestrictInput =
                true
        },
        new()
        {
            Number = 5,
            Title = "Understanding Your Deck",
            Message =
                "Your deck's data structure determines how cards are stored and drawn. The " +
                "Data Structure Tutorials explain how each structure works.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 6,
            Title = "Who Acts First?",
            Message =
                "Initiative determines who acts first in each phase. The player with " +
                "Initiative goes first, and Initiative swaps when a new turn begins.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "initiative",
            RestrictInput =
                true
        },
        new()
        {
            Number = 7,
            Title = "The Four Battle Phases",
            Message =
                "Each turn follows four phases: Utility, Placement, Attack, then End. Both " +
                "players act during each phase, with the Initiative holder acting first.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "phase-bar",
            RestrictInput =
                true
        },

        // LESSON 2 — YOUR FIRST TURN

        new()
        {
            Number = 8,
            Title = "Using Utility Cards",
            Message =
                "Utility cards provide special effects. Select the highlighted Energy " +
                "Potion to inspect it.",
            RequiredAction =
                TutorialAction.SelectUtility,
            HighlightTarget =
                "tutorial-utility",
            RestrictInput =
                true
        },
        new()
        {
            Number = 9,
            Title = "Play Energy Potion",
            Message =
                "Energy Potion restores Energy. Check its effect, then play it using the " +
                "Utility action.",
            RequiredAction =
                TutorialAction.PlayUtility,
            HighlightTarget =
                "utility-action",
            RestrictInput =
                true
        },
        new()
        {
            Number = 10,
            Title = "Reading the Battle Log",
            Message =
                "The Battle Log records important events, including card effects and " +
                "changes during battle. Review it, then press Continue.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "battle-log",
            RestrictInput =
                true
        },
        new()
        {
            Number = 11,
            Title = "The Placement Phase",
            Message =
                "During Placement, prepare your field by deploying Characters and Attacks. " +
                "You'll set up your first cards now.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "phase-placement",
            RestrictInput =
                true
        },
        new()
        {
            Number = 12,
            Title = "Deploy Sorcerer Red Panda",
            Message =
                "Deploy Sorcerer Red Panda into your Character slot.",
            RequiredAction =
                TutorialAction.DeployCharacter,
            HighlightTarget =
                "sorcerer",
            RestrictInput =
                true
        },
        new()
        {
            Number = 13,
            Title = "Character Abilities",
            Message =
                "Characters can have Entry, Passive, and Active abilities. Entry effects " +
                "trigger when a Character is deployed, Passives apply under their " +
                "conditions, and Actives are used deliberately.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 14,
            Title = "Entry Ability: Magic Surge",
            Message =
                "Sorcerer Red Panda's Entry ability, Magic Surge, gives your Magic Attacks " +
                "+1 Damage during the turn it enters play.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 15,
            Title = "Deploy Fireball",
            Message =
                "Deploy Fireball. It's a Magic Attack, so it benefits from Sorcerer's Magic " +
                "damage bonuses.",
            RequiredAction =
                TutorialAction.DeployAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 16,
            Title = "Stacking Ability Bonuses",
            Message =
                "Fireball now benefits from two effects: Magic Surge adds +1 Damage this " +
                "turn, and Arcane Focus adds another +1 while no Physical Attack is " +
                "deployed.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 17,
            Title = "Conditional Passive Abilities",
            Message =
                "Deploy Piercing Strike. It's Physical, so deploying it disables Arcane " +
                "Focus. Magic Surge's +1 Damage still applies this turn.",
            RequiredAction =
                TutorialAction.DeployAttack,
            HighlightTarget =
                "piercing-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 18,
            Title = "Your Field Is Ready",
            Message =
                "Your Character and Attacks are in place. Press Continue to finish your " +
                "Placement action.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 19,
            Title = "Your First Attack",
            Message =
                "It's the Attack phase. Use Piercing Strike first and watch how its " +
                "piercing effect interacts with Shield.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "piercing-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 20,
            Title = "Piercing Through Shield",
            Message =
                "Piercing Strike can damage the Character through its Shield. Check the " +
                "battlefield and Battle Log to see the result.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "enemy-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 21,
            Title = "Attacking a Shield",
            Message =
                "Now use Fireball. Unlike a piercing Attack, a normal hit damages Shield " +
                "before it damages HP.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 22,
            Title = "Attack Phase Complete",
            Message =
                "Your Attack action is complete. Press Continue to move on; the End phase " +
                "resolves automatically when the phase sequence reaches it.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 23,
            Title = "Understanding Defense",
            Message =
                "Your opponent has attacked. Compare your Character's Shield and HP with " +
                "the Battle Log to see how the incoming damage was handled.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 24,
            Title = "Temporary Effects Expire",
            Message =
                "Magic Surge lasts only for the turn Sorcerer enters play, so it has " +
                "expired. Piercing Strike is still deployed, which keeps Arcane Focus " +
                "inactive.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },

        // LESSON 3 — COMBAT & FIELD MANAGEMENT

        new()
        {
            Number = 25,
            Title = "Discard to Draw",
            Message =
                "You can use a Utility action even without playing a Utility card. Discard " +
                "the highlighted card to draw a replacement.",
            RequiredAction =
                TutorialAction.DiscardAndDraw,
            HighlightTarget =
                "discard-card",
            RestrictInput =
                true
        },
        new()
        {
            Number = 26,
            Title = "Where Discarded Cards Go",
            Message =
                "The discarded card has moved to your Discard Pile, and a replacement has " +
                "become available in your hand.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 27,
            Title = "Complete Your Utility Phase",
            Message =
                "You've finished your Utility action. Press Continue to move to the next " +
                "phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 28,
            Title = "Replace an Attack",
            Message =
                "Replace Piercing Strike with Ice Shards, a Magic Attack that hits twice.",
            RequiredAction =
                TutorialAction.ReplaceAttack,
            HighlightTarget =
                "piercing-slot",
            RestrictInput =
                true
        },
        new()
        {
            Number = 29,
            Title = "Passive Bonuses Revisited",
            Message =
                "Both deployed Attacks are Magic now, so Arcane Focus is active again and " +
                "gives them +1 Damage.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 30,
            Title = "Complete Your Placement Phase",
            Message =
                "Your Placement changes are complete. Press Continue to enter the Attack " +
                "phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 31,
            Title = "Using Active Abilities",
            Message =
                "Active abilities are activated manually and may cost Energy or have a " +
                "cooldown. Use Sorcerer Red Panda's Active ability now.",
            RequiredAction =
                TutorialAction.UseCharacterAbility,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 32,
            Title = "Combine Ability Bonuses",
            Message =
                "Use Fireball again. It gains +1 Damage from Arcane Focus and +1 from " +
                "Sorcerer's Active ability.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 33,
            Title = "Understanding Fatigue",
            Message =
                "Fireball has reached its use limit and is now Fatigued. You'll need to " +
                "resolve its Fatigue during Placement before using it again.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 34,
            Title = "Multi-Hit Attacks: Ice Shards",
            Message =
                "Use Ice Shards. It has two hits, so one use resolves as two separate hits.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "multi-hit-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 35,
            Title = "Resolving Multiple Hits",
            Message =
                "Each hit resolves separately. Check the Battle Log to compare how the hits " +
                "affect your opponent's Shield and HP.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-log",
            RestrictInput =
                true
        },
        new()
        {
            Number = 36,
            Title = "Finish the Attack Phase",
            Message =
                "Your attacks are finished for now. Press Continue to complete your Attack " +
                "action.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 37,
            Title = "End-of-Turn Recovery",
            Message =
                "The End phase is where end-of-turn effects resolve. Shield and " +
                "other turn-based effects are updated, cards may be drawn, and " +
                "Energy is recovered. Check the battlefield and Battle Log " +
                "to see the changes.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 38,
           Title = "Ability Cooldowns",
            Message =
                "End-of-turn processing can also affect ability availability. " +
                "The Sorcerer's Active ability has a cooldown, which determines " +
                "when it can be used again. Some abilities need time before " +
                "they become available again.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },

        // LESSON 4 — DECKS & ADVANCED PLACEMENT

        new()
        {
            Number = 39,
            Title = "Peek at Your Deck",
            Message =
                "Use your Utility action to inspect your deck. Click your deck and choose " +
                "Peek.",
            RequiredAction =
                TutorialAction.PeekDeck,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 40,
            Title = "First In, First Out (FIFO)",
            Message =
                "Your deck uses a Queue: cards are drawn in First-In, First-Out order. The " +
                "card that entered the Queue earliest is retrieved first.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 41,
            Title = "Complete Your Utility Phase",
            Message =
                "You've finished Peek. Press Continue to leave the Utility phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 42,
            Title = "Rest a Fatigued Attack",
            Message =
                "Fireball is still Fatigued. Rest it during Placement to restore its uses " +
                "before trying it again.",
            RequiredAction =
                TutorialAction.RestAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 43,
            Title = "Replace Your Character",
            Message =
                "You can replace your Character during Placement. Select your current " +
                "Character slot to begin.",
            RequiredAction =
                TutorialAction.BeginCharacterReplacement,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 44,
            Title = "Deploy Wanderer Red Panda",
            Message =
                "Deploy Wanderer Red Panda into your Character slot, replacing Sorcerer.",
            RequiredAction =
                TutorialAction.ReplaceCharacter,
            HighlightTarget =
                "wanderer",
            RestrictInput =
                true
        },
        new()
        {
            Number = 45,
            Title = "Entry Ability: Quick Setup",
            Message =
                "Wanderer's Entry ability reduces the buildup cost of one deployed Attack " +
                "by 2. Select the Attack you want to adjust.",
            RequiredAction =
                TutorialAction.ResolveWandererEntry,
            HighlightTarget =
                "wanderer-entry",
            RestrictInput =
                true
        },
        new()
        {
            Number = 46,
            Title = "Inspect Your Character",
            Message =
                "Inspect Wanderer on the field to review its abilities. Each Character has " +
                "its own Entry, Passive, and Active effects.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "wanderer",
            RestrictInput =
                true
        },
        new()
        {
            Number = 47,
            Title = "Complete Your Placement Phase",
            Message =
                "Wanderer's Entry effect has been resolved. Press Continue to finish " +
                "Placement.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 48,
            Title = "Ice Shards: A Second Look",
            Message =
                "Use Ice Shards again. Notice how a multi-hit Attack resolves and how its " +
                "damage interacts with the opponent's remaining Shield and HP.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "multi-hit-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 49,
            Title = "Battle Basics Complete",
            Message =
                "You've practiced the core battle flow, deploying cards, using abilities, " +
                "attacking, managing Fatigue, taking Utility actions, and reading the " +
                "Battle Log.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-board",
            RestrictInput =
                true
        },

        // LESSON 5 — YOU'RE READY

        new()
        {
            Number = 50,
            Title = "Victory, Defeat, and Draws",
            Message =
                "A match can end in Victory, Defeat, or a Draw, depending on how the battle " +
                "concludes.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-board",
            RestrictInput =
                true
        },
        new()
        {
            Number = 51,
            Title = "Surrendering a Match",
            Message =
                "You can also end a match by surrendering. Surrender now to continue this " +
                "tutorial.",
            RequiredAction =
                TutorialAction.Surrender,
            HighlightTarget =
                "surrender-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 52,
            Title = "Review the Final Battlefield",
            Message =
                "Choose Observe Board to inspect the final battlefield and review the " +
                "Battle Log.",
            RequiredAction =
                TutorialAction.ObserveBoard,
            HighlightTarget =
                "observe-board-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 53,
            Title = "Tutorial Complete: Ready for Battle",
            Message =
                "You've completed the Battle Tutorial. Return to the Tutorial menu whenever " +
                "you're ready.",
            RequiredAction =
                TutorialAction.ReturnToTutorial,
            HighlightTarget =
                "return-to-tutorial",
            RestrictInput =
                true
        }
    };

private static readonly IReadOnlyList<TutorialStep> QueueSteps =
        BuildDsaSteps(DeckStructure.Queue);

    private static readonly IReadOnlyList<TutorialStep> PriorityQueueSteps =
        BuildDsaSteps(DeckStructure.PriorityQueue);

    private static readonly IReadOnlyList<TutorialStep> StackSteps =
        BuildDsaSteps(DeckStructure.Stack);

    private static readonly IReadOnlyList<TutorialStep> RandomListSteps =
        BuildDsaSteps(DeckStructure.RandomList);

    private static readonly IReadOnlyList<TutorialStep> LinkedListSteps =
        BuildDsaSteps(DeckStructure.LinkedList);

    private static IReadOnlyList<TutorialStep> BuildDsaSteps(
        DeckStructure structure)
    {
        string structureName = structure switch
        {
            DeckStructure.Queue => "Queue",
            DeckStructure.PriorityQueue => "Priority Queue",
            DeckStructure.Stack => "Stack",
            DeckStructure.RandomList => "Random List",
            DeckStructure.LinkedList => "Linked List",
            _ => "Deck"
        };

        TutorialAction utilityAction = structure switch
        {
            DeckStructure.Queue or DeckStructure.Stack => TutorialAction.PeekDeck,
            DeckStructure.RandomList => TutorialAction.ViewDrawPool,
            DeckStructure.LinkedList => TutorialAction.StartReorderNodes,
            _ => TutorialAction.Acknowledge
        };

        string utilityTitle = structure switch
        {
            DeckStructure.Queue => "Peek at the Queue",
            DeckStructure.Stack => "Peek at the Stack",
            DeckStructure.RandomList => "Inspect the Random List",
            DeckStructure.LinkedList => "Reorder the Linked List",
            _ => "Priority Queue Utility Phase"
        };

        string utilityMessage = structure switch
        {
            DeckStructure.Queue =>
                "Click the deck and choose Peek to inspect the next two cards. After you inspect them, both sample cards move to the discard pile for the recycling demonstration. Your three starting cards stay in your hand.",
            DeckStructure.Stack =>
                "Click the deck and choose Peek to inspect the top two cards. After you inspect them, both sample cards move to the discard pile for the recycling demonstration. Your three starting cards stay in your hand.",
            DeckStructure.RandomList =>
                "Click the deck and choose View Draw Pool to inspect the three cards remaining in the random pool. Those three sample cards move to the discard pile after the inspection. Your starting Character and two Attacks stay in your hand.",
            DeckStructure.LinkedList =>
                "Click the deck and choose Reorder Nodes. Move one of the two remaining nodes and finish. These nodes stay connected in the deck; they will be drawn before the two cards already in the discard pile are recycled.",
            _ =>
                "The Priority Queue has no manual peek or reorder action. Continue to place the sample Attack, Ice Shards (priority 2), and Character, Wanderer Red Panda (priority 5), in the discard pile. When recycled, the Character should be drawn first because it has higher priority."
        };

        List<TutorialStep> steps = new();
        void Add(int number, string title, string message,
            TutorialAction action, string target) => steps.Add(new TutorialStep
            {
                Number = number,
                Title = title,
                Message = message,
                RequiredAction = action,
                HighlightTarget = target,
                RestrictInput = true
            });

        Add(0, $"How the {structureName} Works",
            structure switch
            {
                DeckStructure.Queue => "A Queue draws from the front: first in, first out (FIFO). The two sample cards will be recycled in the order they entered the discard pile.",
                DeckStructure.Stack => "A Stack draws from the top: last in, first out (LIFO). Recycled cards are pushed onto the top, so the last card pushed is drawn first.",
                DeckStructure.PriorityQueue => "A Priority Queue retrieves higher-priority cards first. Watch the Character (priority 5) come before Ice Shards (priority 2), even if discard order is different.",
                DeckStructure.RandomList => "A Random List chooses a card randomly from the remaining pool. You will inspect the three-card pool and later see cards drawn randomly after they are recycled.",
                _ => "A Linked List stores cards as connected nodes. Drawing removes the head node; reordering changes which existing node comes first. Recycled cards are randomized among themselves and appended to the tail."
            }, TutorialAction.Acknowledge, "player-deck");

        Add(steps.Count, utilityTitle, utilityMessage, utilityAction,
            structure == DeckStructure.LinkedList ? "player-deck" :
            structure == DeckStructure.PriorityQueue ? "discard-pile" : "player-deck");

        if (structure == DeckStructure.LinkedList)
        {
            Add(steps.Count, "Move a Node",
                "Move one of the two nodes to a different position. This changes the order in which those two cards will be drawn.",
                TutorialAction.MoveReorderNode, "linked-list-nodes");
            Add(steps.Count, "Confirm the New Order",
                "Finish reordering. The same two nodes remain in the deck in the order you selected; they are not discarded as part of this demonstration.",
                TutorialAction.FinishReorderNodes, "linked-list-nodes");
        }

        Add(steps.Count, "Continue to Placement",
            "The Utility demonstration is complete. Press Continue to enter Placement. The three cards in your hand are your Character and two Attacks.",
            TutorialAction.ContinuePhase, "continue-button");

        if (structure == DeckStructure.Stack)
        {
            Add(steps.Count, "Watch Automatic Stack Placement",
                "Stack placement is automatic. Watch the Character and both Attacks move from your hand onto the field. The hand should be empty before you continue.",
                TutorialAction.ObserveAutomaticPlacement, "player-field");
        }
        else
        {
            Add(steps.Count, "Place Your Character",
                "During Placement, choose Sage Red Panda from your hand and deploy it to the Character slot. This uses the Character card from your hand.",
                TutorialAction.DeployCharacter, "player-character");
            Add(steps.Count, "Place Your First Attack",
                "Choose either Attack card from your hand and place it in an empty Attack slot. Placement removes the card from your hand; you will use the deployed attack during the Attack phase.",
                TutorialAction.DeployAttack, "player-field");
            Add(steps.Count, "Place Your Second Attack",
                "Place your remaining Attack card in the other Attack slot. Your Character and both Attacks should now be on the field and your hand should be empty.",
                TutorialAction.DeployAttack, "player-field");
        }

        Add(steps.Count, "Continue to Attack",
            "Placement is complete and your hand should be empty. Press Continue to enter the Attack phase; you will use the Character's deployed attacks there.",
            TutorialAction.ContinuePhase, "continue-button");
        Add(steps.Count, "Use Your First Attack",
            "During the Attack phase, click one of your deployed attacks to resolve it against the opponent.",
            TutorialAction.UseAttack, "player-field");
        Add(steps.Count, "Use Your Second Attack",
            "Now use the other deployed attack. The cards remain deployed on the field; using them is separate from placing them and does not put them back in your hand.",
            TutorialAction.UseAttack, "player-field");
        Add(steps.Count, "Inspect the Discard Pile",
            "Before the End phase resolves, click the discard pile to inspect its cards and order. This viewer is available only during this DSA tutorial. Close it when you have finished looking.",
            TutorialAction.ViewDiscardPile, "discard-pile");
        Add(steps.Count, "Watch the End-Phase Recycling",
            structure switch
            {
                DeckStructure.Queue => "Press Continue. With the Queue empty, its discarded cards are enqueued in discard order. They will be drawn from the front, first in, first out.",
                DeckStructure.Stack => "Press Continue. With the Stack empty, each discarded card is pushed onto the top. The last card pushed becomes the next card drawn.",
                DeckStructure.PriorityQueue => "Press Continue. With the Priority Queue empty, the recycled Character (priority 5) is reinserted ahead of Ice Shards (priority 2), regardless of discard order.",
                DeckStructure.RandomList => "Press Continue. With the random pool empty, the discarded cards return to the pool. Future draws choose randomly rather than following discard order.",
                _ => "Press Continue. The two reordered Linked List nodes are drawn first. Once they are gone, the two cards from the discard pile are randomized and appended to the tail as new nodes; the original nodes are not shuffled or recycled."
            }, TutorialAction.ContinuePhase, "continue-button");
        Add(steps.Count, "Observe the Recycled Cards",
            structure switch
            {
                DeckStructure.Queue => "Back in Utility, the two recycled cards should be in your hand and the deck should be empty. The Queue draws them in queue order.",
                DeckStructure.Stack => "Back in Utility, the two recycled cards should be in your hand and the deck should be empty. The Stack draws in reverse push order.",
                DeckStructure.PriorityQueue => "Back in Utility, the deck should be empty and both recycled cards should be in your hand. Wanderer Red Panda (priority 5) should appear before Ice Shards (priority 2).",
                DeckStructure.RandomList => "Back in Utility, three recycled cards should be in your hand and the random pool should be empty. Their hand order reflects random retrieval.",
                _ => "Back in Utility, four cards should be in your hand and the deck should be empty: the two original reordered nodes were drawn first, followed by the two recycled cards that were shuffled before being appended to the tail."
            }, TutorialAction.Acknowledge, "player-hand");
        Add(steps.Count, $"{structureName} Tutorial Complete",
            "You've inspected the Utility action, completed Placement, used your deployed attacks, inspected the discard pile, and observed how this data structure retrieves recycled cards.",
            TutorialAction.FinishTutorial, "player-deck");

        return steps;
    }

    public static void StartTutorial(
    TutorialState state,
    TutorialType tutorial)
{
    if (tutorial == TutorialType.None)
    {
        return;
    }
    state.Start(
        tutorial);
    ApplyCurrentStep(
        state);
}
    public static void StartLearnToPlay(
    TutorialState state)
{
    StartTutorial(
        state,
        TutorialType.LearnToPlay);
}
public static int GetStepCount(
    TutorialState state)
{
    if (!state.IsActive ||
        state.CurrentTutorial ==
            TutorialType.None)
    {
        return 0;
    }
    return GetSteps(
        state.CurrentTutorial).Count;
}
public static bool IsFinalStep(
    TutorialState state)
{
    TutorialStep? step =
        GetCurrentStep(
            state);
    if (step == null)
    {
        return false;
    }
    int count =
        GetStepCount(
            state);
    return count > 0 &&
           step.Number ==
               count - 1;
}
public static int GetStepCount(
    TutorialType tutorial)
{
    return GetSteps(
        tutorial).Count;
}
    public static TutorialStep? GetCurrentStep(
        TutorialState state)
    {
        if (!state.IsActive ||
            state.CurrentTutorial ==
                TutorialType.None)
        {
            return null;
        }
        IReadOnlyList<TutorialStep> steps =
            GetSteps(
                state.CurrentTutorial);
        if (state.CurrentStep < 0 ||
            state.CurrentStep >= steps.Count)
        {
            return null;
        }
        return steps[
            state.CurrentStep];
    }
    public static string GetInstructionTitle(
        TutorialState state)
    {
        return GetCurrentStep(state)?.Title ??
               "";
    }
    public static string GetInstructionMessage(
        TutorialState state)
    {
        return GetCurrentStep(state)?.Message ??
               "";
    }
    public static string GetHighlightTarget(
        TutorialState state)
    {
        return GetCurrentStep(state)?.HighlightTarget ??
               "";
    }
    public static TutorialAction GetRequiredAction(
        TutorialState state)
    {
        return GetCurrentStep(state)?.RequiredAction ??
               TutorialAction.None;
    }
    public static bool RequiresAction(
        TutorialState state,
        TutorialAction action)
    {
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null)
        {
            return false;
        }
        return step.RequiredAction ==
               action;
    }
    /*
        Called when the player performs an actual action
        in Battle.
        Returns true only when that action was the action
        required by the current Tutorial step.
    */
    public static bool NotifyAction(
        TutorialState state,
        TutorialAction action)
    {
        if (!state.IsActive ||
            state.IsCompleted)
        {
            return false;
        }
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null ||
            step.RequiredAction != action)
        {
            return false;
        }
        Advance(
            state);
        return true;
    }
    public static bool AcknowledgeInstruction(
        TutorialState state)
    {
        return NotifyAction(
            state,
            TutorialAction.Acknowledge);
    }
    public static bool CanPerformAction(
        TutorialState state,
        TutorialAction action)
    {
        /*
            Outside a Tutorial the game remains completely
            unrestricted.
        */
        if (!state.IsActive)
        {
            return true;
        }
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null)
        {
            return true;
        }
        if (!step.RestrictInput)
        {
            return true;
        }
        return step.RequiredAction ==
               action;
    }
    public static bool IsInputRestricted(
        TutorialState state)
    {
        TutorialStep? step =
            GetCurrentStep(
                state);
        return step?.RestrictInput ??
               false;
    }
    public static bool ShouldHighlight(
        TutorialState state,
        string target)
    {
        if (!state.IsActive ||
            string.IsNullOrWhiteSpace(target))
        {
            return false;
        }
        string currentTarget =
            GetHighlightTarget(
                state);
        return string.Equals(
            currentTarget,
            target,
            StringComparison.OrdinalIgnoreCase);
    }
    public static void CompleteTutorial(
        TutorialState state)
    {
        if (!state.IsActive)
        {
            return;
        }
        state.CompleteCurrentTutorial();
    }
    public static void ExitTutorial(
        TutorialState state)
    {
        state.EndTutorial();
    }
    private static void Advance(
        TutorialState state)
    {
        IReadOnlyList<TutorialStep> steps =
            GetSteps(
                state.CurrentTutorial);
        int nextStep =
            state.CurrentStep + 1;
        if (nextStep >= steps.Count)
        {
            CompleteTutorial(
                state);
            return;
        }
        state.AdvanceStep();
        ApplyCurrentStep(
            state);
    }
    private static void ApplyCurrentStep(
        TutorialState state)
    {
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null)
        {
            state.RequiredAction = "";
            state.InputRestricted = false;
            return;
        }
        state.RequiredAction =
            step.RequiredAction.ToString();
        state.InputRestricted =
            step.RestrictInput;
        state.InstructionAcknowledged =
            false;
    }
    public static bool IsLearnToPlayDeckCard(
        Card card)
    {
        return LearnToPlayDeckCardIds.Contains(
            card.Id);
    }
    public static bool IsLearnToPlayDeckComplete(
        Deck deck)
    {
        if (deck.Cards.Count !=
            LearnToPlayDeckCardIds.Count)
        {
            return false;
        }
        return LearnToPlayDeckCardIds.All(
            requiredId =>
                deck.Cards.Any(card =>
                    card.Id == requiredId));
    }
    public static string GetLearnToPlayDeckProgress(
        Deck deck)
    {
        int collected =
            LearnToPlayDeckCardIds.Count(
                requiredId =>
                    deck.Cards.Any(card =>
                        card.Id == requiredId));
        return
            $"{collected} / " +
            $"Tutorial Progress: {LearnToPlayDeckCardIds.Count} required cards";
    }
private static IReadOnlyList<TutorialStep> GetSteps(
    TutorialType tutorial)
{
    return tutorial switch
    {
        TutorialType.LearnToPlay =>
            LearnToPlaySteps,
        TutorialType.Queue =>
            QueueSteps,
        TutorialType.PriorityQueue =>
            PriorityQueueSteps,
        TutorialType.Stack =>
            StackSteps,
        TutorialType.RandomList =>
            RandomListSteps,
        TutorialType.LinkedList =>
            LinkedListSteps,
        _ =>
            Array.Empty<TutorialStep>()
    };
}
    public static bool IsLearnToPlayDeckOrderCorrect(
    Deck deck)
{
    if (deck.Cards.Count !=
        LearnToPlayDeckOrder.Count)
    {
        return false;
    }
    for (int i = 0;
         i < LearnToPlayDeckOrder.Count;
         i++)
    {
        if (deck.Cards[i].Id !=
            LearnToPlayDeckOrder[i])
        {
            return false;
        }
    }
    return true;
}
public static string GetLearnToPlayDeckOrderHint(
    Deck deck)
{
    if (deck.Cards.Count !=
        LearnToPlayDeckOrder.Count)
    {
        return
            "Add all required cards before arranging the Queue.";
    }
    for (int i = 0;
         i < LearnToPlayDeckOrder.Count;
         i++)
    {
        string requiredId =
            LearnToPlayDeckOrder[i];
        Card? requiredCard =
            deck.Cards.FirstOrDefault(card =>
                card.Id == requiredId);
        if (requiredCard == null)
        {
            continue;
        }
        if (deck.Cards[i].Id != requiredId)
        {
            return
                $"Position {i + 1} should be " +
                $"{requiredCard.Name}.";
        }
    }
    return
        "Queue order is correct.";
}
public static string GetLearnToPlayDeckInstruction(
    string cardId)
{
    return cardId switch
    {
        "UM1" =>
    "Your next card is Energy Potion.\n\nMagic • Utility\n\nUse the Utilities and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"CM1" =>
    "Your next card is Sorcerer Red Panda.\n\nMagic • Character\n\nUse the Characters and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AM6" =>
    "Your next card is Fireball.\n\nMagic • Attack\n\nUse the Attacks and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AP4" =>
    "Your next card is Piercing Strike.\n\nPhysical • Attack\n\nUse the Attacks and Physical filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AM3" =>
    "Your next card is Ice Shards.\n\nMagic • Attack\n\nUse the Attacks and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AP7" =>
    "Your next card is Pounce.\n\nPhysical • Attack\n\nUse the Attacks and Physical filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"CN1" =>
    "Your next card is Wanderer Red Panda.\n\nNeutral • Character\n\nUse the Characters and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UN4" =>
    "Your next card is Discard For Energy.\n\nNeutral • Utility\n\nUse the Utilities and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UN1" =>
    "Your next card is Shield Booster.\n\nNeutral • Utility\n\nUse the Utilities and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AM7" =>
    "Your next card is Chain Lightning.\n\nMagic • Attack\n\nUse the Attacks and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UM2" =>
    "Your next card is Energy Drain.\n\nMagic • Utility\n\nUse the Utilities and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UN3" =>
    "Your next card is Discount Coupon.\n\nNeutral • Utility\n\nUse the Utilities and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

        _ =>
            "Find the highlighted Tutorial card."
    };
}
}
