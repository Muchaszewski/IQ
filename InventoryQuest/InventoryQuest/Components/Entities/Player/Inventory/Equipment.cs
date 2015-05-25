using System;
using System.Collections.Generic;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;

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
        private const int AMULETS_COUNT = 1;

        /// <summary>
        ///     Max rings posible to wear at once
        /// </summary>
        private const int RINGS_COUNT = 6;

        /// <summary>
        ///     Player
        /// </summary>
        private readonly Player player;

        private Item[] _Amulets;
        private List<Item> _Armor;
        private Item[] _Ring;
        private Item _Shield;
        private Item _Weapon;

        /// <summary>
        ///     If equiped item changed
        ///     <para>Called in method UpdateStatisticsOnUnEquip</para>
        /// </summary>
        public EventHandler<EventArgs> EquipmentChange = delegate { };

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
        }

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
        public Item Shield
        {
            get { return _Shield; }
            set { _Shield = value; }
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

        public void Equip(Item item, EnumItemSlot slot)
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
                    Armor[(int) slot - 1] = item;
                    break;
                case EnumItemSlot.Neck:
                    throw new NotImplementedException("Amulet is obsolete");
                    break;
                case EnumItemSlot.Finger:
                    throw new NotImplementedException("Amulet is obsolete");
                    break;
                case EnumItemSlot.OffHand: //Shield
                    Shield = item;
                    break;
                case EnumItemSlot.Weapon:
                    Weapon = item;
                    break;
                case EnumItemSlot.Unknown:
                    throw new ArgumentOutOfRangeException("slot", slot, null);
                default:
                    throw new ArgumentOutOfRangeException("slot", slot, null);
            }
            SetCustomStats();
        }

        /// <summary>
        ///     Update player statistics
        /// </summary>
        public void UpdateStatisticsOnEquip(Item item)
        {
            List<StatValueInt> playerStatsInt = player.Stats.GetAllStatsInt();
            List<StatValueInt> itemStatsInt = item.Stats.GetAllStatsInt();
            List<StatValueFloat> playerStatsFloat = player.Stats.GetAllStatsFloat();
            List<StatValueFloat> itemStatsFloat = item.Stats.GetAllStatsFloat();

            for (var i = 0; i < playerStatsInt.Count; i++)
            {
                playerStatsInt[i].Extend += itemStatsInt[i].Base;
            }
            for (var i = 0; i < playerStatsFloat.Count; i++)
            {
                playerStatsFloat[i].Extend += itemStatsFloat[i].Base;
            }
        }

        public void UpdateStatisticsOnUnEquip(Item item)
        {
            var items = new List<Item>();
            items.Add(item);
            UpdateStatisticsOnUnEquip(items);
        }

        /// <summary>
        ///     Update player statistics if unequiping an item
        /// </summary>
        public void UpdateStatisticsOnUnEquip(List<Item> items)
        {
            //Get all player stats
            List<StatValueInt> playerStats = player.Stats.GetAllStatsInt();
            //Iter thru player stats
            foreach (Item item in items)
            {
                //Get item stats
                List<StatValueInt> itemStats = item.Stats.GetAllStatsInt();
                //Iter thru item stats
                for (var i = 0; i < playerStats.Count; i++)
                {
                    //Remove every stat from player
                    playerStats[i].Extend -= itemStats[i].Base;
                }
            }

            List<StatValueFloat> playerStatsFloat = player.Stats.GetAllStatsFloat();
            foreach (Item item in items)
            {
                List<StatValueFloat> itemStats = item.Stats.GetAllStatsFloat();
                for (var i = 0; i < playerStatsFloat.Count; i++)
                {
                    playerStatsFloat[i].Extend -= itemStats[i].Base;
                }
            }
            EquipmentChange(this, EventArgs.Empty);
        }

        public void SetCustomStats()
        {
            player.SetCustomAccuracy();
        }
    }
}