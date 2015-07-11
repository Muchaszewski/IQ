using InventoryQuest;
using InventoryQuest.Components.Statistics;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for MinMaxStatWindow.xaml
    /// </summary>
    public partial class MinMaxStatTypeWindow : Window
    {
        private List<MinMaxStatType> _ValueFloat;

        public List<MinMaxStatType> ValueFloat
        {
            get { return _ValueFloat; }
            set { _ValueFloat = value; }
        }

        private List<MinMaxStatType> _ValueInt;

        public List<MinMaxStatType> ValueInt
        {
            get { return _ValueInt; }
            set { _ValueInt = value; }
        }

        public MinMaxStatTypeWindow()
        {
            InitializeComponent();
            PopulateComboBox();
            Clear();
        }

        public void PopulateComboBox()
        {
            ComboBoxType.ItemsSource = Enum.GetNames(typeof(EnumTypeStat));
        }

        public void PopulateDataGrid()
        {
            DataGrid.ItemsSource = null;
            if (ValueInt != null)
            {
                DataGrid.ItemsSource = ValueInt;
            }
            else
            {
                DataGrid.ItemsSource = ValueFloat;
            }
        }

        public MinMaxStatTypeWindow(List<MinMaxStatType> valueFloat)
            : this()
        {
            ValueFloat = valueFloat;
            LabelType.Content = "Float";
            PopulateDataGrid();
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

        private void IsNumeric(ref TextCompositionEventArgs e)
        {
            if (ValueInt != null)
            {
                try
                {
                    Convert.ToInt32(e.Text);
                }
                catch
                {
                    e.Handled = true;
                }
            }
            if (ValueFloat != null)
            {
                if (!char.IsDigit(e.Text, e.Text.Length - 1) && e.Text[e.Text.Length - 1] != ',')
                {
                    e.Handled = true;
                }
            }
        }

        private void TextBoxMin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IsNumeric(ref e);
        }

        private void TextBoxMax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IsNumeric(ref e);
        }

        private void TextBoxMinMax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IsNumeric(ref e);
        }

        private void TextBoxMaxMax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            IsNumeric(ref e);
        }

        private void ComboBoxType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = DataGrid.SelectedIndex;
            if (index >= 0)
            {
                if (ValueInt != null && index < ValueInt.Count)
                {
                    TextBoxMax.Text = ValueInt[index].Max.ToString();
                    TextBoxMaxMax.Text = ValueInt[index].MaxMaxLevel.ToString();
                    TextBoxMin.Text = ValueInt[index].Min.ToString();
                    TextBoxMinMax.Text = ValueInt[index].MinMaxLevel.ToString();
                    ComboBoxType.SelectedIndex = (int)ValueInt[index].StatType;
                }
                if (ValueFloat != null && index < ValueFloat.Count)
                {
                    TextBoxMax.Text = ValueFloat[index].Max.ToString();
                    TextBoxMaxMax.Text = ValueFloat[index].MaxMaxLevel.ToString();
                    TextBoxMin.Text = ValueFloat[index].Min.ToString();
                    TextBoxMinMax.Text = ValueFloat[index].MinMaxLevel.ToString();
                    ComboBoxType.SelectedIndex = (int)ValueFloat[index].StatType;
                }
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxType.SelectedIndex == -1)
            {
                MessageBox.Show("Stat Type cannot be empty");
                return;
            }
            if (ValueInt != null)
            {
                var value = new MinMaxStatType();
                value.Max = Int32.Parse(TextBoxMax.Text);
                value.MaxMaxLevel = Int32.Parse(TextBoxMaxMax.Text);
                value.Min = Int32.Parse(TextBoxMin.Text);
                value.MinMaxLevel = Int32.Parse(TextBoxMinMax.Text);
                value.StatType = (EnumTypeStat)ComboBoxType.SelectedIndex;
                ValueInt.Add(value);
                Clear();
            }
            if (ValueFloat != null)
            {
                var value = new MinMaxStatType();
                value.Max = float.Parse(TextBoxMax.Text);
                value.MaxMaxLevel = float.Parse(TextBoxMaxMax.Text);
                value.Min = float.Parse(TextBoxMin.Text);
                value.MinMaxLevel = float.Parse(TextBoxMinMax.Text);
                value.StatType = (EnumTypeStat)ComboBoxType.SelectedIndex;
                ValueFloat.Add(value);
                Clear();
            }
            PopulateDataGrid();
        }
        private void Clear()
        {
            TextBoxMax.Text = "0";
            TextBoxMaxMax.Text = "0";
            TextBoxMin.Text = "0";
            TextBoxMinMax.Text = "0";
            ComboBoxType.SelectedIndex = -1;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

    }
}
