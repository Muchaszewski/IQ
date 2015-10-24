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
using Creator.Utils;
using InventoryQuest;
using InventoryQuest.Components;
using InventoryQuest.Components.Entities;
using InventoryQuest.Components.Entities.Generation.Types;
using InventoryQuest.Utils;

namespace Creator.Main
{
    /// <summary>
    /// Interaction logic for Monsters.xaml
    /// </summary>
    public partial class Monsters : UserControl
    {
        private List<List<Image>> _imagesMonstersList;

        private Window _parentWindow;

        public Monsters()
        {
            InitializeComponent();
            _parentWindow = Window.GetWindow(this);

            //Do not execute this part of the code if its in edior
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                _imagesMonstersList = LoadAllMonstersImages();

                PopulateMonster();
            }

            DataGridMonsterAllItems.SelectedIndex = 0;
        }

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

        private static ImageIDPair ResolveMonsterImage(string type, string item)
        {
            var image = new ImageIDPair();
            image.ImageIDType = ResourcesNames.MonstersImageNames.FindIndex(x => x.Name == type);
            image.ImageIDItem = ResourcesNames.MonstersImageNames[image.ImageIDType].List.FindIndex(x => x == item);
            return image;
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

        private void ButtonMonstersHealth_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMonsterAllItems.SelectedIndex != -1 &&
                DataGridMonsterAllItems.SelectedIndex < GenerationStorage.Instance.Entities.Count)
            {
                EntityType entity =
                    GenerationStorage.Instance.Entities[(DataGridMonsterAllItems.SelectedItem as EntityType).ID];
                var window = new MinMaxStatWindow(entity.HealthPoints);
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                Utils.GenericUtils.CenterWindow(_parentWindow, window);
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
                GenericUtils.CenterWindow(_parentWindow, window);
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
    }
}
