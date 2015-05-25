using System;
using System.Collections.Generic;
using InventoryQuest.Components.Items;

namespace InventoryQuest.Components.Entities.Player.Inventory
{
    /// <summary>
    ///     List of items
    /// </summary>
    [Serializable]
    public class Inventory
    {
        private readonly List<Item> _Items = new List<Item>();

        /// <summary>
        ///     List of items in inventory
        /// </summary>
        public List<Item> Items
        {
            get { return _Items; }
            set { Items = value; }
        }

        /// <summary>
        ///     If item was added
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventItemArgs> EventItemAdded = delegate { };

        /// <summary>
        ///     If item was deleted
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventItemArgs> EventItemDeleted = delegate { };

        /// <summary>
        ///     If any of item has changed
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<EventItemArgs> EventItemChanged = delegate { };

        /// <summary>
        ///     Add new item to inventory
        /// </summary>
        public void AddItem(Item item)
        {
            if (item != null)
            {
                var index = GetNewIndex();
                Items.Add(item);
                item.Index = index;
                Items.Sort((x, y) => x.Index.CompareTo(y.Index));
                EventItemAdded.Invoke(this, new EventItemArgs(item));
            }
        }

        /// <summary>
        ///     Get first empty index
        /// </summary>
        /// <returns></returns>
        public int GetNewIndex()
        {
            var index = -1;
            foreach (Item item in Items)
            {
                if (item.Index < 0) continue;

                if (index + 1 == item.Index)
                {
                    index = item.Index;
                }
                else
                {
                    index += 1;
                    return index;
                }
            }
            return index += 1;
        }

        /// <summary>
        ///     Add new list of items to inventory
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(List<Item> item)
        {
            foreach (Item i in item)
            {
                AddItem(i);
            }
        }

        /// <summary>
        ///     Remove item form inventory
        /// </summary>
        public void RemoveItem(Item item)
        {
            var index = Items.IndexOf(item);
            Items.RemoveAt(index);
            Items.Sort((x, y) => x.Index.CompareTo(y.Index));
            EventItemDeleted.Invoke(this, new EventItemArgs(item));
        }

        /// <summary>
        ///     Remove item form inventory
        /// </summary>
        public void RemoveItem(List<Item> item)
        {
            foreach (Item i in item)
            {
                RemoveItem(i);
            }
        }
    }
}