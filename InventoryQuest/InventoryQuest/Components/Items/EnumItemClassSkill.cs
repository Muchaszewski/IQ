using System;
using InventoryQuest.Components.Statistics;

namespace InventoryQuest.Components.Items
{
    /// <summary>
    ///     TODO comment
    /// </summary>
    public enum EnumItemClassSkill
    {
        Cloth,
        Light,
        Medium,
        Heavy,

        [ItemParameter(EnumTypeStat.Strength, EnumTypeStat.Vitality)] Shield,

        [ItemParameter(EnumTypeStat.Wisdom, EnumTypeStat.Perception)] Offhand,
        [ItemParameter(EnumTypeStat.Dexterity, EnumTypeStat.Strength)] Unarmed,
        [ItemParameter(EnumTypeStat.Dexterity, EnumTypeStat.Dexterity)] Dagger,
        [ItemParameter(EnumTypeStat.Strength, EnumTypeStat.Dexterity)] Sword,
        [ItemParameter(EnumTypeStat.Strength, EnumTypeStat.Strength)] Axe,
        [ItemParameter(EnumTypeStat.Strength, EnumTypeStat.Strength)] Mace,
        Flail,
        Polearm,
        [ItemParameter(EnumTypeStat.Intelligence, EnumTypeStat.Wisdom)] Staff,
        [ItemParameter(EnumTypeStat.Intelligence, EnumTypeStat.Intelligence)] Wand,
        [ItemParameter(EnumTypeStat.Dexterity, EnumTypeStat.Perception)] Bow,
        [ItemParameter(EnumTypeStat.Dexterity, EnumTypeStat.Strength)] Crossbow,
        [ItemParameter(EnumTypeStat.Perception, EnumTypeStat.Strength)] Thrown
    }

    /// <summary>
    ///     Item attributes. Used to calculate item stats based on player stats contained in attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ItemParameter : Attribute
    {
        public ItemParameter(EnumTypeStat attribute1, EnumTypeStat attribute2)
        {
            Attribute1 = attribute1;
            Attribute2 = attribute2;
        }

        public EnumTypeStat Attribute1 { get; private set; }
        public EnumTypeStat Attribute2 { get; private set; }
    }
}