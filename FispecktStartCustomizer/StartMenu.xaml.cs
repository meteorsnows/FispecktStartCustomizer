using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public Boolean editingMode = false;
        public Thickness defaultMargin;
        public void AddButton(String imageSrc, String path, FunctionsImplementations.Size size, Thickness margin)
        {
            Console.WriteLine(defaultMargin);
            Button b = new Button();
            b.Style = Application.Current.Resources["ButtonRevealStyle"] as Style;
            if (imageSrc != null)
            {
                b.Content = new BitmapImage(new Uri(imageSrc));
            }
            else
            {
                MessageBox.Show("No button icon, using default", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            b.Width = size.width;
            b.Height = size.height;
            b.Margin = FunctionsImplementations.AddMargin(margin, defaultMargin);
            b.Tag = path; // Throwing path into button properties for shortcut
            b.AddHandler(Button.ClickEvent, new RoutedEventHandler(b_Click));
            grid.Children.Add(b);

        }

        

        private void b_Click(object sender, RoutedEventArgs e)
        {
            String path = (String)((Button)e.Source).Tag;
            if (path != null)
            {
                try
                {
                    Process p = new Process();
                    p.StartInfo.FileName = path;
                    p.Start();
                } catch (System.ComponentModel.Win32Exception err)
                {
                    MessageBox.Show(err.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("File not specified!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public StartMenu(Thickness defaultMargin)
        {
            this.defaultMargin = defaultMargin;
            InitializeComponent();
        }
    }
}
