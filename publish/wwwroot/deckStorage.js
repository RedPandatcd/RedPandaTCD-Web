window.deckStorage = {
    load: () => localStorage.getItem("redPandaCustomDecks"),

    save: (json) =>
        localStorage.setItem("redPandaCustomDecks", json)
};