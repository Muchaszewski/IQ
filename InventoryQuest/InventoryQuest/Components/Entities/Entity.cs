﻿using System;
using InventoryQuest.Components.Statistics;

namespace InventoryQuest.Components.Entities
{
    /// <summary>
    ///     Base entity
    /// </summary>
    [Serializable]
    public class Entity
    {
        private EnumSex _Sex = EnumSex.None;

        /// <summary>
        ///     Entity base constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="level">Level</param>
        /// <param name="stats">Stats</param>
        /// <param name="type">Type</param>
        public Entity(string name, int level, Stats stats, EnumEntityType type)
        {
            Name = name;
            Level = level;
            Stats = stats;
            Type = type;
            IsAlive = true;
        }

        /// <summary>
        ///     Displayed name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Entity sex. Default None
        /// </summary>
        public EnumSex Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }

        /// <summary>
        ///     Displayed entity type
        ///     TODO: Multiple tags
        /// </summary>
        public EnumEntityType Type { get; set; }

        /// <summary>
        ///     Enitity stats
        /// </summary>
        public Stats Stats { get; set; }

        /// <summary>
        ///     Entity level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     ID image of this entity
        /// </summary>
        public ImageIDPair ImageID { get; set; }

        /// <summary>
        ///     Time left to next atack
        /// </summary>
        public float NextTurn { get; set; }

        /// <summary>
        ///     Total time without reciving damage
        /// </summary>
        public float NoDamageTime { get; set; }

        public float Position { get; set; }

        /// <summary>
        ///     Is displayed on right side
        /// </summary>
        public bool IsRightSide { get; set; }

        /// <summary>
        ///     Returns alive status
        /// </summary>
        public bool IsAlive { get; set; }

        /// <summary>
        ///     Get Damage per second
        /// </summary>
        public virtual float DPS
        {
            get
            {
                float avgDmg = ((MinDamage + MaxDamage) / 2f);
                float avgCrit = (Stats.CriticalChance.Extend * Stats.CriticalDamage.Extend) / (2f * 100 * 100);
                var speed = AttackSpeed;
                if (avgCrit == 0)
                {
                    avgCrit = 1;
                }
                return avgDmg * avgCrit * speed;
            }
        }


        /// <summary>
        ///     General power based on all stats
        ///     TODO: Redo/ Delete
        /// </summary>
        internal int EntityPower
        {
            get
            {
                var power = 0;
                foreach (StatValueInt item in Stats.GetAllStatsInt())
                {
                    power++;
                }
                foreach (StatValueFloat item in Stats.GetAllStatsFloat())
                {
                    power++;
                }
                return power;
            }
        }

        /// <summary>
        ///     Return damage base on current stats and critical
        ///     <para> TODO Does not count in Magic damage </para>
        ///     <para> out critical damage alone </para>
        /// </summary>
        /// <returns></returns>
        public virtual float Attack(out int critical)
        {
            float damage = RandomNumberGenerator.NextRandom(MinDamage, MaxDamage);
            critical = 0;
            if (RandomNumberGenerator.NextRandom(100) <= Stats.CriticalChance.Extend)
            {
                critical = (int)damage;
                damage *= Stats.CriticalDamage.Extend / 100f;
                critical -= (int)damage;
                critical = Math.Abs(critical);
            }
            return damage;
        }

        /// <summary>
        ///     Return pierce of current attack
        ///     <para> TODO Macic damage </para>
        /// </summary>
        /// <returns></returns>
        public virtual int Pierce()
        {
            return Stats.ArmorPenetration.Extend;
        }

        /// <summary>
        ///     Return defend base on defend stats </para>
        ///     TODO Does not count in Magic damage
        /// </summary>
        /// <returns></returns>
        public virtual int Defend()
        {
            return Stats.Armor.Extend;
        }

        public virtual float Parry
        {
            get { return Stats.Deflection.Extend; }
        }
        #region Statistics
        //
        //  All statistics overrided for no weapon fight
        //
        public virtual float Accuracy
        {
            get { return Stats.Accuracy.Extend; }
        }

        public virtual float AttackSpeed
        {
            get { return Stats.AttackSpeed.Extend; }
        }

        public virtual float MinDamage
        {
            get { return Stats.MinDamage.Extend; }
        }

        public virtual float MaxDamage
        {
            get { return Stats.MaxDamage.Extend; }
        }
        #endregion
    }
}