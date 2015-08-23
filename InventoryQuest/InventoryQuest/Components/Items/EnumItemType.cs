using System;
using System.Collections.Generic;
using System.Reflection;

namespace InventoryQuest.Components.Items
{
    public enum EnumItemType
    {
        Helmet, //0
        Chest,
        Belt,
        Leggins,
        Boots,
        Arms,
        Shoulders,
        Gloves,
        Cloak, //8
        Amulet, //9
        Ring, //10

        Shield, //11

        OffHand, //12
        Unarmed, //13
        Dagger, //14
        Sword,
        Axe,
        Mace,
        Flail,
        Polearm,
        Staff,
        Wand,
        Bow,
        Crossbow,
        Thrown, //24
        Lore,
        Bestiary,
    }

    [AttributeUsage(AttributeTargets.All)]
    public class DropChance : Attribute
    {
        private static List<DropChance> _Items = new List<DropChance>();

        static DropChance()
        {
            MemberInfo[] mem = typeof (EnumItemType).GetMembers();
            foreach (MemberInfo item in mem)
            {
                if (item.DeclaringType == typeof(EnumItemType) && item.GetCustomAttributes(typeof(EnumItemType), false).Length != 0)
                {
                    var attrib = (DropChance) item.GetCustomAttributes(typeof (DropChance), false)[0];
                    Items.Add(attrib);
                    MaxChances += Items[Items.Count - 1].Chance;
                }
            }
        }

        public DropChance(int chance)
        {
            Chance = chance;
        }

        public static int MaxChances { get; private set; }

        public static List<DropChance> Items
        {
            get { return _Items; }
            private set { _Items = value; }
        }

        public int Chance { get; set; }
    }
}