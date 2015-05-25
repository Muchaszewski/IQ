using System;

namespace InventoryQuest.Components.Items
{
    public class EventItemArgs : EventArgs
    {
        public EventItemArgs(Item item)
        {
            Item = item;
        }

        public Item Item { get; set; }
    }
}