using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Creator.Utils;
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Items.Generation.Types;

namespace Creator.Main
{
    /// <summary>
    /// Interaction logic for ItemsList.xaml
    /// </summary>
    public partial class ItemsList : UserControl, IGenericTab
    {
        public ItemsList()
        {
            InitializeComponent();

            //Do not execute this part of the code if its in edior
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                PopulateControls();
            }
        }

        private class ItemsCategory
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public int Weight { get; set; }
        }

        private IEnumerable<ItemType> ItemsOfSelectedType;

        public void PopulateControls()
        {
            DataGridItemsLists.ItemsSource = GenerationStorage.Instance.ItemsLists;
        }

        private IEnumerable<DisplayNameWeightList> SelectCategoryItemsOfType(int enumItemType)
        {
            var itemTypes = new List<DisplayNameWeightList>();
            var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
            if (groupType == EnumItemGroupType.Armor)
            {
                itemTypes.AddRange(
                    from item in GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ArmorTypeID
                    let tItem = GenerationStorage.Instance.Armors[item.ID]
                    where (int)tItem.Type == enumItemType
                    select new DisplayNameWeightList
                    {
                        ID = item.ID,
                        Name = tItem.Name,
                        Weight = item.Weight
                    });
            }
            else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
            {
                try
                {
                    itemTypes.AddRange(
                        from item in GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].JewelerTypeID
                        let tItem = GenerationStorage.Instance.Jewelery[item.ID]
                        where (int)tItem.Type == enumItemType
                        select new DisplayNameWeightList
                        {
                            ID = item.ID,
                            Name = tItem.Name,
                            Weight = item.Weight
                        });
                }
                catch { }
            }
            else if (groupType == EnumItemGroupType.Shield) //Shields
            {
                itemTypes.AddRange(
                    from item in GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ShieldTypeID
                    let tItem = GenerationStorage.Instance.Shields[item.ID]
                    where (int)tItem.Type == enumItemType
                    select new DisplayNameWeightList
                    {
                        ID = item.ID,
                        Name = tItem.Name,
                        Weight = item.Weight
                    });
            }
            else if (groupType == EnumItemGroupType.OffHand)  //Offhand
            {
                itemTypes.AddRange(
                    from item in GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].OffHandTypeID
                    let tItem = GenerationStorage.Instance.OffHands[item.ID]
                    where (int)tItem.Type == enumItemType
                    select new DisplayNameWeightList
                    {
                        ID = item.ID,
                        Name = tItem.Name,
                        Weight = item.Weight
                    });
            }
            //13 is Unarmed
            else if (groupType == EnumItemGroupType.Weapon) //Weapons
            {
                itemTypes.AddRange(
                    from item in GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].WeaponTypeID
                    let tItem = GenerationStorage.Instance.Weapons[item.ID]
                    where (int)tItem.Type == enumItemType
                    select new DisplayNameWeightList
                    {
                        ID = item.ID,
                        Name = tItem.Name,
                        Weight = item.Weight
                    });
            }
            else if (groupType == EnumItemGroupType.Lore) //Lore
            {
                itemTypes.AddRange(
                    from item in GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].LoreTypeID
                    let tItem = GenerationStorage.Instance.Lore[item.ID]
                    where (int)tItem.Type == enumItemType
                    select new DisplayNameWeightList
                    {
                        ID = item.ID,
                        Name = tItem.Name,
                        Weight = item.Weight
                    });
            }
            return itemTypes;
        }


        private void PopulateCategoryList()
        {
            //ItemsCategoryList
            var itemsCategoryList = new List<ItemsCategory>();
            string[] enumNamesList = Enum.GetNames(typeof(EnumItemType));

            for (var i = 0; i < enumNamesList.Length; i++)
            {
                if (GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].Weight.Count <
                    enumNamesList.Length)
                {
                    int count = enumNamesList.Length -
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].Weight.Count;
                    for (int j = 0; j < count; j++)
                    {
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].Weight.Add(0);
                    }
                }
                int weight = GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].Weight[i];
                IEnumerable<DisplayNameWeightList> itemTypes = SelectCategoryItemsOfType(i);
                itemsCategoryList.Add(itemTypes == null
                    ? new ItemsCategory { Name = enumNamesList[i], Count = 0, Weight = weight }
                    : new ItemsCategory { Name = enumNamesList[i], Count = itemTypes.Count(), Weight = weight });
            }
            DataGridItemsCategoryList.ItemsSource = null;
            DataGridItemsCategoryList.ItemsSource = itemsCategoryList;
            ////ItemsCategoryList
        }

        private void DataGridItemsCategoryList_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void DataGridItemsLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsLists.SelectedIndex != -1 &&
                DataGridItemsLists.SelectedIndex < GenerationStorage.Instance.ItemsLists.Count)
            {
                PopulateCategoryList();
            }
            else
            {
                DataGridItemsCategoryList.ItemsSource = null;
            }
        }

        private void PopulateItemsListsItems()
        {
            if (DataGridItemsLists.SelectedIndex != -1 &&
                DataGridItemsLists.SelectedIndex < GenerationStorage.Instance.ItemsLists.Count)
            {
                if (DataGridItemsCategoryList.SelectedIndex != -1)
                {
                    ItemsOfSelectedType = ItemUtils.SelectItemsOfType(DataGridItemsCategoryList.SelectedIndex);

                    DataGridItemsListsAll.ItemsSource = ItemsOfSelectedType;

                    DataGridItemsListsItems.ItemsSource =
                        SelectCategoryItemsOfType(DataGridItemsCategoryList.SelectedIndex);
                }
                else
                {
                    DataGridItemsListsAll.ItemsSource = null;
                    DataGridItemsListsItems.ItemsSource = null;
                }
            }
            else
            {
                DataGridItemsListsAll.ItemsSource = null;
                DataGridItemsListsItems.ItemsSource = null;
            }
        }

        private void DataGridItemsCategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateItemsListsItems();
        }

        private void DataGridItemsListsAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsLists.SelectedIndex != -1 &&
                DataGridItemsLists.SelectedIndex < GenerationStorage.Instance.ItemsLists.Count)
            {
                if (DataGridItemsCategoryList.SelectedIndex != -1)
                {
                    if (DataGridItemsListsAll.SelectedIndex != -1 &&
                        DataGridItemsListsAll.SelectedIndex < ItemsOfSelectedType.Count())
                    {
                        ButtonItemsListsAdd.IsEnabled = true;
                    }
                    else
                    {
                        ButtonItemsListsAdd.IsEnabled = false;
                    }
                }
                else
                {
                    ButtonItemsListsAdd.IsEnabled = false;
                }
            }
            else
            {
                ButtonItemsListsAdd.IsEnabled = false;
            }
        }

        private void DataGridItemsListsItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsLists.SelectedIndex != -1 &&
                DataGridItemsLists.SelectedIndex < GenerationStorage.Instance.ItemsLists.Count)
            {
                if (DataGridItemsCategoryList.SelectedIndex != -1)
                {
                    ButtonItemsListsRemove.IsEnabled = DataGridItemsListsItems.SelectedIndex != -1;
                }
                else
                {
                    ButtonItemsListsRemove.IsEnabled = false;
                }
            }
            else
            {
                ButtonItemsListsRemove.IsEnabled = false;
            }
        }

        //Buttons

        private void ButtonItemsListsAdd_Click(object sender, RoutedEventArgs e)
        {
            var enumItemType = DataGridItemsCategoryList.SelectedIndex;
            IEnumerable<ItemType> items = DataGridItemsListsAll.SelectedItems.Cast<ItemType>();
            foreach (ItemType item in items)
            {
                var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
                if (groupType == EnumItemGroupType.Armor)
                {
                    if (
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ArmorTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ArmorTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
                {
                    if (
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].JewelerTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].JewelerTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.Shield) //Shields
                {
                    if (
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ShieldTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ShieldTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.OffHand)  //Offhand
                {
                    if (
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].OffHandTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].OffHandTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                //13 is Unarmed
                else if (groupType == EnumItemGroupType.Weapon) //Weapons
                {
                    if (
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].WeaponTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].WeaponTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.Lore) //Lore
                {
                    if (
                        GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].LoreTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].LoreTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                PopulateItemsListsItems();
            }
        }

        private void ButtonItemsListsRemove_Click(object sender, RoutedEventArgs e)
        {
            var enumItemType = DataGridItemsCategoryList.SelectedIndex;
            var item = DataGridItemsListsItems.SelectedItem as DisplayNameWeightList;

            var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
            if (groupType == EnumItemGroupType.Armor)
            {
                GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ArmorTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
            {
                GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].JewelerTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.Shield) //Shields
            {
                GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].ShieldTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.OffHand)  //Offhand
            {
                GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].OffHandTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            //13 is Unarmed
            else if (groupType == EnumItemGroupType.Weapon) //Weapons
            {
                GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].WeaponTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.Lore) //Lore
            {
                GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].LoreTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            PopulateItemsListsItems();
        }

        private void TextBoxItemsListsWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void DataGridItemsCategoryList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string[] enumNamesList = Enum.GetNames(typeof(EnumItemType));
            var item = DataGridItemsCategoryList.SelectedItem as ItemsCategory;
            for (int i = 0; i < enumNamesList.Count(); i++)
            {
                if (enumNamesList[i] == item.Name)
                {
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex].Weight[i] = Convert.ToInt32((e.EditingElement as TextBox).Text);
                    break;
                }
            }
        }


        private void DataGridItemsListsItems_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var item = (DisplayNameWeightList)DataGridItemsListsItems.SelectedItem;
            var enumItemType = DataGridItemsCategoryList.SelectedIndex;
            if (DataGridItemsCategoryList.SelectedIndex != -1 && item != null)
            {
                var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
                if (groupType == EnumItemGroupType.Armor)
                {
                    var v = GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex]
                        .ArmorTypeID.Find(x => x.ID == item.ID);
                    v.Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
                {
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex]
                        .JewelerTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.Shield) //Shields
                {
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex]
                        .ShieldTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.OffHand) //Offhand
                {
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex]
                        .OffHandTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                //13 is Unarmed
                else if (groupType == EnumItemGroupType.Weapon) //Weapons
                {
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex]
                        .WeaponTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.Lore) //Lore
                {
                    GenerationStorage.Instance.ItemsLists[DataGridItemsLists.SelectedIndex]
                        .LoreTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
            }
        }

        private void DataGridItemsListsItems_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }
    }
}
