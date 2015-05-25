namespace InventoryQuest.Components.Statistics
{
    internal interface IStats<T>
    {
        #region BaseStats

        /// <summary>
        ///     Strength
        /// </summary>
        T Strength { get; set; }

        /// <summary>
        ///     Vitaltiy
        /// </summary>
        T Vitality { get; set; }

        /// <summary>
        ///     Intelligence
        /// </summary>
        T Intelligence { get; set; }

        /// <summary>
        ///     Dexterity
        /// </summary>
        T Dexterity { get; set; }

        /// <summary>
        ///     Perception
        /// </summary>
        T Perception { get; set; }


        /// <summary>
        ///     Wisdom
        /// </summary>
        T Wisdom { get; set; }

        #endregion

        #region CharacterStats

        /// <summary>
        ///     Health points
        /// </summary>
        T HealthPoints { get; set; }

        /// <summary>
        ///     Health points
        /// </summary>
        T HealthRegen { get; set; }

        /// <summary>
        ///     Mana
        /// </summary>
        T ManaPoints { get; set; }


        /// <summary>
        ///     Mana
        /// </summary>
        T ManaRegen { get; set; }

        /// <summary>
        ///     Shield
        /// </summary>
        T ShieldPoints { get; set; }

        /// <summary>
        ///     Shield
        /// </summary>
        T ShieldRegen { get; set; }

        /// <summary>
        ///     Stamina
        /// </summary>
        T StaminaPoints { get; set; }

        /// <summary>
        ///     Stamina
        /// </summary>
        T StaminaRegen { get; set; }

        #endregion

        #region CombatStats

        /// <summary>
        ///     Accuracy
        /// </summary>
        T Accuracy { get; set; }

        /// <summary>
        ///     Weapon atack speed
        /// </summary>
        T AttackSpeed { get; set; }

        /// <summary>
        ///     Weapon armro penetration
        /// </summary>
        T ArmorPenetration { get; set; }

        /// <summary>
        ///     Weapon parry chance
        /// </summary>
        T ParryChance { get; set; }

        /// <summary>
        ///     Critical Damage
        /// </summary>
        T CriticalDamage { get; set; }

        /// <summary>
        ///     Critical Chance
        /// </summary>
        T CriticalChance { get; set; }

        /// <summary>
        ///     Weapon damage
        /// </summary>
        T MinDamage { get; set; }

        /// <summary>
        ///     Weapon damage
        /// </summary>
        T MaxDamage { get; set; }

        /// <summary>
        ///     TODO:
        /// </summary>
        T Armor { get; set; }

        /// <summary>
        ///     Evasion Chance
        /// </summary>
        T Evasion { get; set; }

        /// <summary>
        ///     Block chance value
        /// </summary>
        T BlockChance { get; set; }

        /// <summary>
        ///     Block chance value
        /// </summary>
        T BlockAmount { get; set; }

        #endregion
    }
}