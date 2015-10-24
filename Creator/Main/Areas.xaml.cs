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
using System.Windows.Shapes;
using Creator.Utils;
using InventoryQuest.Components;

namespace Creator.Main
{
    /// <summary>
    /// Interaction logic for Areas.xaml
    /// </summary>
    public partial class Areas : UserControl, IGenericTab
    {
        private Window _parentWindow;

        public Areas()
        {
            InitializeComponent();

            _parentWindow = Window.GetWindow(this);

            //Do not execute this part of the code if its in edior
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                PopulateControls();
            }
        }

        public void PopulateControls()
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
                GenericUtils.CenterWindow(_parentWindow, window);

                if (window.DialogResult != null && window.DialogResult == true)
                {
                    item.ImageString = window.ImageString;
                    RefreshArea();
                }
            }
        }

    }
}
