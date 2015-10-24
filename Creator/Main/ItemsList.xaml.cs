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
            var enumItemListType = Enum.GetNames(typeof(EnumItemListType));
            ComboBoxItemListType.ItemsSource = enumItemListType;
            ComboBoxItemListType.SelectedIndex = 0;
            DataGridItemsListsMenu.Items.Clear();
            for (int index = 0; index < enumItemListType.Length; index++)
            {
                var s = enumItemListType[index];
                var menuItem = new MenuItem();
                menuItem.Header = s;
                DataGridItemsListsMenu.Items.Add(menuItem);
                //Access to modified closure
                var delegateIndex = index;
                menuItem.Click += delegate
                {
                    ((ItemsLists)DataGridItemsLists.SelectedItem).ItemListType = (EnumItemListType)delegateIndex;
                    PopulateDataGridItemsLists();
                };
            }
            PopulateDataGridItemsLists();
        }


        private IEnumerable<DisplayNameWeightList> SelectCategoryItemsOfType(int enumItemType)
        {
            var itemTypes = new List<DisplayNameWeightList>();
            var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
            if (groupType == EnumItemGroupType.Armor)
            {
                itemTypes.AddRange(
                    from item in ((ItemsLists)DataGridItemsLists.SelectedItem).ArmorTypeID
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
                        from item in ((ItemsLists)DataGridItemsLists.SelectedItem).JewelerTypeID
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
                    from item in ((ItemsLists)DataGridItemsLists.SelectedItem).ShieldTypeID
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
                    from item in ((ItemsLists)DataGridItemsLists.SelectedItem).OffHandTypeID
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
                    from item in ((ItemsLists)DataGridItemsLists.SelectedItem).WeaponTypeID
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
                    from item in ((ItemsLists)DataGridItemsLists.SelectedItem).LoreTypeID
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

        private void PopulateDataGridItemsLists()
        {
            DataGridItemsLists.ItemsSource = null;
            var list = GenerationStorage.Instance.ItemsLists.FindAll((x) => x.ItemListType == (EnumItemListType)ComboBoxItemListType.SelectedIndex);
            DataGridItemsLists.ItemsSource = list;
        }

        private void PopulateCategoryList()
        {
            //ItemsCategoryList
            var itemsCategoryList = new List<ItemsCategory>();
            string[] enumNamesList = Enum.GetNames(typeof(EnumItemType));

            for (var i = 0; i < enumNamesList.Length; i++)
            {
                if (((ItemsLists)DataGridItemsLists.SelectedItem).Weight.Count <
                    enumNamesList.Length)
                {
                    int count = enumNamesList.Length -
                        ((ItemsLists)DataGridItemsLists.SelectedItem).Weight.Count;
                    for (int j = 0; j < count; j++)
                    {
                        ((ItemsLists)DataGridItemsLists.SelectedItem).Weight.Add(0);
                    }
                }
                int weight = ((ItemsLists)DataGridItemsLists.SelectedItem).Weight[i];
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
            if (DataGridItemsLists.SelectedItem != null && DataGridItemsLists.SelectedItem.GetType().Name != "NamedObject")
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
            if (DataGridItemsLists.SelectedItem != null && DataGridItemsLists.SelectedItem.GetType().Name != "NamedObject")
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
            if (DataGridItemsLists.SelectedItem != null && DataGridItemsLists.SelectedItem.GetType().Name != "NamedObject")
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
            if (DataGridItemsLists.SelectedItem != null && DataGridItemsLists.SelectedItem.GetType().Name != "NamedObject")
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
                        ((ItemsLists)DataGridItemsLists.SelectedItem).ArmorTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    ((ItemsLists)DataGridItemsLists.SelectedItem).ArmorTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
                {
                    if (
                        ((ItemsLists)DataGridItemsLists.SelectedItem).JewelerTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    ((ItemsLists)DataGridItemsLists.SelectedItem).JewelerTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.Shield) //Shields
                {
                    if (
                        ((ItemsLists)DataGridItemsLists.SelectedItem).ShieldTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    ((ItemsLists)DataGridItemsLists.SelectedItem).ShieldTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.OffHand)  //Offhand
                {
                    if (
                        ((ItemsLists)DataGridItemsLists.SelectedItem).OffHandTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    ((ItemsLists)DataGridItemsLists.SelectedItem).OffHandTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                //13 is Unarmed
                else if (groupType == EnumItemGroupType.Weapon) //Weapons
                {
                    if (
                        ((ItemsLists)DataGridItemsLists.SelectedItem).WeaponTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    ((ItemsLists)DataGridItemsLists.SelectedItem).WeaponTypeID.Add(new GenerationWeight
                    {
                        ID = item.ID,
                        Weight = Convert.ToInt32(TextBoxItemsListsWeight.Text)
                    });
                }
                else if (groupType == EnumItemGroupType.Lore) //Lore
                {
                    if (
                        ((ItemsLists)DataGridItemsLists.SelectedItem).LoreTypeID
                            .FirstOrDefault(x => x.ID == item.ID) != null)
                    {
                        continue;
                    }
                    ((ItemsLists)DataGridItemsLists.SelectedItem).LoreTypeID.Add(new GenerationWeight
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
                ((ItemsLists)DataGridItemsLists.SelectedItem).ArmorTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
            {
                ((ItemsLists)DataGridItemsLists.SelectedItem).JewelerTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.Shield) //Shields
            {
                ((ItemsLists)DataGridItemsLists.SelectedItem).ShieldTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.OffHand)  //Offhand
            {
                ((ItemsLists)DataGridItemsLists.SelectedItem).OffHandTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            //13 is Unarmed
            else if (groupType == EnumItemGroupType.Weapon) //Weapons
            {
                ((ItemsLists)DataGridItemsLists.SelectedItem).WeaponTypeID.RemoveAll(
                    x => x.ID == item.ID);
            }
            else if (groupType == EnumItemGroupType.Lore) //Lore
            {
                ((ItemsLists)DataGridItemsLists.SelectedItem).LoreTypeID.RemoveAll(
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
                    ((ItemsLists)DataGridItemsLists.SelectedItem).Weight[i] = Convert.ToInt32((e.EditingElement as TextBox).Text);
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
                    var v = ((ItemsLists)DataGridItemsLists.SelectedItem)
                        .ArmorTypeID.Find(x => x.ID == item.ID);
                    v.Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
                {
                    ((ItemsLists)DataGridItemsLists.SelectedItem)
                        .JewelerTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.Shield) //Shields
                {
                    ((ItemsLists)DataGridItemsLists.SelectedItem)
                        .ShieldTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.OffHand) //Offhand
                {
                    ((ItemsLists)DataGridItemsLists.SelectedItem)
                        .OffHandTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                //13 is Unarmed
                else if (groupType == EnumItemGroupType.Weapon) //Weapons
                {
                    ((ItemsLists)DataGridItemsLists.SelectedItem)
                        .WeaponTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
                else if (groupType == EnumItemGroupType.Lore) //Lore
                {
                    ((ItemsLists)DataGridItemsLists.SelectedItem)
                        .LoreTypeID.Find(x => x.ID == item.ID).Weight =
                        Convert.ToInt32((e.EditingElement as TextBox).Text);
                }
            }
        }

        private void DataGridItemsListsItems_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void ComboBoxItemListType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateDataGridItemsLists();
        }

        private void DataGridItemsLists_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            var item = new ItemsLists();
            item.ItemListType = (EnumItemListType)ComboBoxItemListType.SelectedIndex;
            GenerationStorage.Instance.ItemsLists.Add(item);
            e.NewItem = item;
        }

        private void DataGridItemsLists_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Delete == e.Key)
            {
                GenerationStorage.Instance.ItemsLists.Remove((ItemsLists)DataGridItemsLists.SelectedItem);
            }
        }
    }
}
