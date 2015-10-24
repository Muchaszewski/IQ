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
    public partial class ImageWindow : Window
    {
        private List<PairTypeItem> _ImagesID = new List<PairTypeItem>();

        public List<PairTypeItem> ImagesID
        {
            get { return _ImagesID; }
            set { _ImagesID = value; }
        }

        public ImageWindow()
        {
            InitializeComponent();
            PopulateListBoxTypes();
            PopulateListBoxImageIDs();
        }

        public ImageWindow(List<PairTypeItem> imagesID)
            : this()
        {
            ImagesID = imagesID;
            PopulateListBoxImageIDs();
        }

        public void PopulateListBoxTypes()
        {
            List<string> images = ResourcesManager.GetAllFiles("", false).ToList();
            images.RemoveAt(images.Count - 1); //remove weapon
            images.AddRange(ResourcesManager.GetAllFiles("weapon", false));
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
            var names = ResourcesManager.GetAllFiles("", false).ToList();
            names.RemoveAt(names.Count - 1);
            var index = ListBoxTypes.SelectedIndex;
            if (index >= names.Count)
            {
                var weaponNames = ResourcesManager.GetAllFiles("weapon", false);
                foreach (var item in ResourcesManager.GetAllFiles(@"weapon/" + weaponNames[index - names.Count]))
                {
                    var bi = new BitmapImage(new Uri(item, UriKind.Absolute));
                    images.Add(new Image() { Source = bi, Stretch = Stretch.Fill, Width = 64, Height = 118 });
                }
                ListBoxImages.ItemsSource = images;
            }
            else if (index >= 0)
            {
                foreach (var item in ResourcesManager.GetAllFiles(names[index]))
                {
                    var bi = new BitmapImage(new Uri(item, UriKind.Absolute));
                    images.Add(new Image() { Source = bi, Stretch = Stretch.Fill, Width = 64, Height = 118 });
                }
                ListBoxImages.ItemsSource = images;
            }
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
                Type = ResourcesNames.ItemsImageNames[type].Name,
                Item = ResourcesNames.ItemsImageNames[type].List[image]
            };
            if (
                ImagesID.FirstOrDefault(
                    x => x.Item == item.Item && x.Type == item.Type) == null)
            {
                ImagesID.Add(item);
            }
            ImagesID.Sort(
                delegate(PairTypeItem p1, PairTypeItem p2)
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
            var image = ResourcesNames.ResolveItemsImage(item.Type, item.Item);
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
