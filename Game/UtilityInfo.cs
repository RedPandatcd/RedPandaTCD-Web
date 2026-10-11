namespace RedPandaTCD_Web.Game;

public static class UtilityInfo
{
    public static string GetDescription(
        Card card)
    {
        return card.Name switch
        {
            "Energy Potion" =>
                "Restore 3 Energy, up to your maximum Energy.",

            "Energy Drain" =>
                "Drain 3 Energy from the opponent and restore 3 of your Energy.",

            "Attack Amplifier" =>
                "Your Attacks gain +1 Damage.",

            "Defensive Stance" =>
                "Deflect the next incoming hit.",

            "Shield Booster" =>
                "Gain 3 Shield.",

           "Shield Breaker" =>
                "Remove 3 Shield from the opponent.",

            "Discount Coupon" =>
                "All Energy costs are reduced by 1 for the rest of the turn.",

            "Discard For Energy" =>
                "Uses this card to select a card to discard , get the cards energy cost as a refund",

            _ =>
                "No effect description available."
        };
    }
}