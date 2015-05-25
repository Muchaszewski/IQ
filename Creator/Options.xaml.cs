using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Creator.Properties;

namespace Creator
{
    /// <summary>
    ///     Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public Options()
        {
            InitializeComponent();
            TextBoxPath.Text = Settings.Default.UserSavePath;
        }

        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                TextBoxPath.Text = dialog.SelectedPath;
            }
        }

        private void TextBoxPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.Default.UserSavePath = TextBoxPath.Text;
            Settings.Default.Save();
        }
    }
}