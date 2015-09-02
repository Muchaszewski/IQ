using System;
using System.Collections.Generic;
using System.Reflection;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Utils;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class ItemType
    {
        private List<EnumTypeStat> _BaseStatsAllowed;
        private string _Description;
        private int _DropLevel;
        private MinMaxStat _Durability = new MinMaxStat();
        private string _ExtraName;
        private int _ID;
        private List<PairTypeItem> _ImageID = new List<PairTypeItem>();
        private string _Name;
        private EnumItemRarity _Rarity = EnumItemRarity.Poor;
        private int _RequiredLevel;
        private List<MinMaxStatType> _RequiredStats = new List<MinMaxStatType>();
        private EnumItemClassSkill _Skill;
        private List<PairTypeItem> _SoundID = new List<PairTypeItem>();
        private EnumItemType _Type;

        /// <summary>
        ///     Item name
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        /// <summary>
        ///     Special item name
        /// </summary>
        public string ExtraName
        {
            get { return _ExtraName; }
            set { _ExtraName = value; }
        }

        /// <summary>
        ///     Allowed base stats on item
        /// </summary>
        public List<EnumTypeStat> BaseStatsAllowed
        {
            get { return _BaseStatsAllowed; }
            set { _BaseStatsAllowed = value; }
        }

        /// <summary>
        ///     Type of this item
        /// </summary>
        public EnumItemType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        /// <summary>
        ///     Skill that increase effectiveness of this item
        /// </summary>
        public EnumItemClassSkill Skill
        {
            get { return _Skill; }
            set { _Skill = value; }
        }

        /// <summary>
        ///     Min, Max Durability on item
        /// </summary>
        public MinMaxStat Durability
        {
            get { return _Durability; }
            set { _Durability = value; }
        }

        /// <summary>
        ///     Id of image from item type group
        /// </summary>
        public List<PairTypeItem> ImageID
        {
            get { return _ImageID; }
            set { _ImageID = value; }
        }

        /// <summary>
        ///     Sound of an Item
        /// </summary>
        public List<PairTypeItem> SoundID
        {
            get { return _SoundID; }
            set { _SoundID = value; }
        }

        /// <summary>
        ///     Flavor text </para>
        ///     /n - make a new lane </para>
        ///     Avoid adding new lane at the end
        /// </summary>
        public string FlavorText
        {
            get { return _Description; }
            set { _Description = value; }
        }

        /// <summary>
        ///     Minimal rarity posible to drop </para>
        ///     Mistic rarity only if set to mistic</para>
        ///     Poor if not setup
        /// </summary>
        public EnumItemRarity Rarity
        {
            get { return _Rarity; }
            set { _Rarity = value; }
        }

        /// <summary>
        ///     Level that this item begin with
        /// </summary>
        public int RequiredLevel
        {
            get { return _RequiredLevel; }
            set { _RequiredLevel = value; }
        }

        /// <summary>
        ///     Required level to drop this item
        /// </summary>
        public int DropLevel
        {
            get { return _DropLevel; }
            set { _DropLevel = value; }
        }

        /// <summary>
        ///     Required stats
        /// </summary>
        public List<MinMaxStatType> RequiredStats
        {
            get { return _RequiredStats; }
            set { _RequiredStats = value; }
        }

        //TODO By reflection set all stats to 0

        protected virtual void ResetAllSkippedStats()
        {
            //PropertyInfo[] props = GetType().GetProperties();
            //foreach (PropertyInfo item in props)
            //{
            //    if (item.GetValue(this, new object[] { props[0] }) == null)
            //    {
            //        if (item.PropertyType == typeof(MinMaxStat))
            //        {
            //            item.SetValue(this, new MinMaxStat(0, 0), props);
            //        }
            //        else if (item.PropertyType == typeof(MinMaxStat))
            //        {
            //            item.SetValue(this, new MinMaxStat(0, 0), props);
            //        }
            //        else if (item.PropertyType == typeof(string) && item.Name == "Name")
            //        {
            //            item.SetValue(this, "!MISSING!!NAME!", props);
            //        }
            //    }
            //}
        }
    }
}