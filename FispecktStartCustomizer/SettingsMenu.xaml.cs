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
        public SettingsMenu()
        {
            InitializeComponent();

            /*******************************************************************Read config file***************************************************/

            String configFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\StartCustomizer\\Config.ini";
            Console.WriteLine(configFilePath);
            var configFile = new RWIni(configFilePath);
            configFile.Write("TestKey", "TestValue", "TestSection");
        }

        private void AddProgram_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Autorun_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
