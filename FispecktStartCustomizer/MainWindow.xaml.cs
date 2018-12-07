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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;

namespace FispecktStartCustomizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// '
    ///

    public partial class MainWindow
    {
        bool start;
        HookToStart HtS = new HookToStart();
        Thread t = new Thread(new ThreadStart(new HookToStart().Hook));
        public MainWindow()
        {
            t.IsBackground = true;
        }
        


        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            start = !start;
            if (start)
            {
                t.Start();
            }
            else
            {
                this.Close();
            }
           
        }
    }
}
