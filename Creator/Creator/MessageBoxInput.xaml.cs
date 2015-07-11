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
    /// Interaction logic for MessageBoxInput.xaml
    /// </summary>
    public partial class MessageBoxInput : Window
    {

        public string InputValue
        {
            get { return TextBoxInput.Text; }
        }

        public MessageBoxInput()
        {
            InitializeComponent();
            TextBoxInput.Focus();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            if (InputValue != "")
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
