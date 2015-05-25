using InventoryQuest;
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
    public partial class MinMaxStatWindow : Window
    {
        private MinMaxStat _ValueFloat;

        public MinMaxStat ValueFloat
        {
            get { return _ValueFloat; }
            set { _ValueFloat = value; }
        }

        private MinMaxStat _ValueInt;

        public MinMaxStat ValueInt
        {
            get { return _ValueInt; }
            set { _ValueInt = value; }
        }

        public MinMaxStatWindow()
        {
            InitializeComponent();
        }

        public MinMaxStatWindow(MinMaxStat valueFloat)
            : this()
        {
            ValueFloat = valueFloat;
            TextBoxMax.Text = ValueFloat.Max.ToString();
            TextBoxMaxMax.Text = ValueFloat.MaxMaxLevel.ToString();
            TextBoxMin.Text = ValueFloat.Min.ToString();
            TextBoxMinMax.Text = ValueFloat.MinMaxLevel.ToString();
            LabelType.Content = "Float";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValueInt != null)
            {
                int parsedValue;

                Int32.TryParse(TextBoxMin.Text, out parsedValue);
                ValueInt.Min = parsedValue;

                Int32.TryParse(TextBoxMax.Text, out parsedValue);
                ValueInt.Max = parsedValue;

                Int32.TryParse(TextBoxMinMax.Text, out parsedValue);
                ValueInt.MinMaxLevel = parsedValue;

                Int32.TryParse(TextBoxMaxMax.Text, out parsedValue);
                ValueInt.MaxMaxLevel = parsedValue;

                if (ValueInt.Max < ValueInt.Min || ValueInt.MinMaxLevel > ValueInt.MaxMaxLevel)
                {
                    MessageBox.Show("Min cannot be bigger then Max");
                    return;
                }
            }
            if (ValueFloat != null)
            {
                float parsedValue;

                float.TryParse(TextBoxMin.Text, out parsedValue);
                ValueFloat.Min = parsedValue;

                float.TryParse(TextBoxMax.Text, out parsedValue);
                ValueFloat.Max = parsedValue;

                float.TryParse(TextBoxMinMax.Text, out parsedValue);
                ValueFloat.MinMaxLevel = parsedValue;

                float.TryParse(TextBoxMaxMax.Text, out parsedValue);
                ValueFloat.MaxMaxLevel = parsedValue;

                if (ValueFloat.Max < ValueFloat.Min || ValueFloat.MinMaxLevel > ValueFloat.MaxMaxLevel)
                {
                    MessageBox.Show("Min cannot be bigger then Max");
                    return;
                }
            }
            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBoxMin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //TextUtils.IsNumeric(ref e, ValueInt != null ? true : false);
        }

        private void TextBoxMax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //TextUtils.IsNumeric(ref e, ValueInt != null ? true : false);
        }

        private void TextBoxMinMax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //TextUtils.IsNumeric(ref e, ValueInt != null ? true : false);
        }

        private void TextBoxMaxMax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //TextUtils.IsNumeric(ref e, ValueInt != null ? true : false);
        }

        private void TextBoxMin_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxMin.SelectAll();
        }

        private void TextBoxMax_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxMax.SelectAll();
        }

        private void TextBoxMinMax_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxMinMax.SelectAll();
        }

        private void TextBoxMaxMax_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBoxMaxMax.SelectAll();
        }

    }
}
