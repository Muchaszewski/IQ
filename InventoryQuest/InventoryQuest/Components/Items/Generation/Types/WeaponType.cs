using System;

namespace InventoryQuest.Components.Items.Generation.Types
{
    [Serializable]
    public class WeaponType : ItemType
    {
        private MinMaxStat _Accuracy = new MinMaxStat();
        private MinMaxStat _ArmorPenetration = new MinMaxStat();
        private MinMaxStat _AttackSpeed = new MinMaxStat();
        private MinMaxStat _MaxDamage = new MinMaxStat();
        private MinMaxStat _MinDamage = new MinMaxStat();
        private MinMaxStat _ParryChance = new MinMaxStat();
        private int _Range;
        private EnumItemHands _RequiredHands;

        public WeaponType()
        {
            ResetAllSkippedStats();
        }

        /// <summary>
        ///     Chance to hit
        /// </summary>
        public MinMaxStat Accuracy
        {
            get { return _Accuracy; }
            set { _Accuracy = value; }
        }

        /// <summary>
        ///     Min, Max Atack speed on weapon
        /// </summary>
        public MinMaxStat AttackSpeed
        {
            get { return _AttackSpeed; }
            set { _AttackSpeed = value; }
        }

        /// <summary>
        ///     Min, Max Damage on weapon
        /// </summary>
        public MinMaxStat MinDamage
        {
            get { return _MinDamage; }
            set { _MinDamage = value; }
        }

        /// <summary>
        ///     Min, Max Damage on weapon
        /// </summary>
        public MinMaxStat MaxDamage
        {
            get { return _MaxDamage; }
            set { _MaxDamage = value; }
        }

        /// <summary>
        ///     Min, Max Armor penetration on weapon (Value should be positive)
        /// </summary>
        public MinMaxStat ArmorPenetration
        {
            get { return _ArmorPenetration; }
            set { _ArmorPenetration = value; }
        }

        /// <summary>
        ///     Min, Max Parry Chance on weapon
        /// </summary>
        public MinMaxStat Deflection
        {
            get { return _ParryChance; }
            set { _ParryChance = value; }
        }

        /// <summary>
        ///     Required hands
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
    }
}