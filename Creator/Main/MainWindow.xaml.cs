using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Creator.Utils;
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Entities.Generation.Types;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Items.Generation.Types;
using InventoryQuest.Utils;

namespace Creator.Main
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _parentWindow;

        public MainWindow()
        {
            InitializeComponent();
            _parentWindow = this;

            LoadAll();
        }

        public void LoadAll()
        {
            try
            {
                //PopulateItems();
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Missing Storage.xml file");
                System.Diagnostics.Process.GetCurrentProcess().Close();
                return;
            }
            PopulateMonsterLists();
            PopulateAreas();
        }

        public static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                var controlName = child.GetValue(NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                var result = FindVisualChildByName<T>(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }


        private void ValidateItems()
        {
            ItemType temp = Validation.ValidateItems();
            if (temp != null)
            {
                //ComboBoxItemsType.SelectedIndex = (int)temp.Type;
                // DataGridItemsAll.SelectedItem = temp;
            }
        }

        private void ValidateSpot()
        {
            Spot temp = Validation.ValidateSpot();
            if (temp != null)
            {
                DataGridAreasAll.SelectedItem = temp;
            }
        }

        private void RefreshAll()
        {
            GenerationStorage.SetInstance(GenerationStorage.LoadXml());
            //PopulateDataGridItemsAll();
            LoadAll();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            //GenerationStorage.SaveXml();
            MessageBoxResult Result = MessageBox.Show("Save before quitting?", "Warning", MessageBoxButton.YesNoCancel);
            if (Result == MessageBoxResult.Yes)
            {
                MessageBox.Show("All changes have been saved.");
                GenerationStorage.SaveXml(Properties.Settings.Default.UserSavePath);
            }
            else if (Result == MessageBoxResult.No)
            {
                //Exit without save                
            }
            else if (Result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            GenerationStorage.SaveXml(Properties.Settings.Default.UserSavePath);
        }

        private void MenuItemReload_Click(object sender, RoutedEventArgs e)
        {
            RefreshAll();
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ValidateItems();
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ValidateSpot();
        }

        private void Refresh()
        {
            LoadAll();
            //PopulateDataGridItemsAll();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool isCtrlKey = Keyboard.Modifiers == ModifierKeys.Control ? true : false;
            if (e.Key == Key.S && isCtrlKey)
            {
                GenerationStorage.SaveXml(Properties.Settings.Default.UserSavePath);
                e.Handled = true;
            }
            if (e.Key == Key.G && isCtrlKey)
            {
                ValidateItems();
                e.Handled = true;
            }
            if (e.Key == Key.F && isCtrlKey)
            {
                ValidateSpot();
                e.Handled = true;
            }
            if (e.Key == Key.R && isCtrlKey)
            {
                RefreshAll();
                e.Handled = true;
            }
            if (e.Key == Key.F5)
            {
                Refresh();
                e.Handled = true;
            }
        }

        private void MenuItemRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        #region Areas

        void PopulateAreas()
        {
            DataGridAreasAll.ItemsSource = GenerationStorage.Instance.Spots;
            DataGridAreasMonsterListAll.ItemsSource = GenerationStorage.Instance.EntityLists;
            DataGridAreasItemsListAll.ItemsSource = GenerationStorage.Instance.ItemsLists;
        }

        private void DataGridAreasAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshArea();
        }

        private void RefreshArea()
        {
            if (DataGridAreasAll.SelectedItem != null &&
                DataGridAreasAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var spot = (Spot)DataGridAreasAll.SelectedItem;
                DataGridAreasItemsList.ItemsSource = spot.ItemsList;
                DataGridAreasMonsterList.ItemsSource = spot.EntitiesList;
                if (DataGridAreasMonsterListAll.SelectedItem != null &&
                DataGridAreasMonsterListAll.SelectedItem.GetType().Name != "NamedObject")
                {
                    ButtonAreasMonstersAdd.IsEnabled = true;
                }
                else
                {
                    ButtonAreasMonstersAdd.IsEnabled = false;
                }
                if (DataGridAreasItemsListAll.SelectedItem != null &&
                DataGridAreasItemsListAll.SelectedItem.GetType().Name != "NamedObject")
                {
                    ButtonAreasItemsAdd.IsEnabled = true;
                }
                else
                {
                    ButtonAreasItemsAdd.IsEnabled = false;
                }
                label.Content = spot.ImageString;
            }
            else
            {
                DataGridAreasMonsterList.ItemsSource = null;
                DataGridAreasItemsList.ItemsSource = null;
                ButtonAreasMonstersAdd.IsEnabled = false;
                ButtonAreasMonstersRemove.IsEnabled = false;
                ButtonAreasItemsAdd.IsEnabled = false;
                ButtonAreasItemsRemove.IsEnabled = false;
            }
        }

        private void DataGridAreasMonsterListAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAreasAll.SelectedItem == null || DataGridAreasAll.SelectedItem.GetType().Name == "NamedObject")
                return;
            if (DataGridAreasMonsterListAll.SelectedItem != null &&
                DataGridAreasMonsterListAll.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasMonstersAdd.IsEnabled = true;
            }
            else
            {
                ButtonAreasMonstersAdd.IsEnabled = false;
            }
        }

        private void DataGridAreasItemsListAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAreasAll.SelectedItem == null || DataGridAreasAll.SelectedItem.GetType().Name == "NamedObject")
                return;
            if (DataGridAreasItemsListAll.SelectedItem != null &&
                DataGridAreasItemsListAll.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasItemsAdd.IsEnabled = true;
            }
            else
            {
                ButtonAreasItemsAdd.IsEnabled = false;
            }
        }

        private void DataGridAreasItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAreasItemsList.SelectedItem != null &&
                DataGridAreasItemsList.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasItemsRemove.IsEnabled = true;
            }
            else
            {
                ButtonAreasItemsRemove.IsEnabled = false;
            }
        }

        private void DataGridAreasMonsterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAreasMonsterList.SelectedItem != null &&
                DataGridAreasMonsterList.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasMonstersRemove.IsEnabled = true;
            }
            else
            {
                ButtonAreasMonstersRemove.IsEnabled = false;
            }
        }

        private void TextBoxAreasMonstersWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void TextBoxAreasItemsWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void ButtonAreasMonstersAdd_Click(object sender, RoutedEventArgs e)
        {
            var list = DataGridAreasMonsterListAll.SelectedItem as EntityLists;
            var item = new GenerationWeightLists();
            item.Name = list.Name;
            item.Weight = Convert.ToInt32(TextBoxAreasMonstersWeight.Text);
            var spot = DataGridAreasAll.SelectedItem as Spot;
            if (spot.EntitiesList.FirstOrDefault(x => x.Name == item.Name) == null)
            {
                spot.EntitiesList.Add(item);
            }
            DataGridAreasMonsterList.ItemsSource = null;
            DataGridAreasMonsterList.ItemsSource = spot.EntitiesList;
        }

        private void ButtonAreasItemsAdd_Click(object sender, RoutedEventArgs e)
        {
            var list = DataGridAreasItemsListAll.SelectedItem as ItemsLists;
            var item = new GenerationWeightLists();
            item.Name = list.Name;
            item.Weight = Convert.ToInt32(TextBoxAreasItemsWeight.Text);
            var spot = DataGridAreasAll.SelectedItem as Spot;
            if (spot.ItemsList.FirstOrDefault(x => x.Name == item.Name) == null)
            {
                spot.ItemsList.Add(item);
            }
            DataGridAreasItemsList.ItemsSource = null;
            DataGridAreasItemsList.ItemsSource = spot.ItemsList;
        }

        private void ButtonAreasMonstersRemove_Click(object sender, RoutedEventArgs e)
        {
            var spot = DataGridAreasAll.SelectedItem as Spot;
            var entity = DataGridAreasMonsterList.SelectedItem as GenerationWeightLists;
            spot.EntitiesList.Remove(spot.EntitiesList.Find(x => x.Name == entity.Name));
            DataGridAreasMonsterList.ItemsSource = null;
            DataGridAreasMonsterList.ItemsSource = spot.EntitiesList;
        }

        private void ButtonAreasItemsRemove_Click(object sender, RoutedEventArgs e)
        {
            var spot = DataGridAreasAll.SelectedItem as Spot;
            var item = DataGridAreasItemsList.SelectedItem as GenerationWeightLists;
            spot.ItemsList.Remove(spot.ItemsList.Find(x => x.Name == item.Name));
            DataGridAreasItemsList.ItemsSource = null;
            DataGridAreasItemsList.ItemsSource = spot.ItemsList;
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            if (DataGridAreasAll.SelectedItem != null && DataGridAreasAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridAreasAll.SelectedItem as Spot;
                if (item.ImageString == null)
                {
                    item.ImageString = "";
                }
                var window = new ImageAreaWindow(item.ImageString);
                window.Owner = this;
                window.ShowDialog();

                if (window.DialogResult != null && window.DialogResult == true)
                {
                    item.ImageString = window.ImageString;
                    RefreshArea();
                }
            }
        }

        #endregion Areas


        #region MonsterLists

        private ComboBox comboBoxMonsterLists;

        private void PopulateMonsterLists()
        {
            //MonsterLists
            DataGridMonsterLists.ItemsSource = GenerationStorage.Instance.EntityLists;
            ////Monster Lists

            //MonsterItems
            DataGridMonsterListsAllItems.ItemsSource = GenerationStorage.Instance.Entities;
            ////MonsterItems
        }

        //DataGrid Actions
        private void DataGridMonsterLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MonsterListsItems
            if (comboBoxMonsterLists == null)
            {
                comboBoxMonsterLists = FindVisualChildByName<ComboBox>(DataGridMonsterListsItems,
                    "DataGridMonsterListsItemsRarity");
            }
            PopulateDataGridMonstersListsItems();
            MonterValidateButtons();
            ////MonsterListsItems
        }

        private void PopulateDataGridMonstersListsItems()
        {
            if (DataGridMonsterLists.SelectedIndex != -1 &&
                DataGridMonsterLists.SelectedIndex < GenerationStorage.Instance.EntityLists.Count)
            {
                var list = (EntityLists)DataGridMonsterLists.SelectedItem;
                var displayList = new List<DisplayNameWeightList>();
                switch ((EnumEntityRarity)comboBoxMonsterLists.SelectedIndex)
                {
                    case EnumEntityRarity.Normal:
                        PopulateDataGridMonstersListsItemsEnumCase(list.EntityTypeNormalID, ref displayList);
                        break;
                    case EnumEntityRarity.Uncommon:
                        PopulateDataGridMonstersListsItemsEnumCase(list.EntityTypeUncommonID, ref displayList);
                        break;
                    case EnumEntityRarity.Rare:
                        PopulateDataGridMonstersListsItemsEnumCase(list.EntityTypeRareID, ref displayList);
                        break;
                }
                DataGridMonsterListsItems.ItemsSource = displayList;
            }
            else
            {
                DataGridMonsterListsItems.ItemsSource = null;
            }
        }

        /// <summary>
        ///     Add items to DataGridMonstersListsItems
        /// </summary>
        /// <param name="list"></param>
        /// <param name="displayList"></param>
        private void PopulateDataGridMonstersListsItemsEnumCase(List<GenerationWeight> list,
            ref List<DisplayNameWeightList> displayList)
        {
            displayList.AddRange(list.Select(item => new DisplayNameWeightList
            {
                Name = GenerationStorage.Instance.Entities[item.ID].Name,
                ID = item.ID,
                Weight = item.Weight
            }));
        }

        private void DataGridMonsterItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MonterValidateButtons();
        }

        private void MonterValidateButtons()
        {
            if (DataGridMonsterLists.SelectedIndex != -1 &&
                DataGridMonsterLists.SelectedIndex < GenerationStorage.Instance.EntityLists.Count)
            {
                ButtonMonstersListsRemove.IsEnabled = true;

                ButtonMonsterListsAdd.IsEnabled = DataGridMonsterListsAllItems.SelectedIndex != -1;
            }
            else
            {
                ButtonMonsterListsAdd.IsEnabled = false;
                ButtonMonstersListsRemove.IsEnabled = false;
            }
        }

        private void DataGridMonsterListsItemsRarity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateDataGridMonstersListsItems();
        }

        ////DataGrid Actions

        //Buttons

        private void MonsterListsAdd_Click(object sender, RoutedEventArgs e)
        {
            var list = (EntityLists)DataGridMonsterLists.SelectedItem;

            switch ((EnumEntityRarity)comboBoxMonsterLists.SelectedIndex)
            {
                case EnumEntityRarity.Normal:
                    MonsterListsAdd(list.EntityTypeNormalID);
                    break;
                case EnumEntityRarity.Uncommon:
                    MonsterListsAdd(list.EntityTypeUncommonID);
                    break;
                case EnumEntityRarity.Rare:
                    MonsterListsAdd(list.EntityTypeRareID);
                    break;
            }
        }

        private void MonsterListsAdd(List<GenerationWeight> list)
        {
            try
            {
                foreach (EntityType item in DataGridMonsterListsAllItems.SelectedItems)
                {
                    if (list.FirstOrDefault(x => x.ID == item.ID) == null)
                    {
                        list.Add(new GenerationWeight { ID = item.ID, Weight = Convert.ToInt32(TextBoxMonsterLists.Text) });
                    }
                }

                PopulateDataGridMonstersListsItems();
                DataGridMonsterLists.InvalidateVisual();
            }
            catch
            {
                MessageBox.Show("TextBox Weight cannot be Empty");
            }
        }

        private void MonsterListsRemove_Click(object sender, RoutedEventArgs e)
        {
            var list = (EntityLists)DataGridMonsterLists.SelectedItem;

            switch ((EnumEntityRarity)comboBoxMonsterLists.SelectedIndex)
            {
                case EnumEntityRarity.Normal:
                    MonsterListsRemove(list.EntityTypeNormalID);
                    break;
                case EnumEntityRarity.Uncommon:
                    MonsterListsRemove(list.EntityTypeUncommonID);
                    break;
                case EnumEntityRarity.Rare:
                    MonsterListsRemove(list.EntityTypeRareID);
                    break;
            }
        }

        private void MonsterListsRemove(List<GenerationWeight> list)
        {
            foreach (DisplayNameWeightList item in DataGridMonsterListsItems.SelectedItems)
            {
                list.RemoveAll(x => x.ID == item.ID);
            }

            PopulateDataGridMonstersListsItems();
            DataGridMonsterLists.InvalidateVisual();
        }

        ////Buttons

        //TextBox
        private void TextBoxMonsterLists_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void MenuItemOptions_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Options();
            dialog.ShowDialog();
        }
      

        ////TextBox

        #endregion MonsterLists

        //

    }
}