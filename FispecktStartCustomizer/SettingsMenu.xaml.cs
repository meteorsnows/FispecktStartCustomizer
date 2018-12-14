using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace FispecktStartCustomizer
{
    /// <summary>
    /// Логика взаимодействия для SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : Window
    {
        RWIni configFile = FunctionsImplementations.GetConfigFile();
        public SettingsMenu()
        {
            InitializeComponent();
        }

        private void AddProgram_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            
            if (ofd.ShowDialog() == true)
            {
                sss.Items.Add(ofd.FileName);
            }
        }

        private void Autorun_Click(object sender, RoutedEventArgs e)
        {
            //TODO: AUTORUN FUNCTION
        }

        private void ResetConfig_Click(object sender, RoutedEventArgs e)
        {
            configFile.DeleteSection("Config");
            MessageBox.Show("Config resetted sucessfully", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Console.WriteLine(sss.SelectedItem);
        }
    }
}
