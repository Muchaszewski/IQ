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
using InventoryQuest.Utils;

namespace Creator
{
    /// <summary>
    /// Interaction logic for SoundsWindow.xaml
    /// </summary>
    public partial class SoundsWindow : Window
    {
        private List<PairTypeItem> _ImagesID = new List<PairTypeItem>();
        public List<PairTypeItem> ImagesID
        {
            get { return _ImagesID; }
            set { _ImagesID = value; }
        }

        public SoundsWindow()
        {
            InitializeComponent();
            PopulateListBoxImageIDs();
        }


        public SoundsWindow(List<PairTypeItem> imagesID)
            : this()
        {
            ImagesID = imagesID;
            PopulateListBoxImageIDs();
        }


        private void ButtonPlaySound_Click(object sender, RoutedEventArgs e)
        {

        }

        public void PopulateListBoxImageIDs()
        {
            ListBoxIDs.ItemsSource = null;
            ListBoxIDs.ItemsSource = ImagesID;
        }

        public void PopulateDataGridSounds()
        {

        }

    }
}
