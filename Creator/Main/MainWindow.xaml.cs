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
                //DataGridAreasAll.SelectedItem = temp;
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

        private void MenuItemOptions_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Options();
            dialog.ShowDialog();
        }

    }
}