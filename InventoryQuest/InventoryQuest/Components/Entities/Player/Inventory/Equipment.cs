using System;
using System.Collections.Generic;
using System.Linq;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Game;
using UnityEngine;

namespace InventoryQuest.Components.Entities.Player.Inventory
{
    /// <summary>
    ///     Player equipment
    /// </summary>
    [Serializable]
    public class Equipment
    {
        /// <summary>
        ///     Max amulet posible to wear at once
        /// </summary>
        protected const int AMULETS_COUNT = 1;

        /// <summary>
        ///     Max rings posible to wear at once
        /// </summary>
        protected const int RINGS_COUNT = 2;

        /// <summary>
        ///     Player
        /// </summary>
        protected readonly Player player;

        private Item[] _Amulets;
        private List<Item> _Armor;
        private Item[] _Ring;
        private Item _offHand;
        private Item _Weapon;

        /// <summary>
        ///     Every equiped armor piece
        /// </summary>
        public List<Item> Armor
        {
            get { return _Armor; }
            set { _Armor = value; }
        }

        /// <summary>
        ///     Equiped shield
        /// </summary>
        public Item OffHand
        {
            get { return _offHand; }
            set { _offHand = value; }
        }

        /// <summary>
        ///     Equiped weapon
        /// </summary>
        public Item Weapon
        {
            get { return _Weapon; }
            set { _Weapon = value; }
        }

        /// <summary>
        ///     Every amulet equiped
        /// </summary>
        public Item[] Amulets
        {
            get { return _Amulets; }
            set { _Amulets = value; }
        }

        /// <summary>
        ///     Every ring equiped
        /// </summary>
        public Item[] Ring
        {
            get { return _Ring; }
            set { _Ring = value; }
        }

        public List<Item> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        private List<Item> _items = new List<Item>();

        /// <summary>
        /// </summary>
        /// <param name="player">This player</param>
        public Equipment(Player player)
        {
            this.player = player;
            Amulets = new Item[AMULETS_COUNT];
            Ring = new Item[RINGS_COUNT];
            Armor = new List<Item>();
            for (var i = 0; i < 9; i++)
            {
                Armor.Add(null);
            }

            foreach (var item in Armor)
            {
                Items.Add(item);
            }
            foreach (var amulet in Amulets)
            {
                Items.Add(amulet);
            }
            foreach (var item in Ring)
            {
                Items.Add(item);
            }
            Items.Add(OffHand);
            Items.Add(Weapon);
        }

        public Item GetItem(EnumItemSlot slot, int extraSlot = 0)
        {
            switch (slot)
            {
                case EnumItemSlot.Head:
                case EnumItemSlot.Chest:
                case EnumItemSlot.Waist:
                case EnumItemSlot.Legs:
                case EnumItemSlot.Feet:
                case EnumItemSlot.Shoulders:
                case EnumItemSlot.Hands:
                case EnumItemSlot.Back:
                    return Armor[(int)slot - 1];
                case EnumItemSlot.Neck:
                    return Amulets[(int)extraSlot];
                case EnumItemSlot.Finger:
                    return Ring[(int)extraSlot];
                case EnumItemSlot.OffHand:
                    return OffHand;
                case EnumItemSlot.Weapon:
                    return Weapon;
                case EnumItemSlot.Unknown:
                    throw new ArgumentOutOfRangeException("slot", slot, null);
                default:
                    throw new ArgumentOutOfRangeException("slot", slot, null);
            }
        }

        void ZeroOutStatistics()
        {
            //Get all player stats
            List<StatValueInt> playerStats = player.Stats.GetAllStatsInt();
            for (var i = 0; i < playerStats.Count; i++)
            {
                playerStats[i].Extend = playerStats[i].Base;
            }

            List<StatValueFloat> playerStatsFloat = player.Stats.GetAllStatsFloat();
            for (var i = 0; i < playerStatsFloat.Count; i++)
            {
                playerStatsFloat[i].Extend = playerStatsFloat[i].Base;
            }
        }


        /// <summary>
        ///     Update player statistics if equiping an item
        /// </summary>
        public void UpdateStatistics()
        {
            ZeroOutStatistics();

            //Get all player stats
            List<StatValueInt> playerStats = player.Stats.GetAllStatsInt();
            List<StatValueFloat> playerStatsFloat = player.Stats.GetAllStatsFloat();
            //Iter thru player stats
            foreach (Item item in Items)
            {
                if (item == null) continue;
                //Get item stats
                List<StatValueInt> itemStats = item.Stats.GetAllStatsInt();
                //Iter thru item stats
                for (var i = 0; i < playerStats.Count; i++)
                {
                    playerStats[i].Extend += itemStats[i].Base;
                }

                List<StatValueFloat> itemStatsFloat = item.Stats.GetAllStatsFloat();
                for (var i = 0; i < playerStatsFloat.Count; i++)
                {
                    playerStatsFloat[i].Extend += itemStatsFloat[i].Base;
                }
            }
        }
    }
}