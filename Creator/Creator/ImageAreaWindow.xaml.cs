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
using InventoryQuest.Utils;
using System.IO;

namespace Creator
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageAreaWindow : Window
    {
        public string ImageString { get; set; }

        private List<string> list = new List<string>();
        private List<Image> images = new List<Image>();

        public ImageAreaWindow()
        {
            InitializeComponent();
            PopulateImagesGrid();
        }

        public ImageAreaWindow(string imageString)
        {
            InitializeComponent();
            PopulateImagesGrid();
            ImageString = imageString;
            label.Content = ImageString;
        }

        public void PopulateImagesGrid()
        {
            images = new List<Image>();
            list = new List<string>();
            foreach (var item in ResourcesManager.GetAllFiles(@"..\landscapes"))
            {
                if (item.Contains("Icon")) continue;
                list.Add(item);
                var bi = new BitmapImage(new Uri(item, UriKind.Absolute));
                images.Add(new Image() { Source = bi, Stretch = Stretch.Uniform, Width = 144, Height = 201 });
            }
            ListBoxImages.ItemsSource = images;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();

        }

        private void ListBoxImages_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ImageString = Path.GetFileNameWithoutExtension(list[ListBoxImages.SelectedIndex]);
            label.Content = ImageString;
        }
    }
}
