namespace InventoryQuest.Components.Items.Generation.Types
{
    public interface IItemType
    {
        /// <summary>
        ///     Item name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Type of this item
        /// </summary>
        EnumItemDisplayedType Type { get; set; }

        /// <summary>
        ///     Skill that increase effectiveness of this item
        /// </summary>
        EnumItemType Skill { get; set; }

        /// <summary>
        ///     Slot for this item
        /// </summary>
        EnumItemSlot Slot { get; set; }

        int RequiredStats { get; set; }

        /// <summary>
        ///     Min, Max Durability on item
        /// </summary>
        MinMaxStat Durability { get; set; }
    }
}