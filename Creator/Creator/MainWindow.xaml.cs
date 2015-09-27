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
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Entities.Generation.Types;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Items.Generation.Types;
using InventoryQuest.Components.Statistics;
using InventoryQuest.Utils;

namespace Creator
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<List<Image>> _imagesItemsList;
        private List<List<Image>> _imagesMonstersList;


        public MainWindow()
        {
            InitializeComponent();
            _imagesItemsList = LoadAllItemsImages();
            _imagesMonstersList = LoadAllMonstersImages();
            LoadAll();
            DataGridMonsterAllItems.SelectedIndex = 0;
        }

        public void LoadAll()
        {
            try
            {
                PopulateItems();
            }
            catch (TypeInitializationException)
            {
                MessageBox.Show("Missing Storage.xml file");
                System.Diagnostics.Process.GetCurrentProcess().Close();
                return;
            }
            PopulateItemsList();
            PopulateMonster();
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

        private static List<List<Image>> LoadAllItemsImages()
        {
            List<List<Image>> images = new List<List<Image>>();
            List<string> names = ResourcesManager.GetAllFiles("", false).ToList();
            names.RemoveAt(names.Count - 1);
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                images.Add(new List<Image>());
                foreach (var item in ResourcesManager.GetAllFiles(name))
                {
                    var bi = new BitmapImage(new Uri(item, UriKind.Absolute));
                    images[i].Add(new Image() { Source = bi, Stretch = Stretch.Fill, Width = 64, Height = 118 });
                }
            }
            var weaponNames = ResourcesManager.GetAllFiles("weapon", false);
            for (int i = 0; i < weaponNames.Length; i++)
            {
                var weaponName = weaponNames[i];
                images.Add(new List<Image>());
                foreach (var item in ResourcesManager.GetAllFiles(@"weapon/" + weaponName))
                {
                    var bi = new BitmapImage(new Uri(item, UriKind.Absolute));
                    images[i + names.Count].Add(new Image() { Source = bi, Stretch = Stretch.Fill, Width = 64, Height = 118 });
                }
            }
            return images;
        }

        private static List<List<Image>> LoadAllMonstersImages()
        {
            List<List<Image>> images = new List<List<Image>>();
            List<string> names = ResourcesManager.GetAllFiles("../portraits", false).ToList();
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                images.Add(new List<Image>());
                foreach (var item in ResourcesManager.GetAllFiles(Path.Combine("../portraits", name)))
                {
                    var bi = new BitmapImage(new Uri(item, UriKind.Absolute));
                    images[i].Add(new Image() { Source = bi, Stretch = Stretch.Fill, Width = 113, Height = 113 });
                }
            }
            return images;
        }

        private void ValidateItems()
        {
            ItemType temp = Validation.ValidateItems();
            if (temp != null)
            {
                ComboBoxItemsType.SelectedIndex = (int)temp.Type;
                DataGridItemsAll.SelectedItem = temp;
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
            PopulateDataGridItemsAll();
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
            PopulateDataGridItemsAll();
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


        #region Items

        private void PopulateItems()
        {
            ComboBoxItemsType.ItemsSource = Enum.GetNames(typeof(EnumItemType));
            ComboBoxItemsType.SelectedIndex = 0;
            ComboBoxItemType.ItemsSource = Enum.GetNames(typeof(EnumItemType));
            ComboBoxItemSkill.ItemsSource = Enum.GetNames(typeof(EnumItemClassSkill));
            ComboBoxRarity.ItemsSource = Enum.GetNames(typeof(EnumItemRarity));
            ComboBoxItemRequiredHands.ItemsSource = Enum.GetNames(typeof(EnumItemHands));
        }

        private void DataGridItemsAll_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            var item = new ItemType();
            var enumItemType = ComboBoxItemsType.SelectedIndex;
            var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
            if (groupType == EnumItemGroupType.Armor)
            {
                item = new ArmorType();
                item.Type = (EnumItemType)enumItemType;
                item.ID = GenerationStorage.Instance.Armors.Count;
                GenerationStorage.Instance.Armors.Add((ArmorType)item);
            }
            else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
            {
                item = new JeweleryType();
                item.Type = (EnumItemType)enumItemType;
                item.ID = GenerationStorage.Instance.Jewelery.Count;
                GenerationStorage.Instance.Jewelery.Add((JeweleryType)item);
            }
            else if (groupType == EnumItemGroupType.Shield) //Shields
            {
                item = new ShieldType();
                item.Type = (EnumItemType)enumItemType;
                item.ID = GenerationStorage.Instance.Shields.Count;
                GenerationStorage.Instance.Shields.Add((ShieldType)item);
            }
            else if (groupType == EnumItemGroupType.OffHand)  //Offhand
            {
                item = new OffHandType();
                item.Type = (EnumItemType)enumItemType;
                item.ID = GenerationStorage.Instance.OffHands.Count;
                GenerationStorage.Instance.OffHands.Add((OffHandType)item);
            }
            //13 is Unarmed
            else if (groupType == EnumItemGroupType.Weapon) //Weapons
            {
                item = new WeaponType();
                item.Type = (EnumItemType)enumItemType;
                item.ID = GenerationStorage.Instance.Weapons.Count;
                GenerationStorage.Instance.Weapons.Add((WeaponType)item);
            }
            else if (groupType == EnumItemGroupType.Lore) //Lore and Bestiary
            {
                item = new LoreType();
                item.Type = (EnumItemType)enumItemType;
                item.ID = GenerationStorage.Instance.Lore.Count;
                GenerationStorage.Instance.Lore.Add((LoreType)item);
            }

            e.NewItem = item;
        }

        private void PopulateDataGridItemsAll()
        {
            DataGridItemsAll.ItemsSource = SelectItemsOfType(ComboBoxItemsType.SelectedIndex);
        }

        private void ItemsHideAllTabs()
        {
            ItemsTabArmor.Visibility = Visibility.Collapsed;
            ItemsTabShield.Visibility = Visibility.Collapsed;
            ItemsTabWeapon.Visibility = Visibility.Collapsed;
        }

        private void ComboBoxItemsType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateDataGridItemsAll();
            ItemsHideAllTabs();
            var enumItemType = ComboBoxItemsType.SelectedIndex;
            var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
            if (groupType == EnumItemGroupType.Armor)
            {
                ItemsTabArmor.Visibility = Visibility.Visible;
            }
            else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
            {
            }
            else if (groupType == EnumItemGroupType.Shield) //Shields
            {
                ItemsTabShield.Visibility = Visibility.Visible;
            }
            else if (groupType == EnumItemGroupType.OffHand)  //Offhand
            {
            }
            //13 is Unarmed
            else if (groupType == EnumItemGroupType.Weapon) //Weapons
            {
                ItemsTabWeapon.Visibility = Visibility.Visible;
            }
        }

        private static ImageIDPair ResolveItemImage(string type, string item)
        {
            var image = new ImageIDPair();
            image.ImageIDType = ImagesNames.ItemsImageNames.FindIndex(x => x.Name == type);
            image.ImageIDItem = ImagesNames.ItemsImageNames[image.ImageIDType].List.FindIndex(x => x == item);
            return image;
        }

        private static ImageIDPair ResolveMonsterImage(string type, string item)
        {
            var image = new ImageIDPair();
            image.ImageIDType = ImagesNames.MonstersImageNames.FindIndex(x => x.Name == type);
            image.ImageIDItem = ImagesNames.MonstersImageNames[image.ImageIDType].List.FindIndex(x => x == item);
            return image;
        }

        private void RefreshItems()
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = (ItemType)DataGridItemsAll.SelectedItem;
                TextBoxItemsName.Text = item.Name;
                TextBoxItemsExtraName.Text = item.ExtraName;
                TextBoxItemsFlavor.Text = item.FlavorText;
                ComboBoxItemType.SelectedIndex = (int)item.Type;
                ComboBoxItemSkill.SelectedIndex = (int)item.Skill;
                if (item.Durability == null)
                {
                    ButtonItemDurability.Content = 0.ToString();
                }
                else
                {
                    ButtonItemDurability.Content = item.Durability.ToString();
                }
                ComboBoxRarity.SelectedIndex = (int)item.Rarity;
                TextBoxItemRequired.Text = item.RequiredLevel.ToString();
                TextBoxItemDrop.Text = item.DropLevel.ToString();

                var imageList = new List<Image>();
                // "Try" bracket added for program not to crash when an image of certain index does not exist.
                PairTypeItem pp = null;

                foreach (var pair in item.ImageID)
                {
                    try
                    {
                        pp = pair;
                        var id = ResolveItemImage(pair.Type, pair.Item);
                        imageList.Add(_imagesItemsList[id.ImageIDType][id.ImageIDItem]);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Given image doesnot exist. Please ensure that this image in in sprite folder\r\n" + pp + "\r\n" + e.Message);
                    }
                }

                ListBoxImages.ItemsSource = imageList;

                var enumItemType = ComboBoxItemsType.SelectedIndex;
                var groupType = ((EnumItemType)enumItemType).GetAttributeOfType<TypeToSlot>().GroupType;
                if (groupType == EnumItemGroupType.Armor)
                {
                    var armor = (ArmorType)DataGridItemsAll.SelectedItem;
                    if (armor.Armor == null) return;
                    ButtonItemsArmor.Content = armor.Armor.ToString();
                }
                else if (groupType == EnumItemGroupType.Jewelery) //Amu and Rings
                {
                    var jewelery = (JeweleryType)DataGridItemsAll.SelectedItem;
                }
                else if (groupType == EnumItemGroupType.Shield) //Shields
                {
                    var shield = (ShieldType)DataGridItemsAll.SelectedItem;
                    try
                    {
                        ButtonItemBlockChance.Content = shield.BlockChance.ToString();
                        ButtonItemBlockAmount.Content = shield.BlockAmount.ToString();
                    }
                    catch { }
                }
                else if (groupType == EnumItemGroupType.OffHand)  //Offhand
                {
                    var offhand = (OffHandType)DataGridItemsAll.SelectedItem;
                }
                //13 is Unarmed
                else if (groupType == EnumItemGroupType.Weapon) //Weapons
                {
                    var weapon = (WeaponType)DataGridItemsAll.SelectedItem;
                    try
                    {
                        ButtonItemAccuracy.Content = weapon.Accuracy.ToString();
                        ButtonItemAttackSpeed.Content = weapon.AttackSpeed.ToString();
                        ButonItemMinDmg.Content = weapon.MinDamage.ToString();
                        ButtonItemMaxDmg.Content = weapon.MaxDamage.ToString();
                        ButtonItemArmorPen.Content = weapon.ArmorPenetration.ToString();
                        ButtonItemParryChance.Content = weapon.Deflection.ToString();
                        ComboBoxItemRequiredHands.SelectedIndex = (int)weapon.RequiredHands;
                        TextBoxRange.Text = weapon.Range.ToString();
                    }
                    catch { }
                }
                else if (groupType == EnumItemGroupType.Lore) //Weapons
                {
                    var lore = (LoreType)DataGridItemsAll.SelectedItem;
                }
            }
        }

        private void DataGridItemsAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshItems();
        }

        private void ButtonItemDurability_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                var window = new MinMaxStatWindow(item.Durability);
                CenterWindow(window);
                item.Durability = window.ValueFloat;
                RefreshItems();
            }
        }

        private void TextBoxItemsName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                item.Name = TextBoxItemsName.Text;
            }
        }

        private void TextBoxItemsExtraName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                item.ExtraName = TextBoxItemsExtraName.Text;
            }
        }

        private void TextBoxItemsFlavor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                item.FlavorText = TextBoxItemsFlavor.Text;
            }
        }

        private void ComboBoxItemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                item.Type = (EnumItemType)ComboBoxItemType.SelectedIndex;
            }
        }

        private void ComboBoxItemSkill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                item.Skill = (EnumItemClassSkill)ComboBoxItemSkill.SelectedIndex;
            }
        }

        private void ComboBoxRarity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                item.Rarity = (EnumItemRarity)ComboBoxRarity.SelectedIndex;
            }
        }

        private void TextBoxItemRequired_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                int temp = 0;
                int.TryParse(TextBoxItemRequired.Text, out temp);
                item.RequiredLevel = temp;
                if (isToggledButton)
                {
                    TextBoxItemDrop.Text = TextBoxItemRequired.Text;
                    int.TryParse(TextBoxItemDrop.Text, out temp);
                    item.DropLevel = temp;
                }
            }
        }

        private void TextBoxItemDrop_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                int temp = 0;
                int.TryParse(TextBoxItemDrop.Text, out temp);
                item.DropLevel = temp;
                if (isToggledButton)
                {
                    TextBoxItemRequired.Text = TextBoxItemDrop.Text;
                    int.TryParse(TextBoxItemRequired.Text, out temp);
                    item.RequiredLevel = temp;
                }
            }
        }

        private void ListBoxImages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                var window = new ImageWindow(item.ImageID);
                window.Owner = this;
                window.ShowDialog();
                RefreshItems();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 0 && ComboBoxItemsType.SelectedIndex <= 8) //Armors
                {
                    var item = DataGridItemsAll.SelectedItem as ArmorType;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 0 && ComboBoxItemsType.SelectedIndex <= 8) //Armors
                {
                    var item = DataGridItemsAll.SelectedItem as ArmorType;
                    var window = new MinMaxStatWindow(item.Armor);
                    CenterWindow(window);
                    item.Armor = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButtonItemBlockChance_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex == 11) //Shields
                {
                    var item = DataGridItemsAll.SelectedItem as ShieldType;
                    var window = new MinMaxStatWindow(item.BlockChance);
                    CenterWindow(window);
                    item.BlockChance = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButtonItemBlockAmount_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex == 11) //Shields
                {
                    var item = DataGridItemsAll.SelectedItem as ShieldType;
                    var window = new MinMaxStatWindow(item.BlockAmount);
                    CenterWindow(window);
                    item.BlockAmount = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButtonItemAccuracy_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    var window = new MinMaxStatWindow(item.Accuracy);
                    CenterWindow(window);
                    item.Accuracy = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButtonItemAttackSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    var window = new MinMaxStatWindow(item.AttackSpeed);
                    CenterWindow(window);
                    item.AttackSpeed = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButonItemMinDmg_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    var window = new MinMaxStatWindow(item.MinDamage);
                    CenterWindow(window);
                    item.MinDamage = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButtonItemMaxDmg_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    var window = new MinMaxStatWindow(item.MaxDamage);
                    CenterWindow(window);
                    item.MaxDamage = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButtonItemArmorPen_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    var window = new MinMaxStatWindow(item.ArmorPenetration);
                    CenterWindow(window);
                    item.ArmorPenetration = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ButtonItemParryChance_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    var window = new MinMaxStatWindow(item.Deflection);
                    CenterWindow(window);
                    item.Deflection = window.ValueFloat;
                    RefreshItems();
                }
            }
        }

        private void ComboBoxItemRequiredHands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    item.RequiredHands = (EnumItemHands)ComboBoxItemRequiredHands.SelectedIndex;
                }
            }
        }

        private void ComboBoxItemRange_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    var item = DataGridItemsAll.SelectedItem as WeaponType;
                    item.Range = Convert.ToInt32(TextBoxRange.Text);
                }
            }
        }

        private void ButtonItemRequiredStats_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                var window = new MinMaxStatTypeWindow(item.RequiredStats);
                CenterWindow(window);
                item.RequiredStats = window.ValueFloat;
                RefreshItems();
            }
        }

        private void TextBoxRange_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void TextBoxRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                if (ComboBoxItemsType.SelectedIndex >= 14 && ComboBoxItemsType.SelectedIndex <= 27) //Weapons
                {
                    (DataGridItemsAll.SelectedItem as WeaponType).Range = Convert.ToInt32(TextBoxRange.Text);
                }
            }
        }

        #endregion Items

        #region ItemsList

        private class ItemsCategory
        {
            public string Name { get; set; }
            public int Count { get; set; }
            public int Weight { get; set; }
        }

        private IEnumerable<ItemType> ItemsOfSelectedType;

        private void PopulateItemsList()
        {
            DataGridItemsLists.ItemsSource = GenerationStorage.Instance.ItemsLists;
        }

        private IEnumerable<ItemType> SelectItemsOfType(int enumItemType)
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
                    ItemsOfSelectedType = SelectItemsOfType(DataGridItemsCategoryList.SelectedIndex);

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

        ////Buttons

        #endregion ItemsList

        #region Monsters

        private void PopulateMonster()
        {
            //MonsterItems
            DataGridMonsterAllItems.ItemsSource = GenerationStorage.Instance.Entities;
            ////MonsterItems

            //MonstersTemplates
            DataGridMonstersTemplates.ItemsSource = GenerationStorage.Instance.EntityTemplateList;
            ////MonstersTemplates

            //ItemsList
            DataGridMonstersItemsList.ItemsSource = GenerationStorage.Instance.ItemsLists;
            ////ItemsList
        }

        private void RefreshMonsterAllItems()
        {
            if (DataGridMonsterAllItems.IsFocused == false)
            {
                try
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(DataGridMonsterAllItems.ItemsSource);
                    view.Refresh();
                }
                catch
                {
                    //Ignore
                }
            }
        }

        private void RefreshAllMonstersControls()
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                var entity = (EntityType)DataGridMonsterAllItems.SelectedItem;
                if (entity.HealthPoints == null) return;
                TextBoxMonsterName.Text = entity.Name;
                TextBoxMonsterID.Text = entity.ID.ToString();
                ComboBoxMonsterSex.SelectedIndex = (int)entity.Sex;
                ComboBoxMonsterType.SelectedIndex = (int)entity.Type;


                ButtonMonstersHealth.Content = entity.HealthPoints.ToString();
                ButtonMonstersHealthReg.Content = entity.HealthRegen.ToString();
                ButtonMonstersMana.Content = entity.ManaPoints.ToString();
                ButtonMonstersManaRegen.Content = entity.ManaRegen.ToString();
                ButtonMonstersShield.Content = entity.ShieldPoints.ToString();
                ButtonMonstersShieldRegen.Content = entity.ShieldRegen.ToString();
                ButtonMonstersStamina.Content = entity.StaminaPoints.ToString();
                ButtonMonstersStaminaRegen.Content = entity.StaminaRegen.ToString();

                ButtonMonstersMovment.Content = entity.MovmentSpeed.ToString();
                ButtonMonstersRange.Content = entity.Range.ToString();
                ButtonMonstersAttackSpeed.Content = entity.AttackSpeed.ToString();
                ButtonMonstersAccuracy.Content = entity.Accuracy.ToString();
                ButtonMonstersDefence.Content = entity.Armor.ToString();
                ButtonMonstersMinDmg.Content = entity.MinDamage.ToString();
                ButtonMonstersMaxDmg.Content = entity.MaxDamage.ToString();

                ButtonMonstersArmorPenetration.Content = entity.ArmorPenetration.ToString();
                ButtonMonstersCriticalChance.Content = entity.CriticalChance.ToString();
                ButtonMonstersCriticalDamage.Content = entity.CriticalDamage.ToString();

                ButtonMonstersBlockAmount.Content = entity.BlockAmount.ToString();
                ButtonMonstersBlockChance.Content = entity.BlockChance.ToString();
                ButtonMonstersEvasion.Content = entity.Evasion.ToString();
                ButtonMonstersDeflection.Content = entity.Deflection.ToString();

                DataGridMonstersItems.ItemsSource = null;
                DataGridMonstersItems.ItemsSource = entity.ItemsLists;

                var item = (EntityType)DataGridMonsterAllItems.SelectedItem;
                var imageList = new List<Image>();
                PairTypeItem pp = null;
                foreach (var pair in item.ImageID)
                {
                    try
                    {
                        pp = pair;
                        var id = ResolveMonsterImage(pair.Type, pair.Item);
                        imageList.Add(_imagesMonstersList[id.ImageIDType][id.ImageIDItem]);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(
                            "Given image doesnot exist. Please ensure that this image in in sprite folder\r\n" + pp +
                            "\r\n" + e.Message);
                    }
                }

                ListBoxMonsterImages.ItemsSource = imageList;

                if (DataGridMonstersItemsList.SelectedItem != null &&
                    DataGridMonstersItemsList.SelectedItem.GetType().Name != "NamedObject")
                {
                    ButtonMonsterAdd.IsEnabled = true;
                }
                else
                {
                    ButtonMonsterAdd.IsEnabled = false;
                }
                if (DataGridMonstersItems.SelectedItem != null &&
                    DataGridMonstersItems.SelectedItem.GetType().Name != "NamedObject")
                {
                    ButtonMonstersRemove.IsEnabled = true;
                }
                else
                {
                    ButtonMonstersRemove.IsEnabled = false;
                }
            }
            else
            {
                ButtonMonstersRemove.IsEnabled = false;
                ButtonMonsterAdd.IsEnabled = false;
            }
        }

        private void DataGridMonsterAllItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshAllMonstersControls();
        }

        //Adding new item
        private void DataGridMonsterAllItems_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            var entity = new EntityType
            {
                ID = GenerationStorage.Instance.Entities.Count
            };
            e.NewItem = entity;
        }

        ////Adding new item

        //Edit

        private void TextBoxMonsterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                entity.Name = TextBoxMonsterName.Text;
                RefreshMonsterAllItems();
            }
        }

        private void ComboBoxMonsterSex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                entity.Sex = (EnumSex)ComboBoxMonsterSex.SelectedIndex;
                RefreshMonsterAllItems();
            }
        }

        private void ComboBoxMonsterType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                entity.Type = (EnumEntityType)ComboBoxMonsterType.SelectedIndex;
                RefreshMonsterAllItems();
            }
        }

        private void TextBoxMonsterID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                entity.ID = Convert.ToInt32(TextBoxMonsterID.Text);
                RefreshMonsterAllItems();
            }
        }

        ////Edit

        //Buttons

        private void CenterWindow(Window window)
        {
            window.Owner = this;
            window.ShowDialog();
        }

        private void ButtonMonstersHealth_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.HealthPoints);
                CenterWindow(window);
                entity.HealthPoints = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersHealthReg_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.HealthRegen);
                CenterWindow(window);
                entity.HealthRegen = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersMana_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.ManaPoints);
                CenterWindow(window);
                entity.ManaPoints = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersManaRegen_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.ManaRegen);
                CenterWindow(window);
                entity.ManaRegen = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersShield_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.ShieldPoints);
                CenterWindow(window);
                entity.ShieldPoints = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersShieldRegen_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.ShieldRegen);
                CenterWindow(window);
                entity.ShieldRegen = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersMovment_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.MovmentSpeed);
                CenterWindow(window);
                entity.MovmentSpeed = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }


        private void ButtonMonstersRange_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.Range);
                CenterWindow(window);
                entity.Range = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersAttackSpeed_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.AttackSpeed);
                CenterWindow(window);
                entity.AttackSpeed = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersMinDmg_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.MinDamage);
                CenterWindow(window);
                entity.MinDamage = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersMaxDmg_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.MaxDamage);
                CenterWindow(window);
                entity.MaxDamage = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersDefence_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.Armor);
                CenterWindow(window);
                entity.Armor = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersAccuracy_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.Accuracy);
                CenterWindow(window);
                entity.Accuracy = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersStamina_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.StaminaPoints);
                CenterWindow(window);
                entity.StaminaPoints = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersStaminaRegen_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.StaminaRegen);
                CenterWindow(window);
                entity.StaminaRegen = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersArmorPenetration_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.ArmorPenetration);
                CenterWindow(window);
                entity.ArmorPenetration = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersCriticalChance_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.CriticalChance);
                CenterWindow(window);
                entity.CriticalChance = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersCriticalDamage_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.CriticalDamage);
                CenterWindow(window);
                entity.CriticalDamage = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersBlockAmount_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.BlockAmount);
                CenterWindow(window);
                entity.BlockAmount = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersBlockChance_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.BlockChance);
                CenterWindow(window);
                entity.BlockChance = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersEvasion_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.Evasion);
                CenterWindow(window);
                entity.Evasion = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonstersDeflection_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.Deflection);
                CenterWindow(window);
                entity.Deflection = window.ValueFloat;
                RefreshAllMonstersControls();
            }
        }

        private void ButtonMonsterAdd_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                if (DataGridMonstersItemsList.SelectedItem != null &&
                    DataGridMonstersItemsList.SelectedItem.GetType().Name != "NamedObject")
                {
                    var list = DataGridMonstersItemsList.SelectedItem as ItemsLists;
                    var item = new GenerationWeightLists();
                    item.Name = list.Name;
                    item.Weight = Convert.ToInt32(TextBoxMonsterItemsLists.Text);
                    var entity = DataGridMonsterAllItems.SelectedItem as EntityType;
                    if (entity.ItemsLists.FirstOrDefault(x => x.Name == item.Name) == null)
                    {
                        entity.ItemsLists.Add(item);
                    }
                    DataGridMonstersItems.ItemsSource = null;
                    DataGridMonstersItems.ItemsSource = entity.ItemsLists;
                }
            }
        }

        private void ButtonMonstersRemove_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                if (DataGridMonstersItems.SelectedItem != null &&
                    DataGridMonstersItems.SelectedItem.GetType().Name != "NamedObject")
                {
                    var entity = DataGridMonsterAllItems.SelectedItem as EntityType;
                    var item = DataGridMonstersItems.SelectedItem as GenerationWeightLists;
                    entity.ItemsLists.Remove(entity.ItemsLists.Find(x => x.Name == item.Name));
                    DataGridMonstersItems.ItemsSource = null;
                    DataGridMonstersItems.ItemsSource = entity.ItemsLists;
                }
            }
        }

        private void DataGridMonstersItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                ButtonMonsterAdd.IsEnabled = true;
            }
            else
            {
                ButtonMonsterAdd.IsEnabled = false;
            }
        }

        private void DataGridMonstersItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                ButtonMonstersRemove.IsEnabled = true;
            }
            else
            {
                ButtonMonstersRemove.IsEnabled = false;
            }
        }

        private void TextBoxMonsterItemsLists_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextUtils.IsNumeric(ref e, true);
        }

        private void ListBoxMonsterImages_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedItem != null && DataGridMonsterAllItems.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridMonsterAllItems.SelectedItem as EntityType;
                var window = new ImageMonstersWindow(item.ImageID);
                window.Owner = this;
                window.ShowDialog();
                RefreshAllMonstersControls();
            }
        }

        ////Buttons

        //Templates

        private void RefreshMonstersTemplates()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(DataGridMonstersTemplates.ItemsSource);
            view.Refresh();
        }

        private void ButtonAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            GenerationStorage.Instance.EntityTemplateList.Add(DataGridMonsterAllItems.SelectedItem as EntityType);
            RefreshMonstersTemplates();
        }

        ////Templates

        #endregion Monsters

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

        private void MenuItemOptions_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Options();
            dialog.ShowDialog();
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

        private bool isToggledButton = true;

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            isToggledButton = !isToggledButton;
        }

        private void ButtonOpenSounds_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                var window = new SoundsWindow(item.SoundID);
                window.Owner = this;
                window.ShowDialog();
                RefreshItems();
            }

        }


        ////TextBox

        #endregion MonsterLists

        //

    }
}