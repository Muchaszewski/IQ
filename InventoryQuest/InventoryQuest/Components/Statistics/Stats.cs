using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace InventoryQuest.Components.Statistics
{
    /// <summary>
    ///     Base stats
    /// </summary>
    [Serializable]
    public class Stats
    {
        private readonly List<StatValueFloat> allBaseStatsFloat = new List<StatValueFloat>();
        private readonly List<StatValueInt> allBaseStatsInt = new List<StatValueInt>();

        /// <summary>
        ///     Basic constructor
        /// </summary>
        public Stats()
        {
            FindAllStats();
        }

        private void FindAllStats()
        {
            FindAllStatsFloat();
            FindAllStatsInt();
        }

        private void FindAllStatsInt()
        {
            Type t = GetType();
            PropertyInfo[] props =
                t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            for (var i = 0; i < props.Length; i++)
            {
                PropertyInfo pi = props[i];
                object a = pi.GetValue(this, null);
                if (pi.PropertyType == typeof(StatValueInt))
                {
                    allBaseStatsInt.Add((StatValueInt) a);
                }
            }
        }

        private void FindAllStatsFloat()
        {
            Type t = GetType();
            PropertyInfo[] props =
                t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            for (var i = 0; i < props.Length; i++)
            {
                PropertyInfo pi = props[i];
                object a = pi.GetValue(this, null);
                if (pi.PropertyType == typeof(StatValueFloat))
                {
                    allBaseStatsFloat.Add((StatValueFloat) a);
                }
            }
        }

        /// <summary>
        ///     Reset all stats
        /// </summary>
        public void ResetAll()
        {
            for (var i = 0; i < allBaseStatsInt.Count; i++)
            {
                allBaseStatsInt[i].Reset();
            }
            for (var i = 0; i < allBaseStatsFloat.Count; i++)
            {
                allBaseStatsFloat[i].Reset();
            }
        }

        /// <summary>
        ///     Turn off all stats
        /// </summary>
        public void TurnOffAll()
        {
            for (var i = 0; i < allBaseStatsInt.Count; i++)
            {
                allBaseStatsInt[i].TurnOff();
            }
            for (var i = 0; i < allBaseStatsFloat.Count; i++)
            {
                allBaseStatsFloat[i].TurnOff();
            }
        }

        /// <summary>
        ///     Return stats found by enum
        /// </summary>
        /// <param name="type">Stat type</param>
        /// <returns></returns>
        public StatValueInt GetStatIntByEnum(EnumTypeStat type)
        {
            foreach (StatValueInt item in GetAllStatsInt())
            {
                if (item.Type == type)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        ///     Retrun all stats
        /// </summary>
        /// <returns></returns>
        public List<StatValueInt> GetAllStatsInt()
        {
            return allBaseStatsInt;
        }

        /// <summary>
        ///     Return stats found by enum
        /// </summary>
        /// <param name="type">Stat type</param>
        /// <returns></returns>
        public StatValueFloat GetStatFloatByEnum(EnumTypeStat type)
        {
            foreach (StatValueFloat item in GetAllStatsFloat())
            {
                if (item.Type == type)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        ///     Retrun all stats
        /// </summary>
        /// <returns></returns>
        public List<StatValueFloat> GetAllStatsFloat()
        {
            return allBaseStatsFloat;
        }

        #region BaseStats

        private StatValueInt _Strength = new StatValueInt(EnumTypeStat.Strength);

        /// <summary>
        ///     Strength
        /// </summary>
        public StatValueInt Strength
        {
            get { return _Strength; }
            set { _Strength = value; }
        }

        private StatValueInt _Vitality = new StatValueInt(EnumTypeStat.Vitality);

        /// <summary>
        ///     Vitaltiy
        /// </summary>
        public StatValueInt Vitality
        {
            get { return _Vitality; }
            set { _Vitality = value; }
        }

        private StatValueInt _Intelligence = new StatValueInt(EnumTypeStat.Intelligence);

        /// <summary>
        ///     Intelligence
        /// </summary>
        public StatValueInt Intelligence
        {
            get { return _Intelligence; }
            set { _Intelligence = value; }
        }

        private StatValueInt _Dexterity = new StatValueInt(EnumTypeStat.Dexterity);

        /// <summary>
        ///     Dexterity
        /// </summary>
        public StatValueInt Dexterity
        {
            get { return _Dexterity; }
            set { _Dexterity = value; }
        }

        private StatValueInt _Perception = new StatValueInt(EnumTypeStat.Perception);

        /// <summary>
        ///     Perception
        /// </summary>
        public StatValueInt Perception
        {
            get { return _Perception; }
            set { _Perception = value; }
        }


        private StatValueInt _Wisdom = new StatValueInt(EnumTypeStat.Wisdom);

        /// <summary>
        ///     Wisdom
        /// </summary>
        public StatValueInt Wisdom
        {
            get { return _Wisdom; }
            set { _Wisdom = value; }
        }

        #endregion

        #region CharacterStats

        private StatValueFloat _HealthPoints = new StatValueFloat(EnumTypeStat.HealthPoints);

        /// <summary>
        ///     Health points
        /// </summary>
        public StatValueFloat HealthPoints
        {
            get { return _HealthPoints; }
            set { _HealthPoints = value; }
        }

        private StatValueFloat _HealthRegen = new StatValueFloat(EnumTypeStat.HealthRegen);

        /// <summary>
        ///     Health points
        /// </summary>
        public StatValueFloat HealthRegen
        {
            get { return _HealthRegen; }
            set { _HealthRegen = value; }
        }

        private StatValueFloat _ManaPoints = new StatValueFloat(EnumTypeStat.ManaPoints);

        /// <summary>
        ///     Mana
        /// </summary>
        public StatValueFloat ManaPoints
        {
            get { return _ManaPoints; }
            set { _ManaPoints = value; }
        }


        private StatValueFloat _ManaRegen = new StatValueFloat(EnumTypeStat.ManaRegen);

        /// <summary>
        ///     Mana
        /// </summary>
        public StatValueFloat ManaRegen
        {
            get { return _ManaRegen; }
            set { _ManaRegen = value; }
        }

        private StatValueFloat _ShieldPoints = new StatValueFloat(EnumTypeStat.ShieldPoints);

        /// <summary>
        ///     Shield
        /// </summary>
        public StatValueFloat ShieldPoints
        {
            get { return _ShieldPoints; }
            set { _ShieldPoints = value; }
        }

        private StatValueFloat _ShieldRegen = new StatValueFloat(EnumTypeStat.ShieldRegen);

        /// <summary>
        ///     Shield
        /// </summary>
        public StatValueFloat ShieldRegen
        {
            get { return _ShieldRegen; }
            set { _ShieldRegen = value; }
        }

        private StatValueFloat _StaminaPoints = new StatValueFloat(EnumTypeStat.StaminaPoints);

        /// <summary>
        ///     Stamina
        /// </summary>
        public StatValueFloat StaminaPoints
        {
            get { return _StaminaPoints; }
            set { _StaminaPoints = value; }
        }

        private StatValueFloat _StaminaRegen = new StatValueFloat(EnumTypeStat.StaminaRegen);

        /// <summary>
        ///     Stamina
        /// </summary>
        public StatValueFloat StaminaRegen
        {
            get { return _StaminaRegen; }
            set { _StaminaRegen = value; }
        }

        #endregion

        #region CombatStats

        private StatValueInt _Accuracy = new StatValueInt(EnumTypeStat.Accuracy);

        /// <summary>
        ///     Accuracy
        /// </summary>
        public StatValueInt Accuracy
        {
            get { return _Accuracy; }
            set { _Accuracy = value; }
        }

        private StatValueFloat _AttackSpeed = new StatValueFloat(EnumTypeStat.AttackSpeed);

        /// <summary>
        ///     Weapon atack speed
        /// </summary>
        public StatValueFloat AttackSpeed
        {
            get { return _AttackSpeed; }
            set { _AttackSpeed = value; }
        }

        private StatValueInt _ArmorPenetration = new StatValueInt(EnumTypeStat.ArmorPenetration);

        /// <summary>
        ///     Weapon armro penetration
        /// </summary>
        public StatValueInt ArmorPenetration
        {
            get { return _ArmorPenetration; }
            set { _ArmorPenetration = value; }
        }

        private StatValueInt _Deflection = new StatValueInt(EnumTypeStat.Deflection);

        /// <summary>
        ///     Weapon parry chance
        /// </summary>
        public StatValueInt Deflection
        {
            get { return _Deflection; }
            set { _Deflection = value; }
        }

        private StatValueInt _CriticalDamage = new StatValueInt(EnumTypeStat.CriticalDamage);

        /// <summary>
        ///     Critical Damage
        /// </summary>
        public StatValueInt CriticalDamage
        {
            get { return _CriticalDamage; }
            set { _CriticalDamage = value; }
        }

        private StatValueInt _CriticalChance = new StatValueInt(EnumTypeStat.CriticalChance);

        /// <summary>
        ///     Critical Chance
        /// </summary>
        public StatValueInt CriticalChance
        {
            get { return _CriticalChance; }
            set { _CriticalChance = value; }
        }

        private StatValueInt _MinDamage = new StatValueInt(EnumTypeStat.MinDamage);

        /// <summary>
        ///     Weapon damage
        /// </summary>
        public StatValueInt MinDamage
        {
            get { return _MinDamage; }
            set { _MinDamage = value; }
        }

        private StatValueInt _MaxDamage = new StatValueInt(EnumTypeStat.MaxDamage);

        /// <summary>
        ///     Weapon damage
        /// </summary>
        public StatValueInt MaxDamage
        {
            get { return _MaxDamage; }
            set { _MaxDamage = value; }
        }

        private StatValueFloat _Range = new StatValueFloat(EnumTypeStat.Range);

        /// <summary>
        ///     Weapon damage
        /// </summary>
        public StatValueFloat Range
        {
            get { return _Range; }
            set { _Range = value; }
        }

        #region Magic Damage
        private StatValueInt _FireDamage = new StatValueInt(EnumTypeStat.FireDamage);
        /// <summary>
        /// Fire damage
        /// </summary>

        public StatValueInt FireDamage
        {
            get { return _FireDamage; }
            set { _FireDamage = value; }
        }
        private StatValueInt _ColdDamage = new StatValueInt(EnumTypeStat.ColdDamage);
        /// <summary>
        /// Cold damage
        /// </summary>

        public StatValueInt ColdDamage
        {
            get { return _ColdDamage; }
            set { _ColdDamage = value; }
        }
        private StatValueInt _LightningDamage = new StatValueInt(EnumTypeStat.LightningDamage);
        /// <summary>
        /// Lightning damage
        /// </summary>

        public StatValueInt LightningDamage
        {
            get { return _LightningDamage; }
            set { _LightningDamage = value; }
        }
        private StatValueInt _PoisonDamage = new StatValueInt(EnumTypeStat.PoisonDamage);
        /// <summary>
        /// Poison damage
        /// </summary>

        public StatValueInt PoisonDamage
        {
            get { return _PoisonDamage; }
            set { _PoisonDamage = value; }
        }
        private StatValueInt _ArcaneDamage = new StatValueInt(EnumTypeStat.ArcaneDamage);
        /// <summary>
        ///  Arcane damage
        /// </summary>

        public StatValueInt ArcaneDamage
        {
            get { return _ArcaneDamage; }
            set { _ArcaneDamage = value; }
        }
        private StatValueInt _LightDamage = new StatValueInt(EnumTypeStat.LightDamage);
        /// <summary>
        /// Light damage
        /// </summary>

        public StatValueInt LightDamage
        {
            get { return _LightDamage; }
            set { _LightDamage = value; }
        }
        private StatValueInt _DarkDamage = new StatValueInt(EnumTypeStat.DarkDamage);
        /// <summary>
        /// Dark damage
        /// </summary>

        public StatValueInt DarkDamage
        {
            get { return _DarkDamage; }
            set { _DarkDamage = value; }
        }
        #endregion


        private StatValueInt _Armor = new StatValueInt(EnumTypeStat.Armor);

        /// <summary>
        ///     Armor defence stat
        /// </summary>
        public StatValueInt Armor
        {
            get { return _Armor; }
            set { _Armor = value; }
        }

        #region resists
        private StatValueInt _FireResist = new StatValueInt(EnumTypeStat.FireResist);
        /// <summary>
        /// Armor defence stat
        /// </summary>

        public StatValueInt FireResist
        {
            get { return _FireResist; }
            set { _FireResist = value; }
        }
        private StatValueInt _ColdResist = new StatValueInt(EnumTypeStat.ColdResist);
        /// <summary>
        /// Armor defence stat
        /// </summary>

        public StatValueInt ColdResist
        {
            get { return _ColdResist; }
            set { _ColdResist = value; }
        }
        private StatValueInt _LightningResist = new StatValueInt(EnumTypeStat.LightningResist);
        /// <summary>
        /// Armor defence stat
        /// </summary>

        public StatValueInt LightningResist
        {
            get { return _LightningResist; }
            set { _LightningResist = value; }
        }
        private StatValueInt _PoisonResist = new StatValueInt(EnumTypeStat.PoisonResist);
        /// <summary>
        /// Armor defence stat
        /// </summary>

        public StatValueInt PoisonResist
        {
            get { return _PoisonResist; }
            set { _PoisonResist = value; }
        }
        private StatValueInt _ArcaneResist = new StatValueInt(EnumTypeStat.ArcaneResist);
        /// <summary>
        /// Armor defence stat
        /// </summary>

        public StatValueInt ArcaneResist
        {
            get { return _ArcaneResist; }
            set { _ArcaneResist = value; }
        }
        private StatValueInt _LightResist = new StatValueInt(EnumTypeStat.LightResist);
        /// <summary>
        /// Armor defence stat
        /// </summary>

        public StatValueInt LightResist
        {
            get { return _LightResist; }
            set { _LightResist = value; }
        }

        private StatValueInt _DarkResist = new StatValueInt(EnumTypeStat.DarkResist);
        /// <summary>
        /// Armor defence stat
        /// </summary>

        public StatValueInt DarkResist
        {
            get { return _DarkResist; }
            set { _DarkResist = value; }
        }
        #endregion

        private StatValueInt _Evasion = new StatValueInt(EnumTypeStat.Evasion);

        /// <summary>
        ///     Evasion Chance
        /// </summary>
        public StatValueInt Evasion
        {
            get { return _Evasion; }
            set { _Evasion = value; }
        }

        private StatValueInt _BlockChance = new StatValueInt(EnumTypeStat.BlockChance);

        /// <summary>
        ///     Block chance value
        /// </summary>
        public StatValueInt BlockChance
        {
            get { return _BlockChance; }
            set { _BlockChance = value; }
        }

        private StatValueInt _BlockAmount = new StatValueInt(EnumTypeStat.BlockAmount);

        /// <summary>
        ///     Block chance value
        /// </summary>
        public StatValueInt BlockAmount
        {
            get { return _BlockAmount; }
            set { _BlockAmount = value; }
        }

        #endregion

        #region noRegionStats

        private StatValueFloat _MovmentSpeed = new StatValueFloat(EnumTypeStat.MovementSpeed);

        /// <summary>
        ///     Entity movment speed
        /// </summary>
        public StatValueFloat MovmentSpeed
        {
            get { return _MovmentSpeed; }
            set { _MovmentSpeed = value; }
        }

        #endregion
    }
}