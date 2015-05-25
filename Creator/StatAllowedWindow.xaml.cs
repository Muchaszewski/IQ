using InventoryQuest.Components.Statistics;
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

namespace Creator
{
    /// <summary>
    /// Interaction logic for StatAllowedWindow.xaml
    /// </summary>
    public partial class StatAllowedWindow : Window
    {
        List<EnumTypeStat> _TypeStats = new List<EnumTypeStat>();

        public List<EnumTypeStat> TypeStats
        {
            get { return _TypeStats; }
            set { _TypeStats = value; }
        }

        public StatAllowedWindow()
        {
            InitializeComponent();
            PopulateDataGrid();

        }

        public StatAllowedWindow(List<EnumTypeStat> typeStats)
            : this()
        {
            TypeStats = typeStats;
            PopulateDataGrid();
        }

        public void PopulateDataGrid()
        {
            DataGridStats.ItemsSource = null;
            DataGridStats.ItemsSource = TypeStats;
            BaseStatsAllowed.ItemsSource = Enum.GetNames(typeof(EnumTypeStat));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            TypeStats.Add(EnumTypeStat.Strength);
            PopulateDataGrid();
        }
    }
}
