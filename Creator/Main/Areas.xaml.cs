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
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Items.Generation.Types;
using UnityEngine;

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

            ComboBoxAreasOnCompelte.ItemsSource = Enum.GetNames(typeof(EnumItemType));
            ComboBoxAreasOnCompelte.SelectedIndex = 0;

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
            DataGridAreaList.ItemsSource = GenerationStorage.Instance.Spots;
        }

        void PopulateOnComplete()
        {
            DataGridAreasItemsOnComplete.ItemsSource =
                ItemUtils.SelectItemsOfType(ComboBoxAreasOnCompelte.SelectedIndex);
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
                DataGridAreaList.ItemsSource = GenerationStorage.Instance.Spots;
                var spot = (Spot)DataGridAreasAll.SelectedItem;
                DataGridAreasItemsList.ItemsSource = spot.ItemsList;
                DataGridAreasMonsterList.ItemsSource = spot.EntitiesList;
                DataGridAreasOnComplete.ItemsSource = spot.ItemOnComplete;
                //Position
                GridPosition.IsEnabled = true;
                TextBoxPositionX.Text = spot.Position.x.ToString();
                TextBoxPositionY.Text = spot.Position.y.ToString();
                TextBoxPositionZ.Text = spot.Position.z.ToString();
                TextBoxMonsterValue.Text = spot.MonsterValueToCompleteArea.ToString();
                TextBoxAreasListDistance.Text = spot.Progress.ToString();
                //Connection List
                DataGridTravelList.ItemsSource = null;
                DataGridTravelList.ItemsSource = spot.ListConnections;
                //
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
                ButtonAreasOnCompleteAdd.IsEnabled = false;
                ButtonAreasOnCompleteRemove.IsEnabled = false;
                GridPosition.IsEnabled = false;
                ButtonAreasListAdd.IsEnabled = false;
                ButtonAreasListRemovey.IsEnabled = false;
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
        {            TextUtils.IsNumeric(ref e, true);
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


        private void DataGridAreasItemsOnComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAreasItemsOnComplete.SelectedItem != null &&
                DataGridAreasItemsOnComplete.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasOnCompleteAdd.IsEnabled = true;
            }
            else
            {
                ButtonAreasOnCompleteAdd.IsEnabled = false;
            }
        }

        private void DataGridAreasOnComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAreasOnComplete.SelectedItem != null &&
                DataGridAreasOnComplete.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasOnCompleteRemove.IsEnabled = true;
            }
            else
            {
                ButtonAreasOnCompleteRemove.IsEnabled = false;
            }
        }
        private void ButtonAreasOnCompleteAdd_Click(object sender, RoutedEventArgs e)
        {
            var itemType = DataGridAreasItemsOnComplete.SelectedItem as ItemType;
            var item = new GenerationWeightLists();
            item.Name = itemType.Name;
            item.Weight = Convert.ToInt32(TextBoxAreasItemsWeight.Text);
            item.Category = itemType.Type.ToString();
            item.ID = itemType.ID;
            var spot = DataGridAreasAll.SelectedItem as Spot;
            if (spot.ItemOnComplete == null)
            {
                spot.ItemOnComplete = new List<GenerationWeightLists>();
            }
            if (spot.ItemOnComplete.FirstOrDefault(x => x.Name == item.Name) == null)
            {
                spot.ItemOnComplete.Add(item);
            }
            DataGridAreasOnComplete.ItemsSource = null;
            DataGridAreasOnComplete.ItemsSource = spot.ItemOnComplete;
        }

        private void ButtonAreasOnCompleteRemove_Click(object sender, RoutedEventArgs e)
        {
            var spot = DataGridAreasAll.SelectedItem as Spot;
            var entity = DataGridAreasOnComplete.SelectedItem as GenerationWeightLists;
            spot.ItemOnComplete.Remove(spot.ItemOnComplete.Find(x => x.Name == entity.Name));
            DataGridAreasOnComplete.ItemsSource = null;
            DataGridAreasOnComplete.ItemsSource = spot.ItemOnComplete;
        }

        private void ComboBoxAreasOnCompelte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateOnComplete();
        }

        private void TextBoxPosition_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, false);
        }

        private void ButtonApplyPosition_Click(object sender, RoutedEventArgs e)
        {
            var item = DataGridAreasAll.SelectedItem as Spot;
            item.Position = new Vector3(
                float.Parse(TextBoxPositionX.Text),
                float.Parse(TextBoxPositionY.Text),
                float.Parse(TextBoxPositionZ.Text));
        }

        private void ButtonApplyMonsterValue_Click(object sender, RoutedEventArgs e)
        {
            var item = DataGridAreasAll.SelectedItem as Spot;
            item.MonsterValueToCompleteArea = int.Parse(TextBoxMonsterValue.Text);
        }

        private void ButtonAreasListAdd_Click(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                var next = (Spot)DataGridAreaList.SelectedItem;
                var current = (Spot)DataGridAreasAll.SelectedItem;
                if (current.ListConnections.FirstOrDefault(x => x.SpotString.Equals(next.Name)) == null)
                {
                    var currentConnection = new Spot.SpotConnection()
                    {
                        Distance = int.Parse(TextBoxAreasListDistance.Text),
                        SpotString = next.Name,
                        IsTwoWay = false,
                    };
                    var nextConnection = Spot.FindConnectionBySpot(next, current);
                    if (nextConnection != null)
                    {
                        nextConnection.IsTwoWay = true;
                        currentConnection.IsTwoWay = true;
                    }
                    current.ListConnections.Add(currentConnection);
                }
            }
            else
            {
                var next = (Spot)DataGridAreaList.SelectedItem;
                var current = (Spot)DataGridAreasAll.SelectedItem;
                if (current.ListConnections.FirstOrDefault(x => x.SpotString.Equals(next.Name)) == null)
                {
                    current.ListConnections.Add(new Spot.SpotConnection()
                    {
                        Distance = int.Parse(TextBoxAreasListDistance.Text),
                        SpotString = next.Name,
                        IsTwoWay = true,
                    });
                    next.ListConnections.Add(new Spot.SpotConnection()
                    {
                        Distance = int.Parse(TextBoxAreasListDistance.Text),
                        SpotString = current.Name,
                        IsTwoWay = true,
                    });
                }
            }
            RefreshArea();
        }

        private void ButtonAreasListRemovey_Click(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                var nextConenction = (Spot.SpotConnection)DataGridTravelList.SelectedItem;
                var next = Spot.FindSpotByConnection(nextConenction);
                var current = (Spot)DataGridAreasAll.SelectedItem;
                var currentConnection = Spot.FindConnectionBySpot(next, current);
                if (currentConnection != null)
                {
                    currentConnection.IsTwoWay = false;
                }
                current.ListConnections.Remove(nextConenction);
            }
            else
            {
                var nextConenction = (Spot.SpotConnection)DataGridTravelList.SelectedItem;
                var next = Spot.FindSpotByConnection(nextConenction);
                var current = (Spot)DataGridAreasAll.SelectedItem;
                var currentConnection = Spot.FindConnectionBySpot(next, current);
                current.ListConnections.Remove(nextConenction);
                next.ListConnections.Remove(currentConnection);
            }
            RefreshArea();
        }


        private void DataGridAreaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridAreaList.SelectedItem != null &&
    DataGridAreaList.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasListAdd.IsEnabled = true;
            }
            else
            {
                ButtonAreasListAdd.IsEnabled = false;
            }
        }

        private void DataGridTravelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridTravelList.SelectedItem != null &&
    DataGridTravelList.SelectedItem.GetType().Name != "NamedObject")
            {
                ButtonAreasListRemovey.IsEnabled = true;
            }
            else
            {
                ButtonAreasListRemovey.IsEnabled = false;
            }
        }

        private void TextBoxSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, false);
        }

        private void ButtonApplySize_Click(object sender, RoutedEventArgs e)
        {
            var item = DataGridAreasAll.SelectedItem as Spot;
            item.Size = float.Parse(TextBoxSize.Text);
        }
    }
}
