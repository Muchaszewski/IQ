using InventoryQuest.Components;
using InventoryQuest.Components.Items;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Creator
{
    /// <summary>
    /// Interaction logic for ListWindow.xaml
    /// </summary>
    public partial class ListWindow : Window
    {
        List<int> _ListID = new List<int>();

        public List<int> ListID
        {
            get { return _ListID; }
            set { _ListID = value; }
        }

        EnumTypeID _TypeID;

        internal EnumTypeID TypeID
        {
            get { return _TypeID; }
            set { _TypeID = value; }
        }

        public ListWindow()
        {
            InitializeComponent();
            PopulatePreview();
            PopulateResult();
            PopulateComboType();
            PopulateComboBoxList();
        }

        public ListWindow(List<int> listID, EnumTypeID typeID)
            : this()
        {
            ListID = listID;
            TypeID = typeID;
            if (typeID == EnumTypeID.Armor)
            {
                ComboBoxType.IsEnabled = true;
            }
            PopulatePreview();
            PopulateResult();
            PopulateComboType();
            PopulateComboBoxList();
        }


        public void PopulatePreview()
        {
            //DataGridPreview.ItemsSource = null;
            //if (TypeID == EnumTypeID.Entities)
            //{
            //    int index = ComboBoxList.SelectedIndex;
            //    if (index >= 0)
            //    {
            //        this.DataGridPreview.ItemsSource = GenerationStorage.Instance.Entities.GeneralList[index].Items;
            //    }
            //}
            //else if (TypeID == EnumTypeID.Armor)
            //{
            //    int index = ComboBoxList.SelectedIndex;
            //    int typeIndex = ComboBoxType.SelectedIndex;
            //    if (index >= 0 && typeIndex >= 0)
            //    {
            //        this.DataGridPreview.ItemsSource = GenerationStorage.Instance.Armors.GeneralList[index].Items[typeIndex];
            //    }
            //}
            //else if (TypeID == EnumTypeID.Weapon)
            //{
            //    int index = ComboBoxList.SelectedIndex;
            //    if (index >= 0)
            //    {
            //        this.DataGridPreview.ItemsSource = GenerationStorage.Instance.Weapons.GeneralList[index].Items;
            //    }
            //}
            //else if (TypeID == EnumTypeID.OffHand)
            //{
            //    int index = ComboBoxList.SelectedIndex;
            //    if (index >= 0)
            //    {
            //        this.DataGridPreview.ItemsSource = GenerationStorage.Instance.OffHands.GeneralList[index].Items;
            //    }
            //}
            //else if (TypeID == EnumTypeID.Shield)
            //{
            //    int index = ComboBoxList.SelectedIndex;
            //    if (index >= 0)
            //    {
            //        this.DataGridPreview.ItemsSource = GenerationStorage.Instance.Shields.GeneralList[index].Items;
            //    }
            //}
            //else if (TypeID == EnumTypeID.Spot)
            //{
            //    ComboBoxList.IsEnabled = false;
            //    this.DataGridPreview.ItemsSource = GenerationStorage.Instance.Spots;
            //}
        }

        public void PopulateComboBoxList()
        {
            //var comboBoxList = new List<string>();
            //if (TypeID == EnumTypeID.Entities)
            //{
            //    foreach (var item in GenerationStorage.Instance.Entities.GeneralList)
            //    {
            //        comboBoxList.Add(item.Name);
            //    }
            //    ComboBoxList.ItemsSource = comboBoxList;
            //}
            //else if (TypeID == EnumTypeID.Armor)
            //{
            //    foreach (var item in GenerationStorage.Instance.Armors.GeneralList)
            //    {
            //        comboBoxList.Add(item.Name);
            //    }
            //    ComboBoxList.ItemsSource = comboBoxList;
            //}
            //else if (TypeID == EnumTypeID.Weapon)
            //{
            //    foreach (var item in GenerationStorage.Instance.Weapons.GeneralList)
            //    {
            //        comboBoxList.Add(item.Name);
            //    }
            //    ComboBoxList.ItemsSource = comboBoxList;
            //}
            //else if (TypeID == EnumTypeID.Shield)
            //{
            //    foreach (var item in GenerationStorage.Instance.Shields.GeneralList)
            //    {
            //        comboBoxList.Add(item.Name);
            //    }
            //    ComboBoxList.ItemsSource = comboBoxList;
            //}
            //else if (TypeID == EnumTypeID.OffHand)
            //{
            //    foreach (var item in GenerationStorage.Instance.OffHands.GeneralList)
            //    {
            //        comboBoxList.Add(item.Name);
            //    }
            //    ComboBoxList.ItemsSource = comboBoxList;
            //}

            //if (comboBoxList.Count != 0)
            //{
            //    ComboBoxList.SelectedIndex = 0;
            //}
        }

        public void PopulateResult()
        {
            ListBoxResult.ItemsSource = null;
            ListBoxResult.ItemsSource = ListID;
        }

        public void PopulateComboType()
        {
            List<string> array = new List<string>();
            array.AddRange(Enum.GetNames(typeof(EnumItemSlot)));
            array.RemoveRange(array.Count - 4, 4);
            ComboBoxType.ItemsSource = array;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBoxList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulatePreview();
        }

        private void ButtonListAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxList.SelectedIndex >= 0)
            {
                if (ListID != null)
                {
                    if (ListID.Contains(ComboBoxList.SelectedIndex) == false)
                    {
                        ListID.Add(ComboBoxList.SelectedIndex);
                        ListID.Sort();
                        PopulateResult();
                    }
                }
            }
        }

        private void DataGridPreview_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TypeID == EnumTypeID.Spot)
            {
                if (DataGridPreview.SelectedIndex >= 0)
                {
                    if (ListID.Contains(DataGridPreview.SelectedIndex) == false)
                    {
                        ListID.Add(DataGridPreview.SelectedIndex);
                        ListID.Sort();
                        PopulateResult();
                    }
                }
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ListBoxResult.SelectedIndex >= 0)
            {
                this.ListID.RemoveAt(ListBoxResult.SelectedIndex);
                PopulateResult();
            }
        }

        private void ComboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulatePreview();
        }
    }
}
