using System;
using System.Collections.Generic;
using InventoryQuest.Components.Entities.Generation.Types;
using InventoryQuest.Components.Statistics;

namespace InventoryQuest.Components.Entities.Generation
{
    public static class RandomEnemyFactory
    {
        private static bool _isModified;
        private static float _regenModifier = 1;
        private static float _healthModifier = 1;
        private static float _shieldModifier = 1;
        private static float _manaModifier = 1;
        private static float _staminaModifier = 1;
        private static float _accuracyModifier = 1;
        private static float _damageModifier = 1;
        private static float _armorModifier = 1;
        private static float _defenceModifier = 1;
        private static float _movementSpeedModifier = 1;

        /// <summary>
        ///     Create random list of enemies
        /// </summary>
        /// <param name="Spotot witch from enemy list is taken to make
        /// </param>
        /// <param name="maxLevel">Maximum Level of created enemy</param>
        /// <param name="expectedTotalPower">Expected power of whole group :: DEFAULT maxLevel * 20 * enemiesNumber</param>
        /// <returns></returns>
        [Obsolete]
        public static List<Entity> CreateListOfEnemies(Spot Spot, int maxLevel, int expectedTotalPower)
        {
            var enemies = new List<Entity>();
            var length = 10;
            for (var i = 0; i < length; i++)
            {
                Entity enemy = CreateEnemy(Spot, maxLevel - i, EnumEntityRarity.Normal);
                if (enemy.EntityPower < expectedTotalPower)
                {
                    enemies.Add(enemy);
                    i = 0;
                }
                else
                {
                    break;
                }
            }
            if (enemies.Count == 0)
            {
                enemies.Add(CreateEnemy(Spot, maxLevel, EnumEntityRarity.Normal));
            }
            return enemies;
        }

        /// <summary>
        ///     Create random list of enemies
        /// </summary>
        /// <param name="Spotot witch from enemy list is taken to make
        /// </param>
        /// <param name="maxLevel">Maximum Level of created enemy</param>
        /// <param name="numberOfEnemies">Number of enemies (Number larger then 2 may cause instant death while in fight)</param>
        /// <returns></returns>
        [Obsolete]
        public static List<Entity> CreateNumberOfEnemies(Spot Spot, int maxLevel, int numberOfEnemies,
            EnumEntityRarity rarity)
        {
            var enemies = new List<Entity>();
            for (var i = 0; i < numberOfEnemies; i++)
            {
                Entity enemy = CreateEnemy(Spot, maxLevel - i, rarity);
                if (enemy != null)
                {
                    enemies.Add(enemy);
                }
            }
            return enemies;
        }

        public static Entity CreateEnemy(Spot Spot, EnumEntityRarity rarity)
        {
            return CreateEnemy(
                Spot,
                RandomNumberGenerator.NextRandom(Spot.Level - 3, Spot.Level + 1),
                rarity
                );
        }

        /// <summary>
        ///     Creating random enemy
        /// </summary>
        /// <param name="Spotot witch from enemy list is taken to make
        /// </param>
        /// <param name="level">Level of created enemy</param>
        /// <returns></returns>
        public static Entity CreateEnemy(Spot Spot, int level, EnumEntityRarity rarity)
        {
            EntityType type;
            //Choosing random enemy
            var rSpot = 0;

            EntityLists EntityList = GenerationStorage.Instance.EntityLists.Find(
                x => x.Name == Spot.EntitiesList[0].Name);

            _isModified = false;

            switch (rarity)
            {
                case EnumEntityRarity.Normal:
                    if (!_isModified)
                    {
                        _regenModifier = 0.5f;
                        _healthModifier = 0.5f;
                        _shieldModifier = 0.5f;
                        _manaModifier = 0.5f;
                        _staminaModifier = 0.5f;
                        _accuracyModifier = 0.5f;
                        _damageModifier = 0.5f;
                        _armorModifier = 0.5f;
                        _defenceModifier = 0.5f;
                        _movementSpeedModifier = 0.5f;
                        _isModified = true;
                    }
                    if (EntityList.EntityTypeNormalID.Count == 0)
                    {
                        return null;
                    }
                    rSpot = RandomNumberGenerator.NextRandom(EntityList.EntityTypeNormalID.Count);
                    type = GenerationStorage.Instance.Entities[EntityList.EntityTypeNormalID[rSpot].ID];
                    break;
                case EnumEntityRarity.Uncommon:
                    if (!_isModified)
                    {
                        _regenModifier = 1;
                        _healthModifier = 1;
                        _shieldModifier = 1;
                        _manaModifier = 1;
                        _staminaModifier = 1;
                        _accuracyModifier = 1;
                        _damageModifier = 1;
                        _armorModifier = 1;
                        _defenceModifier = 1;
                        _movementSpeedModifier = 1;
                        _isModified = true;
                    }
                    if (EntityList.EntityTypeUncommonID.Count == 0)
                    {
                        goto case EnumEntityRarity.Normal;
                    }
                    rSpot = RandomNumberGenerator.NextRandom(EntityList.EntityTypeUncommonID.Count);
                    type = GenerationStorage.Instance.Entities[EntityList.EntityTypeUncommonID[rSpot].ID];
                    break;
                case EnumEntityRarity.Rare:
                    if (!_isModified)
                    {
                        _regenModifier = 1.25f;
                        _healthModifier = 2f;
                        _shieldModifier = 2f;
                        _manaModifier = 1.25f;
                        _staminaModifier = 2f;
                        _accuracyModifier = 1.1f;
                        _damageModifier = 1.1f;
                        _armorModifier = 1.25f;
                        _defenceModifier = 1.25f;
                        _movementSpeedModifier = 0.8f;
                        _isModified = true;
                    }
                    if (EntityList.EntityTypeRareID.Count == 0)
                    {
                        goto case EnumEntityRarity.Uncommon;
                    }
                    rSpot = RandomNumberGenerator.NextRandom(EntityList.EntityTypeRareID.Count);
                    type = GenerationStorage.Instance.Entities[EntityList.EntityTypeRareID[rSpot].ID];
                    break;
                case EnumEntityRarity.Unique:
                    if (EntityList.EntityTypeUniqueID.Count == 0)
                    {
                        goto case EnumEntityRarity.Rare;
                    }
                    rSpot = RandomNumberGenerator.NextRandom(EntityList.EntityTypeRareID.Count);
                    type = GenerationStorage.Instance.Entities[EntityList.EntityTypeRareID[rSpot].ID];
                    break;
                default:
                    throw new Exception("Unknown error while creating enemy");
            }
            //Setting up name
            var name = type.Name;
            //Setting up stats
            Stats stats = GetStats(type, level);
            //Setting up type
            EnumEntityType enemyType = type.Type;

            return new Entity(name, level, stats, enemyType);
        }

        /// <summary>
        ///     TODO
        /// </summary>
        /// <param name="type"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private static Stats GetStats(EntityType type, int level)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var stats = new Stats();
            stats.Accuracy.Base = (int) (type.Accuracy.GetRandomForLevel(level)*_accuracyModifier);
            stats.ArmorPenetration.Base = (int) type.ArmorPenetration.GetRandomForLevel(level);
            stats.AttackSpeed.Base = (float) type.AttackSpeed.GetRandomForLevel(level);
            stats.BlockAmount.Base = (int) (type.BlockAmount.GetRandomForLevel(level)*_defenceModifier);
            stats.BlockChance.Base = (int) type.BlockChance.GetRandomForLevel(level);
            stats.CriticalDamage.Base = (int) type.CriticalDamage.GetRandomForLevel(level);
            stats.CriticalChance.Base = (int) type.CriticalChance.GetRandomForLevel(level);
            stats.MinDamage.Base = (int) (type.MinDamage.GetRandomForLevel(level)*_damageModifier);
            stats.MaxDamage.Base = (int) (type.MaxDamage.GetRandomForLevel(level)*_damageModifier);
            stats.Armor.Base = (int) (type.Armor.GetRandomForLevel(level)*_armorModifier);
            stats.Evasion.Base = (int) type.Evasion.GetRandomForLevel(level);
            stats.HealthPoints.Base = (float) type.HealthPoints.GetRandomForLevel(level)*_healthModifier;
            stats.HealthRegen.Base = (float) type.HealthRegen.GetRandomForLevel(level)*_regenModifier;
            stats.ManaPoints.Base = (float) type.ManaPoints.GetRandomForLevel(level)*_manaModifier;
            stats.ManaRegen.Base = (float) type.ManaRegen.GetRandomForLevel(level)*_regenModifier;
            stats.Deflection.Base = (int) (type.Deflection.GetRandomForLevel(level)*_defenceModifier);
            stats.StaminaPoints.Base = (float) type.StaminaPoints.GetRandomForLevel(level)*_staminaModifier;
            stats.StaminaRegen.Base = (float) type.StaminaRegen.GetRandomForLevel(level)*_regenModifier;
            stats.ShieldPoints.Base = (float) type.ShieldPoints.GetRandomForLevel(level)*_shieldModifier;
            stats.ShieldRegen.Base = (float) type.ShieldRegen.GetRandomForLevel(level)*_regenModifier;
            stats.MovmentSpeed.Base = (float) type.MovmentSpeed.GetRandomForLevel(level)*_movementSpeedModifier;
            return stats;
        }

        private static void SetRestrictions(ref Stats stats)
        {
            stats.HealthPoints.Minimum = 0;
            stats.ManaPoints.Minimum = 0;
            stats.ShieldPoints.Minimum = 0;
        }
    }
}