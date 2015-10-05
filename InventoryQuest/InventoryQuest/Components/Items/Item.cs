using System;
using System.Collections.Generic;
using InventoryQuest.Components.Statistics;

namespace InventoryQuest.Components.Items
{
    /// <summary>
    ///     Base item
    /// </summary>
    [Serializable]
    public class Item
    {
        private StatValueFloat _Durability = new StatValueFloat(EnumTypeStat.Durability);
        private string _ExtraName = String.Empty;
        private string _FlavorText;
        private ImageIDPair _ImageID;
        private ImageIDPair _SoundID;
        private int _ItemLevel;
        private string _Name;
        private StatValueInt _Price = new StatValueInt(EnumTypeStat.Sell);
        private EnumItemModificator _Quality = EnumItemModificator.Good;
        private int _Range;
        private EnumItemRarity _Rarity = EnumItemRarity.Normal;
        private EnumItemHands _RequiredHands;
        private int _RequiredLevel;
        private List<StatValueInt> _RequiredStats = new List<StatValueInt>();
        private EnumItemClassSkill _Skill;
        private Stats _Stats = new Stats();
        private EnumItemType _Type = EnumItemType.Unarmed;
        private EnumItemSlot _ValidSlot = EnumItemSlot.Unknown;

        /// <summary>
        ///     Empty constructor for basic item
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        ///     Constructor for basic item
        ///     TODO; Unit tests for this class
        /// </summary>
        public Item(string name, EnumItemSlot slot, Stats stats)
        {
            Name = name;
            _ValidSlot = slot;
            _Stats = stats;
        }

        //Displayed name
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        ///     Name only if Mithical/Rare
        /// </summary>
        public string ExtraName
        {
            get { return _ExtraName; }
            set { _ExtraName = value; }
        }

        /// <summary>
        ///     Optional flavor text
        /// </summary>
        public string FlavorText
        {
            get { return _FlavorText; }
            set { _FlavorText = value; }
        }

        /// <summary>
        ///     Item's stats
        /// </summary>
        public Stats Stats
        {
            get { return _Stats; }
            set { _Stats = value; }
        }

        /// <summary>
        ///     Durability
        /// </summary>
        public StatValueFloat Durability
        {
            get { return _Durability; }
            set { _Durability = value; }
        }

        /// <summary>
        ///     SellValue
        /// </summary>
        public StatValueInt Price
        {
            get { return _Price; }
            set { _Price = value; }
        }

        /// <summary>
        ///     Slot this item can fit in
        /// </summary>
        public EnumItemSlot ValidSlot
        {
            get { return _ValidSlot; }
            set { _ValidSlot = value; }
        }

        /// <summary>
        ///     Item type
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
        ///     Is weapon one handed
        /// </summary>
        public EnumItemHands RequiredHands
        {
            get { return _RequiredHands; }
            set { _RequiredHands = value; }
        }

        /// <summary>
        ///     Item range
        /// </summary>
        public int Range
        {
            get { return _Range; }
            set { _Range = value; }
        }

        /// <summary>
        ///     Item level
        /// </summary>
        public int ItemLevel
        {
            get { return _ItemLevel; }
            set { _ItemLevel = value; }
        }

        /// <summary>
        ///     Rarity
        /// </summary>
        public EnumItemRarity Rarity
        {
            get { return _Rarity; }
            set { _Rarity = value; }
        }

        /// <summary>
        ///     Item quality
        /// </summary>
        public EnumItemModificator Quality
        {
            get { return _Quality; }
            set { _Quality = value; }
        }

        /// <summary>
        ///     ID image of this item
        /// </summary>
        public ImageIDPair ImageID
        {
            get { return _ImageID; }
            set { _ImageID = value; }
        }

        /// <summary>
        ///     ID image of this item
        /// </summary>
        public ImageIDPair SoundID
        {
            get { return _SoundID; }
            set { _SoundID = value; }
        }

        /// <summary>
        ///     Required level to wear this item
        /// </summary>
        public int RequiredLevel
        {
            get { return _RequiredLevel; }
            set { _RequiredLevel = value; }
        }

        /// <summary>
        ///     Stats required to wear this item
        /// </summary>
        public List<StatValueInt> RequiredStats
        {
            get { return _RequiredStats; }
            set { _RequiredStats = value; }
        }

        /// <summary>
        ///     Get Damage per second
        /// </summary>
        public float DPS
        {
            get
            {
                float avgDmg = ((Stats.MinDamage.Current + Stats.MaxDamage.Current)/2);
                float avgCrit = (Stats.CriticalChance.Current*Stats.CriticalDamage.Current)/(2*100*100);
                var speed = Stats.AttackSpeed.Current;
                if (avgCrit == 0)
                {
                    avgCrit = 1;
                }
                return avgDmg*avgCrit*speed;
            }
        }

        public override string ToString()
        {
            return Name + " {" + Stats + "}";
        }
    }
}