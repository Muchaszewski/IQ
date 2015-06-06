using System;

namespace InventoryQuest.Components.Items
{
    public class EventItemArgs : EventArgs
    {
        public EventItemArgs(int index, Item item)
        {
            Item = item;
            Index = index;
        }

        public Item Item { get; set; }

        public int Index { get; set; }
    }
}