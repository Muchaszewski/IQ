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
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Entities.Generation.Types;

namespace Creator.Main
{
    /// <summary>
    /// Interaction logic for MonsterLists.xaml
    /// </summary>
    public partial class MonsterLists : UserControl, IGenericTab
    {

        private ComboBox _comboBoxMonsterLists;
        public MonsterLists()
        {
            InitializeComponent();
            //Do not execute this part of the code if its in edior
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                PopulateControls();
            }
        }


        public void PopulateControls()
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
            if (_comboBoxMonsterLists == null)
            {
                _comboBoxMonsterLists = GenericUtils.FindVisualChildByName<ComboBox>(DataGridMonsterListsItems,
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
                switch ((EnumEntityRarity)_comboBoxMonsterLists.SelectedIndex)
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

            switch ((EnumEntityRarity)_comboBoxMonsterLists.SelectedIndex)
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

            switch ((EnumEntityRarity)_comboBoxMonsterLists.SelectedIndex)
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

    }
}
