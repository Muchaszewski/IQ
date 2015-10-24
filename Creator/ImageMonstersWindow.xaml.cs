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

namespace Creator
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageMonstersWindow : Window
    {
        private List<PairTypeItem> _ImagesID = new List<PairTypeItem>();

        public List<PairTypeItem> ImagesID
        {
            get { return _ImagesID; }
            set { _ImagesID = value; }
        }

        public ImageMonstersWindow()
        {
            InitializeComponent();
            PopulateListBoxTypes();
            PopulateListBoxImageIDs();
        }

        public ImageMonstersWindow(List<PairTypeItem> imagesID)
            : this()
        {
            ImagesID = imagesID;
            PopulateListBoxImageIDs();
        }

        public void PopulateListBoxTypes()
        {
            List<string> images = ResourcesManager.GetAllFiles("../portraits", false).ToList();
            ListBoxTypes.ItemsSource = images;
        }

        public void PopulateListBoxImageIDs()
        {
            ListBoxIDs.ItemsSource = null;
            ListBoxIDs.ItemsSource = ImagesID;
        }

        public void PopulateImagesGrid()
        {
            List<Image> images = new List<Image>();
            var names = ResourcesManager.GetAllFiles("../portraits", false).ToList();
            var index = ListBoxTypes.SelectedIndex;
            foreach (var item in ResourcesManager.GetAllFiles(System.IO.Path.Combine("../portraits", names[index])))
            {
                var bi = new BitmapImage(new Uri(item, UriKind.Absolute));
                images.Add(new Image() { Source = bi, Stretch = Stretch.Fill, Width = 113, Height = 113 });
            }
            ListBoxImages.ItemsSource = images;
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
                Type = ResourcesNames.MonstersImageNames[type].Name,
                Item = ResourcesNames.MonstersImageNames[type].List[image]
            };
            if (
                ImagesID.FirstOrDefault(
                    x => x.Item == item.Item && x.Type == item.Type) == null)
            {
                ImagesID.Add(item);
            }
            ImagesID.Sort(
                delegate (PairTypeItem p1, PairTypeItem p2)
                {
                    int toReturn = p1.Type.CompareTo(p2.Type);
                    if (toReturn == 0) toReturn = p1.Item.CompareTo(p2.Item);
                    return toReturn;
                });
            PopulateListBoxImageIDs();
        }

        private void ListBoxIDs_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (PairTypeItem)ListBoxIDs.SelectedItem;
            var image = ResourcesNames.ResolveMonstersImage(item.Type, item.Item);
            ListBoxTypes.SelectedIndex = image.ImageIDType;
            ListBoxImages.SelectedIndex = image.ImageIDItem;
        }

        private void ListBoxIDs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (ListBoxIDs.SelectedIndex != -1)
                {
                    ImagesID.RemoveAt(ListBoxIDs.SelectedIndex);
                    PopulateListBoxImageIDs();
                }
            }
        }
    }
}
