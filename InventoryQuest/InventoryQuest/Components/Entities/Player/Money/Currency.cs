namespace InventoryQuest.Components.Entities.Money
{
    public enum Currency : long
    {
        /// <summary>
        ///     1 copper coin is the smallest amount of money that exist.
        /// </summary>
        [Name("copper", "c")] Copper = 1,

        /// <summary>
        ///     1 Silver coin = 100 copper coins.
        /// </summary>
        [Name("silver", "s")] Silver = 100,

        /// <summary>
        ///     1 Gold coin = 100 silver coins = 10 000 silver coins.
        /// </summary>
        [Name("gold", "g")] Gold = 10000,

        /// <summary>
        ///     1 Platinum coin = 100 gold coins = 10 000 gold coins = 1 000 000 copper coins.
        /// </summary>
        [Name("platinum", "p")] Platinum = 1000000
    }

    public static class CurrencyExtensions
    {
        public static long GetValue(this Currency currency)
        {
            return (long) currency;
        }
    }
}