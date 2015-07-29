using InventoryQuest.Utils;

namespace InventoryQuest.Components.Statistics
{
    /// <summary>
    ///     Available statistics
    /// </summary>
    public enum EnumTypeStat
    {
        [Name("UNKNOWN", "UNKNOWN")] Unknown = -1,

        //Base stats
        [StatScale(1)] [StatType(EnumStatItemPartType.BaseType)] [Name("Strength", "STR")] Strength,
        [StatScale(1)] [StatType(EnumStatItemPartType.BaseType)] [Name("Vitality", "VIT")] Vitality,
        [StatScale(1)] [StatType(EnumStatItemPartType.BaseType)] [Name("Intelligence", "INT")] Intelligence,
        [StatScale(1)] [StatType(EnumStatItemPartType.BaseType)] [Name("Agility", "AGI")] Dexterity,
        [StatScale(1)] [StatType(EnumStatItemPartType.BaseType)] [Name("Wisdom", "WIS")] Wisdom,
        [StatScale(1)] [StatType(EnumStatItemPartType.BaseType)] [Name("Perception", "PER")] Perception,

        //Character stats
        [StatScale(50)] [StatType(EnumStatItemPartType.CharacterType)] [DisplayFormat("0")] [Name("Health", "HTH")] HealthPoints,
        [StatScale(0.5f)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Health Regeneration", "REG")] HealthRegen,
        [StatScale(30)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Mana", "MANA")] [DisplayFormat("0")] ManaPoints,
        [StatScale(0.3f)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Mana Regeneration", "REG")] ManaRegen,
        [StatScale(10)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Shield", "SHD")] [DisplayFormat("0")] ShieldPoints,
        [StatScale(0.1f)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Shield Regeneration", "REG")] ShieldRegen,
        [StatScale(25)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Stamina", "STA")] [DisplayFormat("0")] StaminaPoints,
        [StatScale(0.5f)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Stamina Regeneration", "REG")] StaminaRegen,

        [StatScale(2)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Dodge", "EVA")] Evasion,

        //Combat stats
        [StatScale(1)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Accuracy", "ACC.")] Accuracy,
        [StatScale(0.05f)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Attack Speed", "SPEED")] AttackSpeed,
        [StatScale(1)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Pierce", "PIERCE")] ArmorPenetration,
        [StatScale(5)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Critical Damage", "TODO SHORT CRITICAL DAMAGE")] CriticalDamage,
        [StatScale(1)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Critical Chance", "TODO SHORT CRITICAL CHANCE")] CriticalChance,
        [StatScale(4)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Parry", "TODO SHORT DEFLECTION")] Deflection,
        [StatScale(1)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Range", "RNG")] Range,

        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Damage", "PHISICAL")] MinDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Damage", "PHISICAL")] MaxDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Fire Damage", "FIRE")] FireDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Cold Damage", "COLD")] ColdDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Lightning Damage", "LIGHTNING")] LightningDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Poison Damage", "POISON")] PoisonDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Arcane Damage", "ARCANE")] ArcaneDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Light Damage", "LIGHT")] LightDamage,
        [StatScale(2)] [StatType(EnumStatItemPartType.WeaponType)] [Name("Dark Damage", "DARK")] DarkDamage,


        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Armor", "ARM")] Armor,
        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Fire Resist", "FIRE")] FireResist,
        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Cold Resist", "COLD")] ColdResist,
        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Lightning Resist", "LIGHTNING")] LightningResist,
        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Poison Resist", "POISON")] PoisonResist,
        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Arcane Resist", "ARCANE")] ArcaneResist,
        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Light Resist", "LIGHT")] LightResist,
        [StatScale(2)] [StatType(EnumStatItemPartType.ArmorType)] [Name("Dark Resist", "DARK")] DarkResist,

        [StatScale(1)] [StatType(EnumStatItemPartType.ShieldType)] [Name("Block", "TODO SHORT BLOCK CHANCE")] BlockChance,
        [StatScale(5)] [StatType(EnumStatItemPartType.ShieldType)] [Name("Block Value", "TODO SHORT BLOCK AMOUNT")] BlockAmount,

        [Name("Damage per second", "DPS")] DPS,

        [StatScale(0.2f)] [StatType(EnumStatItemPartType.CharacterType)] [Name("Movement Speed", "MOV")] MovementSpeed,

        //TODO Dump??
        //Item stats
        Durability,
        Sell
    }
}