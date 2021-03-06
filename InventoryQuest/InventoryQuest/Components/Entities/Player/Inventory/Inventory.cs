﻿using System;
using System.Collections.Generic;
using InventoryQuest.Components.Items;
using UnityEngine;

namespace InventoryQuest.Components.Entities.Player.Inventory
{
    /// <summary>
    ///     List of items
    /// </summary>
    [Serializable]
    public class Inventory
    {
        private SortedList<int, Item> _Items = new SortedList<int, Item>();

        /// <summary>
        ///     List of items in inventory
        /// </summary>
        public SortedList<int, Item> Items
        {
            get { return _Items; }
            set { _Items = value; }
        }

        /// <summary>
        ///     If item was added
        /// </summary>
        [field: NonSerialized]
        public static event EventHandler<EventItemArgs> EventItemAdded = delegate { };

        /// <summary>
        ///     If item was deleted
        /// </summary>
        [field: NonSerialized]
        public static event EventHandler<EventItemArgs> EventItemDeleted = delegate { };

        /// <summary>
        ///     If any of item has changed
        /// </summary>
        [field: NonSerialized]
        public static event EventHandler<EventArgs> EventItemSwaped = delegate { };

        [field: NonSerialized]
        public static event EventHandler<EventArgs> EventItemMoved = delegate { };

        /// <summary>
        ///     Add new item to inventory
        /// <para>With resolved loaction</para>
        /// </summary>
        public void AddItem(Item item)
        {
            if (item != null)
            {
                var index = GetNewIndex();
                Items.Add(index, item);
                EventItemAdded.Invoke(this, new EventItemArgs(index, item));
            }
        }

        /// <summary>
        ///     Add new item to inventory
        /// <para>With given location</para>
        /// </summary>
        public void AddItem(int index, Item item)
        {
            if (item != null)
            {
                Items.Add(index, item);
                EventItemAdded.Invoke(this, new EventItemArgs(index, item));
            }
        }

        /// <summary>
        ///     Get first empty index
        /// </summary>
        /// <returns></returns>
        public int GetNewIndex()
        {
            var index = -1;
            foreach (KeyValuePair<int, Item> keyValuePair in Items)
            {
                //Dont check negative indexes
                if (keyValuePair.Key < 0) continue;

                //If selected key if equal to current index
                if (index + 1 == keyValuePair.Key)
                {
                    index = keyValuePair.Key;
                }
                else
                {
                    //Return on first occurance of free spot
                    index += 1;
                    EventItemMoved.Invoke(this, EventArgs.Empty);
                    return index;
                }
            }
            //Return if foreach is empty
            EventItemMoved.Invoke(this, EventArgs.Empty);
            return index += 1;
        }

        /// <summary>
        /// Swap 2 items with places INDEX NOT KEY
        /// </summary>
        /// <param name="indexFrom"></param>
        /// <param name="indexWhere"></param>
        public void SawpItems(int indexFrom, int indexWhere)
        {
            var itemToSawp = Items[indexFrom];
            Items[indexFrom] = Items[indexWhere];
            Items[indexWhere] = itemToSawp;
            EventItemSwaped.Invoke(this, EventArgs.Empty);
        }

        public void MoveItem(int keyFrom, int keyWhere)
        {
            Item itemToSawp;
            Items.TryGetValue(keyFrom, out itemToSawp);
            Items.Remove(keyFrom);
            Items.Add(keyWhere, itemToSawp);
            EventItemMoved.Invoke(this, EventArgs.Empty);
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
        public void RemoveItem(int index)
        {
            EventItemDeleted.Invoke(this, new EventItemArgs(index, Items[index]));
            Items.RemoveAt(index);
        }

        /// <summary>
        ///     Remove item form inventory
        /// </summary>
        public void RemoveItem(Item item)
        {
            var index = Items.IndexOfValue(item);
            EventItemDeleted.Invoke(this, new EventItemArgs(index, item));
            Items.RemoveAt(index);
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