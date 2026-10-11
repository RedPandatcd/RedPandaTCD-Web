namespace RedPandaTCD_Web.Game;
using System.Text.Json;

public static class DeckManager
{
    public static List<Deck> Decks { get; } =
        new List<Deck>();

    public static void InitializeDefaultDecks()
    {
        if (Decks.Count > 0)
        {
            return;
        }

        Decks.Add(
            StarterDeckFactory.CreateQueueStarter());

        Decks.Add(
            StarterDeckFactory.CreateStackStarter());

        Decks.Add(
            StarterDeckFactory.CreatePriorityStarter());

        Decks.Add(
            StarterDeckFactory.CreateRandomStarter());

        Decks.Add(
            StarterDeckFactory.CreateLinkedStarter());
    }

    public static Deck? GetDeck(int index)
    {
        InitializeDefaultDecks();

        if (index < 0 ||
            index >= Decks.Count)
        {
            return null;
        }

        return DeckCloner.Clone(
            Decks[index]);
    }
    public static bool SaveDeck(
    Deck deck,
    string name,
    out string errorMessage)
{
   InitializeDefaultDecks();

    if (string.IsNullOrWhiteSpace(name))
    {
        errorMessage =
            "Deck name cannot be empty.";

        return false;
    }

    if (!DeckBuilder.ValidateDeck(
            deck,
            out errorMessage))
    {
        return false;
    }

    Deck savedDeck =
        DeckCloner.Clone(deck);

    savedDeck.Name =
        name.Trim();

    Decks.Add(savedDeck);

    errorMessage =
        string.Empty;

    return true;
}

public static bool DeleteDeck(
    int index)
{
    InitializeDefaultDecks();

    if (index < 0 ||
        index >= Decks.Count)
    {
        return false;
    }

    Decks.RemoveAt(index);

    return true;
}
public static bool UpdateDeck(
    int index,
    Deck deck,
    string name,
    out string errorMessage)
{
    InitializeDefaultDecks();

    if (index < 0 ||
        index >= Decks.Count)
    {
        errorMessage =
            "Deck not found.";

        return false;
    }

    if (string.IsNullOrWhiteSpace(name))
    {
        errorMessage =
            "Deck name cannot be empty.";

        return false;
    }

    if (!DeckBuilder.ValidateDeck(
            deck,
            out errorMessage))
    {
        return false;
    }

    Deck updatedDeck =
        DeckCloner.Clone(deck);

    updatedDeck.Name =
        name.Trim();

    Decks[index] =
        updatedDeck;

    errorMessage =
        string.Empty;

    return true;
}
public static void DeleteTutorialDeck()
{
    for (int i = Decks.Count - 1; i >= 0; i--)
    {
        if (string.Equals(
                Decks[i].Name,
                "Tutorial Deck",
                StringComparison.OrdinalIgnoreCase))
        {
            Decks.RemoveAt(i);
        }
    }
}
private const int StarterDeckCount = 5;


public static string ExportCustomDecks()
{
    InitializeDefaultDecks();

    var starterDecks = new[]
    {
        StarterDeckFactory.CreateQueueStarter(),
        StarterDeckFactory.CreateStackStarter(),
        StarterDeckFactory.CreatePriorityStarter(),
        StarterDeckFactory.CreateRandomStarter(),
        StarterDeckFactory.CreateLinkedStarter()
    };

    var customDecks = Decks
        .Where(deck => !starterDecks.Any(starter =>
            deck.Name == starter.Name &&
            deck.Structure == starter.Structure))
        .ToList();

    return JsonSerializer.Serialize(customDecks);
}

public static void ImportCustomDecks(string? json)
{
    Decks.Clear();

    // Always restore all five starter decks first.
    InitializeDefaultDecks();

    if (string.IsNullOrWhiteSpace(json))
        return;

    try
    {
        var savedDecks =
            JsonSerializer.Deserialize<List<Deck>>(json);

        if (savedDecks == null)
            return;

        var starterDecks = Decks.ToList();

        foreach (Deck deck in savedDecks)
        {
            if (deck == null)
                continue;

            // Never import a saved copy of a starter deck.
            bool isStarter = starterDecks.Any(starter =>
                deck.Name == starter.Name &&
                deck.Structure == starter.Structure);

            if (isStarter)
                continue;

            Decks.Add(DeckCloner.Clone(deck));
        }
    }
    catch (JsonException)
    {
        // Invalid saved data leaves the starter decks available.
    }
}
}