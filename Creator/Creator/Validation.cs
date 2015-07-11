using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Items.Generation.Types;

namespace Creator
{
    public class Validation
    {

        class ItemIndex
        {
            public ItemType Item { get; set; }
            public int Index { get; set; }
            public IEnumerable<ItemType> Storage { get; set; }
        }

        public static ItemType ValidateItems()
        {
            foreach (var item in EnumerateItems())
            {
                if (!CheckProperty(item.Item, item.Item.Name, "Name", false, String.Empty, "!MISSING!!NAME!")) return item.Item;
                if (!CheckProperty(item.Item, item.Item.Durability.Min, "Durability.Min", false, 0)) return item.Item;
                if (!CheckProperty(item.Item, item.Item.Durability.Max, "Durability.Max", false, 0)) return item.Item;
                if (!CheckProperty(item.Item, item.Item.ImageID.Count, "ImageID.Count", false, 0)) return item.Item;
                if (!CheckProperty(item.Item, item.Item.ID, "Item.ID", true, item.Storage.ElementAt(item.Index).ID)) return item.Item;
                if (item.Item.GetType().Name == "WeaponType")
                {
                    var weapon = (WeaponType)item.Item;
                    var damage = Math.Max(weapon.MinDamage.Min - weapon.MaxDamage.Max - 1, -1);
                    if (!CheckProperty(weapon, damage, "Min > Max", true, -1)) return item.Item;
                }
            }
            return null;
        }


        private static IEnumerable<ItemIndex> EnumerateItems()
        {
            for (int index = 0; index < GenerationStorage.Instance.Armors.Count; index++)
            {
                var item = GenerationStorage.Instance.Armors[index];
                yield return new ItemIndex { Index = index, Item = item, Storage = GenerationStorage.Instance.Armors };
            }
            for (int index = 0; index < GenerationStorage.Instance.Weapons.Count; index++)
            {
                var item = GenerationStorage.Instance.Weapons[index];
                yield return new ItemIndex { Index = index, Item = item, Storage = GenerationStorage.Instance.Weapons };
            }
            for (int index = 0; index < GenerationStorage.Instance.OffHands.Count; index++)
            {
                var item = GenerationStorage.Instance.OffHands[index];
                yield return new ItemIndex { Index = index, Item = item, Storage = GenerationStorage.Instance.OffHands };
            }
            for (int index = 0; index < GenerationStorage.Instance.Shields.Count; index++)
            {
                var item = GenerationStorage.Instance.Shields[index];
                yield return new ItemIndex { Index = index, Item = item, Storage = GenerationStorage.Instance.Shields };
            }
        }


        private static bool CheckProperty(ItemType item, object toCheck, string name, bool isValid, params object[] notValidOperations)
        {
            foreach (var operation in notValidOperations)
            {
                string messageBoxText = "Item ID " + item.ID + " Name " + item.Name + " has invalid " +
                                  name + " with value " + operation;
                if (!isValid)
                {
                    if (toCheck.Equals(operation))
                    {
                        MessageBox.Show(messageBoxText);
                        return false;
                    }
                }
                else
                {
                    if (!toCheck.Equals(operation))
                    {
                        MessageBox.Show(messageBoxText);
                        return false;
                    }
                }
            }
            return true;
        }

        public static Spot ValidateSpot()
        {
            foreach (Spot spot in GenerationStorage.Instance.Spots)
            {
                if (!CheckProperty(spot, spot.Name, "Name", false, String.Empty, GenerationStorage.Instance.Spots.Find(x => x.Name == spot.Name))) return spot;
                if (!CheckProperty(spot, spot.ItemsList.Count, "ItemList", false, 0)) return spot;
                if (!CheckProperty(spot, spot.ItemsTotalWeight, "ItemsWeight", false, 0)) return spot;
                if (!CheckProperty(spot, spot.EntitiesList.Count, "EntitiesList", false, 0)) return spot;
                if (!CheckProperty(spot, spot.EntitiesTotalWeight, "EntitiesWeight", false, 0)) return spot;


            }
            return null;
        }

        private static bool CheckProperty(Spot spot, object toCheck, string name, bool isValid, params object[] notValidOperations)
        {
            foreach (var operation in notValidOperations)
            {
                string messageBoxText = "Spot Name " + spot.Name + " has invalid " +
                                  name + " with value " + operation;
                if (!isValid)
                {
                    if (toCheck.Equals(operation))
                    {
                        MessageBox.Show(messageBoxText);
                        return false;
                    }
                }
                else
                {
                    if (!toCheck.Equals(operation))
                    {
                        MessageBox.Show(messageBoxText);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
