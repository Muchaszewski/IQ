using System;
using System.Collections.Generic;
using System.Reflection;

namespace InventoryQuest.Components.Items
{
    public enum EnumItemType
    {
        [TypeToSlot(EnumItemSlot.Head)]
        Helmet, //0
        [TypeToSlot(EnumItemSlot.Chest)]
        Chest,
        [TypeToSlot(EnumItemSlot.Waist)]
        Belt,
        [TypeToSlot(EnumItemSlot.Legs)]
        Leggins,
        [TypeToSlot(EnumItemSlot.Feet)]
        Boots,
        [TypeToSlot(EnumItemSlot.Shoulders)]
        Shoulders,
        [TypeToSlot(EnumItemSlot.Hands)]
        Gloves,
        [TypeToSlot(EnumItemSlot.Back)]
        Cloak, //8
        [TypeToSlot(EnumItemSlot.Neck)]
        Amulet, //9
        [TypeToSlot(EnumItemSlot.Finger)]
        Ring, //10

        [TypeToSlot(EnumItemSlot.OffHand)]
        Shield, //11

        [TypeToSlot(EnumItemSlot.OffHand)]
        OffHand, //12
        Unarmed, //13
        [TypeToSlot(EnumItemSlot.Weapon)]
        Dagger, //14
        [TypeToSlot(EnumItemSlot.Weapon)]
        Sword,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Axe,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Mace,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Flail,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Polearm,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Staff,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Wand,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Bow,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Crossbow,
        [TypeToSlot(EnumItemSlot.Weapon)]
        Thrown, //24
        [TypeToSlot(EnumItemSlot.Unknown)]
        Lore,
        [TypeToSlot(EnumItemSlot.Unknown)]
        Bestiary,
    }

    [AttributeUsage(AttributeTargets.All)]
    public class TypeToSlot : Attribute
    {
        public EnumItemSlot Slot { get; set; }

        private TypeToSlot() {}

        public TypeToSlot(EnumItemSlot slot)
        {
            Slot = slot;
        }
    }
}