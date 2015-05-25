using System.Collections.Generic;
using InventoryQuest.Components.Items.Generation.Types;

namespace InventoryQuest.Components.Items
{
    internal class ItemComparer : IComparer<ItemType>
    {
        public int Compare(ItemType x, ItemType y)
        {
            var result = y.Rarity.CompareTo(x.Rarity);
            if (result == 0) x.DropLevel.CompareTo(y.DropLevel);

            return result;
        }
    }
}