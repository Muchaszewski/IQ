using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Items.Generation.Types;

namespace Creator.Utils
{
    public static class ItemUtils
    {
        public static IEnumerable<ItemType> SelectItemsOfType(int enumItemType)
        {
            IEnumerable<ItemType> itemTypes = null;
            var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
            if (groupType == EnumItemGroupType.Armor)
            {
                itemTypes = GenerationStorage.Instance.Armors.FindAll(x => x.Type == (EnumItemType)enumItemType);
            }
            else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
            {
                itemTypes = GenerationStorage.Instance.Jewelery.FindAll(x => x.Type == (EnumItemType)enumItemType);
            }
            else if (groupType == EnumItemGroupType.Shield) //Shields
            {
                itemTypes = GenerationStorage.Instance.Shields.FindAll(x => x.Type == (EnumItemType)enumItemType);
            }
            else if (groupType == EnumItemGroupType.OffHand)  //Offhand
            {
                itemTypes = GenerationStorage.Instance.OffHands.FindAll(x => x.Type == (EnumItemType)enumItemType);
            }
            //13 is Unarmed
            else if (groupType == EnumItemGroupType.Weapon) //Weapons
            {
                itemTypes = GenerationStorage.Instance.Weapons.FindAll(x => x.Type == (EnumItemType)enumItemType);
            }
            else if (groupType == EnumItemGroupType.Lore) //Weapons
            {
                itemTypes = GenerationStorage.Instance.Lore.FindAll(x => x.Type == (EnumItemType)enumItemType);
            }
            return itemTypes;
        }
    }
}
