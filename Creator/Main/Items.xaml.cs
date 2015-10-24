using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Items.Generation.Types;
using InventoryQuest.Utils;
using Creator.Utils;

namespace Creator.Main
{
    /// <summary>
    /// Interaction logic for Items.xaml
    /// </summary>
    public partial class Items : UserControl
    {
        private List<List<Image>> _imagesItemsList;
        private List<List<string>> _soundsItemsList;

        private Window _parentWindow;

        private bool _isToggledButton = true;

        public Items()
        {
            InitializeComponent();
            _parentWindow = Window.GetWindow(this);

            //Do not execute this part of the code if its in edior
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _imagesItemsList = LoadAllItemsImages();
                _soundsItemsList = LoadAllItemsSounds();

                PopulateItems();
            }
        }

        private void PopulateItems()
        {
            ComboBoxItemsType.ItemsSource = Enum.GetNames(typeof(EnumItemType));
            ComboBoxItemsType.SelectedIndex = 0;
            ComboBoxItemType.ItemsSource = Enum.GetNames(typeof(EnumItemType));
            ComboBoxItemSkill.ItemsSource = Enum.GetNames(typeof(EnumItemClassSkill));
            ComboBoxRarity.ItemsSource = Enum.GetNames(typeof(EnumItemRarity));
            ComboBoxItemRequiredHands.ItemsSource = Enum.GetNames(typeof(EnumItemHands));
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

        private static List<List<string>> LoadAllItemsSounds()
        {
            List<List<string>> sounds = new List<List<string>>();
            List<string> names = ResourcesManager.GetAllFiles("../", false, false).ToList();
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                sounds.Add(new List<string>());
                foreach (var item in ResourcesManager.GetAllFiles(Path.Combine("../", name), true, false))
                {
                    sounds[i].Add(item);
                }
            }
            return sounds;
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
            DataGridItemsAll.ItemsSource = ItemUtils.SelectItemsOfType(ComboBoxItemsType.SelectedIndex);
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
            image.ImageIDType = ResourcesNames.ItemsImageNames.FindIndex(x => x.Name == type);
            image.ImageIDItem = ResourcesNames.ItemsImageNames[image.ImageIDType].List.FindIndex(x => x == item);
            return image;
        }

        private static ImageIDPair ResolveItemsSound(string type, string item)
        {
            var image = new ImageIDPair();
            image.ImageIDType = ResourcesNames.ItemsSoundsNames.FindIndex(x => x.Name == type);
            image.ImageIDItem = ResourcesNames.ItemsSoundsNames[image.ImageIDType].List.FindIndex(x => x == item);
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

                var soundList = new List<string>();
                pp = null;
                for (int index = 0; index < item.SoundID.Count; index++)
                {
                    var pair = item.SoundID[index];
                    if (pair == null)
                    {
                        soundList.Add(Enum.GetNames(typeof(EnumItemSoundType))[index] + " no sound");
                        continue;
                    }
                    try
                    {
                        pp = pair;
                        soundList.Add(Enum.GetNames(typeof(EnumItemSoundType))[index] + " " + pair.Type + " " + pair.Item);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(
                            "Given sound doesnot exist. Please ensure that this sound in in sounds folder\r\n" + pp +
                            "\r\n" + e.Message);
                    }
                }

                ListBoxSounds.ItemsSource = soundList;

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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _isToggledButton = !_isToggledButton;
        }


        private void TextBoxItemRequired_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                int temp = 0;
                int.TryParse(TextBoxItemRequired.Text, out temp);
                item.RequiredLevel = temp;
                if (_isToggledButton)
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
                if (_isToggledButton)
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                    Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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

        private void ListBoxSounds_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridItemsAll.SelectedItem != null && DataGridItemsAll.SelectedItem.GetType().Name != "NamedObject")
            {
                var item = DataGridItemsAll.SelectedItem as ItemType;
                var window = new SoundWindow(item.SoundID);
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
                window.ShowDialog();
                RefreshItems();
            }
        }
    }
}
