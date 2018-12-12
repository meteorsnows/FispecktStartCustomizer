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

namespace FispecktStartCustomizer
{
    /// <summary>
    /// Логика взаимодействия для StartMenu.xaml
    /// </summary>
    public partial class StartMenu
    {
        public void AddButton(String imageSrc, String path, FunctionsImplementations.Size size, int x, int y)
        {
            Button b = new Button();
            b.Style = Application.Current.Resources["ButtonRevealStyle"] as Style;
            b.Content = new BitmapImage(new Uri(imageSrc));
            b.AddHandler(Button.ClickEvent, new RoutedEventHandler(b_Click));
            grid.Children.Add(b);

        }

        private void b_Click(object sender, RoutedEventArgs e)
        {
        }

        public StartMenu()
        {
            InitializeComponent();
        }
    }
}
