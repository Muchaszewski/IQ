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
            ComboBoxSoundType.ItemsSource = Enum.GetNames(typeof(EnumItemSoundType));
            ComboBoxSoundType.SelectedIndex = 0;
        }

        public SoundWindow(List<PairTypeItem> imagesID)
            : this()
        {
            while (imagesID.Count < ComboBoxSoundType.Items.Count)
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
            if (ComboBoxSoundType.SelectedIndex != -1)
            {
                ListBoxIDs.ItemsSource = new List<PairTypeItem> { ImagesID[ComboBoxSoundType.SelectedIndex] };
            }
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

        private void ListBoxImages_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var type = ListBoxTypes.SelectedIndex;
            var image = ListBoxImages.SelectedIndex;
            var item = new PairTypeItem()
            {
                Type = ResourcesNames.ItemsSoundsNames[type].Name,
                Item = ResourcesNames.ItemsSoundsNames[type].List[image]
            };
            ImagesID[ComboBoxSoundType.SelectedIndex] = item;
            PopulateListBoxImageIDs();
        }

        private void ListBoxIDs_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (PairTypeItem)ListBoxIDs.SelectedItem;
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
                    ImagesID.RemoveAt(ComboBoxSoundType.SelectedIndex);
                    PopulateListBoxImageIDs();
                }
            }
        }
    }
}
