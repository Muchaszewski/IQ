using System;

namespace InventoryQuest.Components.Items
{
    /// <summary>
    ///     Rarity of item
    /// </summary>
    public enum EnumItemRarity
    {
        [RarityChance(50)] Poor,
        [RarityChance(200)] Normal,
        [RarityChance(50)] Uncommon,
        [RarityChance(10)] Rare,
        [RarityChance(1)] Mythical,
        [RarityChance(10)] Lore
    }

    [AttributeUsage(AttributeTargets.All)]
    public class RarityChance : Attribute
    {
        public RarityChance(int chance)
        {
            Chance = chance;
        }

        public int Chance { get; set; }
    }
}