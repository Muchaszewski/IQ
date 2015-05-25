using System;

namespace InventoryQuest.Components.Statistics
{
    [Flags]
    public enum EnumStatItemPartType
    {
        WeaponType = 1,
        ArmorType = 2,
        ShieldType = 4,
        CharacterType = 8,
        BaseType = 16
    }
}