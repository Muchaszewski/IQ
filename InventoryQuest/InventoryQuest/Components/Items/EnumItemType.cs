using System;
using System.Collections.Generic;
using System.Reflection;

namespace InventoryQuest.Components.Items
{
    public enum EnumItemType
    {
        [TypeToSlot(EnumItemSlot.Head, EnumItemGroupType.Armor)]
        Helmet, //0
        [TypeToSlot(EnumItemSlot.Chest, EnumItemGroupType.Armor)]
        Chest,
        [TypeToSlot(EnumItemSlot.Waist, EnumItemGroupType.Armor)]
        Belt,
        [TypeToSlot(EnumItemSlot.Legs, EnumItemGroupType.Armor)]
        Leggins,
        [TypeToSlot(EnumItemSlot.Feet, EnumItemGroupType.Armor)]
        Boots,
        [TypeToSlot(EnumItemSlot.Shoulders, EnumItemGroupType.Armor)]
        Shoulders,
        [TypeToSlot(EnumItemSlot.Hands, EnumItemGroupType.Armor)]
        Gloves,
        [TypeToSlot(EnumItemSlot.Back, EnumItemGroupType.Armor)]
        Cloak, //8
        [TypeToSlot(EnumItemSlot.Neck, EnumItemGroupType.Jewelery)]
        Amulet, //9
        [TypeToSlot(EnumItemSlot.Finger, EnumItemGroupType.Jewelery)]
        Ring, //10

        [TypeToSlot(EnumItemSlot.OffHand, EnumItemGroupType.Shield)]
        Shield, //11

        [TypeToSlot(EnumItemSlot.OffHand, EnumItemGroupType.OffHand)]
        OffHand, //12
        Unarmed, //13
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Dagger, //14
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Sword,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Axe,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Mace,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Flail,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Polearm,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Staff,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Wand,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Bow,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Crossbow,
        [TypeToSlot(EnumItemSlot.Weapon, EnumItemGroupType.Weapon)]
        Thrown, //24
        [TypeToSlot(EnumItemSlot.Unknown, EnumItemGroupType.Lore)]
        Lore,
        [TypeToSlot(EnumItemSlot.Unknown, EnumItemGroupType.Lore)]
        Bestiary,
    }

    [AttributeUsage(AttributeTargets.All)]
    public class TypeToSlot : Attribute
    {
        public EnumItemSlot Slot { get; set; }
        public EnumItemGroupType GroupType { get; set; }

        private TypeToSlot() { }

        public TypeToSlot(EnumItemSlot slot, EnumItemGroupType groupType)
        {
            Slot = slot;
            GroupType = groupType;
        }
    }
}