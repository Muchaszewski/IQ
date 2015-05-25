using System;
using System.Collections.Generic;
using System.Reflection;

namespace InventoryQuest.Components.Entities.Generation.Types
{
    [Serializable]
    public class EntityType
    {
        private MinMaxStat _Accuracy;
        private MinMaxStat _Armor;
        private MinMaxStat _ArmorPenetration;
        private MinMaxStat _AttackSpeed;
        private MinMaxStat _BlockAmount;
        private MinMaxStat _BlockChance;
        private MinMaxStat _CriticalChance;
        private MinMaxStat _CriticalDamage;
        private MinMaxStat _Deflection;
        private MinMaxStat _Evasion;
        private MinMaxStat _HealthPoints;
        private MinMaxStat _HealthRegen;
        private int _ID;
        private List<GenerationWeightLists> _itemsLists = new List<GenerationWeightLists>();
        private MinMaxStat _ManaPoints;
        private MinMaxStat _ManaRegen;
        private MinMaxStat _MaxDamage;
        private MinMaxStat _MinDamage;
        private MinMaxStat _MovmentSpeed;
        private string _Name;
        private EnumSex _Sex = EnumSex.None;
        private MinMaxStat _ShieldPoints;
        private MinMaxStat _ShieldRegen;
        private MinMaxStat _StaminaPoints;
        private MinMaxStat _StaminaRegen;
        private EnumEntityType _Type;

        /// <summary>
        ///     Create new entity </para>
        ///     If stat is not set default is 0
        /// </summary>
        public EntityType()
        {
            //PropertyInfo[] props = GetType().GetProperties();
            //foreach (PropertyInfo item in props)
            //{
            //    if (item.GetValue(this, props) == null)
            //    {
            //        if (item.PropertyType == typeof (MinMaxStat))
            //        {
            //            item.SetValue(this, new MinMaxStat(0, 0),props);
            //        }
            //        else if (item.PropertyType == typeof (MinMaxStat))
            //        {
            //            item.SetValue(this, new MinMaxStat(0, 0), props);
            //        }
            //        else if (item.PropertyType == typeof (string))
            //        {
            //            item.SetValue(this, "", props);
            //        }
            //    }
            //}
        }

        /// <summary>
        ///     Create new entity </para>
        ///     If stat is not set default is 0
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public EntityType(string name, EnumEntityType type)
            : this()
        {
            Name = name;
            Type = type;
        }

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

        public int Level { get; set; }

        public EnumEntityType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public EnumSex Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        public MinMaxStat Accuracy
        {
            get { return _Accuracy; }
            set { _Accuracy = value; }
        }

        public MinMaxStat ArmorPenetration
        {
            get { return _ArmorPenetration; }
            set { _ArmorPenetration = value; }
        }

        public MinMaxStat AttackSpeed
        {
            get { return _AttackSpeed; }
            set { _AttackSpeed = value; }
        }

        public MinMaxStat BlockAmount
        {
            get { return _BlockAmount; }
            set { _BlockAmount = value; }
        }

        public MinMaxStat BlockChance
        {
            get { return _BlockChance; }
            set { _BlockChance = value; }
        }

        public MinMaxStat CriticalChance
        {
            get { return _CriticalChance; }
            set { _CriticalChance = value; }
        }

        public MinMaxStat CriticalDamage
        {
            get { return _CriticalDamage; }
            set { _CriticalDamage = value; }
        }

        public MinMaxStat MinDamage
        {
            get { return _MinDamage; }
            set { _MinDamage = value; }
        }

        public MinMaxStat MaxDamage
        {
            get { return _MaxDamage; }
            set { _MaxDamage = value; }
        }

        public MinMaxStat Armor
        {
            get { return _Armor; }
            set { _Armor = value; }
        }

        public MinMaxStat Evasion
        {
            get { return _Evasion; }
            set { _Evasion = value; }
        }

        public MinMaxStat HealthPoints
        {
            get { return _HealthPoints; }
            set { _HealthPoints = value; }
        }

        public MinMaxStat HealthRegen
        {
            get { return _HealthRegen; }
            set { _HealthRegen = value; }
        }

        public MinMaxStat ManaPoints
        {
            get { return _ManaPoints; }
            set { _ManaPoints = value; }
        }

        public MinMaxStat ManaRegen
        {
            get { return _ManaRegen; }
            set { _ManaRegen = value; }
        }

        public MinMaxStat Deflection
        {
            get { return _Deflection; }
            set { _Deflection = value; }
        }

        public MinMaxStat StaminaPoints
        {
            get { return _StaminaPoints; }
            set { _StaminaPoints = value; }
        }

        public MinMaxStat StaminaRegen
        {
            get { return _StaminaRegen; }
            set { _StaminaRegen = value; }
        }

        public MinMaxStat ShieldPoints
        {
            get { return _ShieldPoints; }
            set { _ShieldPoints = value; }
        }

        public MinMaxStat ShieldRegen
        {
            get { return _ShieldRegen; }
            set { _ShieldRegen = value; }
        }

        public MinMaxStat MovmentSpeed
        {
            get { return _MovmentSpeed; }
            set { _MovmentSpeed = value; }
        }

        public List<GenerationWeightLists> ItemsLists
        {
            get { return _itemsLists; }
            set { _itemsLists = value; }
        }
    }
}