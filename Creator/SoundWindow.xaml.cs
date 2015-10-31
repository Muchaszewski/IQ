using InventoryQuest;
using InventoryQuest.Components.Items;
using InventoryQuest.Components.Statistics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using InventoryQuest.Utils;
using Path = System.IO.Path;

namespace Creator
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class SoundWindow : Window
    {
        private List<PairTypeItem> _ImagesID = new List<PairTypeItem>();

        public List<PairTypeItem> ImagesID
        {
            get { return _ImagesID; }
            set { _ImagesID = value; }
        }

        public SoundWindow()
        {
            InitializeComponent();
            PopulateListBoxTypes();
            PopulateListBoxImageIDs();

        }

        public SoundWindow(List<PairTypeItem> imagesID)
            : this()
        {
            while (imagesID.Count < Enum.GetNames(typeof(EnumItemSoundType)).Length)
            {
                imagesID.Add(null);
            }
            ImagesID = imagesID;
            PopulateListBoxImageIDs();
        }

        public void PopulateListBoxTypes()
        {
            List<string> images = ResourcesManager.GetAllFiles("../", false, false).ToList();
            ListBoxTypes.ItemsSource = images;
        }

        public void PopulateListBoxImageIDs()
        {
            ListBoxIDs.ItemsSource = null;
            var list = new List<string>();
            for (int i = 0; i < ImagesID.Count; i++)
            {
                var item = ImagesID[i];
                if (item != null)
                {
                    list.Add((EnumItemSoundType)i + " " + item.Type + " " + item.Item);
                }
                else
                {
                    list.Add((EnumItemSoundType)i + " null");
                }

            }
            ListBoxIDs.ItemsSource = list;

        }

        public void PopulateImagesGrid()
        {
            List<Label> itemsSource = new List<Label>();
            var names = ResourcesManager.GetAllFiles("../", false, false).ToList();
            var index = ListBoxTypes.SelectedIndex;
            foreach (var item in ResourcesManager.GetAllFiles(System.IO.Path.Combine("../", names[index]), true, false).OrderBy((x) => x))
            {
                itemsSource.Add(new Label() { Content = Path.GetFileName(item) });
            }
            ListBoxImages.ItemsSource = itemsSource;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ListBoxTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateImagesGrid();
        }

        private void ListBoxIDs_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = ImagesID[ListBoxIDs.SelectedIndex];
            if (item == null) return;
            var image = ResourcesNames.ResolveItemsSound(item.Type, item.Item);
            ListBoxTypes.SelectedIndex = image.ImageIDType;
            ListBoxImages.SelectedIndex = image.ImageIDItem;
        }

        private void ListBoxIDs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (ListBoxIDs.SelectedIndex != -1)
                {
                    ImagesID[ListBoxIDs.SelectedIndex] = null;
                    PopulateListBoxImageIDs();
                }
            }
        }

        private void ButtonDragAdd_Click(object sender, RoutedEventArgs e)
        {
            var type = ListBoxTypes.SelectedIndex;
            var image = ListBoxImages.SelectedIndex;
            if (type != -1) {
                var item = new PairTypeItem()
                {
                    Type = ResourcesNames.ItemsSoundsNames[type].Name,
                    Item = ResourcesNames.ItemsSoundsNames[type].List[image]
                };
                ImagesID[(int)EnumItemSoundType.Drag] = item;
                PopulateListBoxImageIDs();
            }
        }

        private void ButtonDropAdd_Click(object sender, RoutedEventArgs e)
        {
            var type = ListBoxTypes.SelectedIndex;
            var image = ListBoxImages.SelectedIndex;
            if (type != -1)
            {
                var item = new PairTypeItem()
                {
                    Type = ResourcesNames.ItemsSoundsNames[type].Name,
                    Item = ResourcesNames.ItemsSoundsNames[type].List[image]
                };
                ImagesID[(int)EnumItemSoundType.Drop] = item;
                PopulateListBoxImageIDs();
            }
        }

        private void ButtonEquipAdd_Click(object sender, RoutedEventArgs e)
        {
            var type = ListBoxTypes.SelectedIndex;
            var image = ListBoxImages.SelectedIndex;
            if (type != -1)
            {
                var item = new PairTypeItem()
                {
                    Type = ResourcesNames.ItemsSoundsNames[type].Name,
                    Item = ResourcesNames.ItemsSoundsNames[type].List[image]
                };
                ImagesID[(int)EnumItemSoundType.Equip] = item;
                PopulateListBoxImageIDs();
            }
        }

        private void ButtonHitAdd_Click(object sender, RoutedEventArgs e)
        {
            var type = ListBoxTypes.SelectedIndex;
            var image = ListBoxImages.SelectedIndex;
            if (type != -1)
            {
                var item = new PairTypeItem()
                {
                    Type = ResourcesNames.ItemsSoundsNames[type].Name,
                    Item = ResourcesNames.ItemsSoundsNames[type].List[image]
                };
                ImagesID[(int)EnumItemSoundType.Hit] = item;
                PopulateListBoxImageIDs();
            }
        }

        private void ButtonParryAdd_Click(object sender, RoutedEventArgs e)
        {
            var type = ListBoxTypes.SelectedIndex;
            var image = ListBoxImages.SelectedIndex;
            if (type != -1)
            {
                var item = new PairTypeItem()
                {
                    Type = ResourcesNames.ItemsSoundsNames[type].Name,
                    Item = ResourcesNames.ItemsSoundsNames[type].List[image]
                };
                ImagesID[(int)EnumItemSoundType.Parry] = item;
                PopulateListBoxImageIDs();
            }
        }
    }
}
