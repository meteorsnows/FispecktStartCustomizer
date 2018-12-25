using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Controls;

namespace FispecktStartCustomizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// '
    ///

    public partial class MainWindow
    {
        private Hooks hook = new Hooks();
        IntPtr hwndButton;
        public MainWindow()
        {
            hook.startMenu.Show();
            FunctionsImplementations.Size s = new FunctionsImplementations.Size();
            s.height = 100;
            s.width = 100;
            Thickness t = new Thickness();
            new SettingsMenu().Show();
            hook.startMenu.AddButton(null /* iconpath */ , null /* exe path */, s, new Thickness(0, 0, 0, 0));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IntPtr TaskBar = FunctionsImplementations.FindWindow("Shell_TrayWnd", null); // Connect to taskbar
            if (TaskBar != (IntPtr)0)
            {
                hwndButton = FunctionsImplementations.FindWindowEx(TaskBar, IntPtr.Zero, "Start", null); // Connect to start menu tooltip
                if (hwndButton != (IntPtr)0)
                {
                    FunctionsImplementations.EnableWindow(hwndButton, false);
                    FunctionsImplementations.GetWindowRect(hwndButton, ref Hooks.buttonRect);
                    hook.Hook();
                }
                else
                {
                    MessageBox.Show("Failed to connect to start menu", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Failed to connect to taskbar", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AcrylicWindow_Closing(object sender, CancelEventArgs e)
        {
            hook.UnHook(); // Unhook on window close
            FunctionsImplementations.EnableWindow(hwndButton, true);
        }

        private void CreateButton(object sender, RoutedEventArgs e)
        {
            FunctionsImplementations.Size s = new FunctionsImplementations.Size();
            s.height = 100;
            s.width = 100;
           hook.startMenu.AddButton(null /* iconpath */ , null /* exe path */, s, new Thickness(0, 0, 0, 0));
        }
    }
}
