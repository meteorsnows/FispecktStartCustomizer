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
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static LowLevelMouseProc proc = hCallback;
        private static IntPtr hookId = IntPtr.Zero;
        private static Win32Point curPos = new Win32Point();
        private static Rect buttonRect = new Rect();

        /***********************************************************************Mouse hook for start button/***********************************************************************/
        public void Hook()
        {
            hookId = HookMouse(proc);
        }

        private void UnHook()
        {
            UnhookWindowsHookEx(hookId);
        }

        private static IntPtr HookMouse(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr hCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                GetCursorPos(ref curPos);
                Console.WriteLine(curPos.X + " " + buttonRect.Left + " " + buttonRect.Right); // Debug information
                if (curPos.X >= buttonRect.Left && curPos.X <= buttonRect.Right)
                {
                    if (curPos.Y >= buttonRect.Top && curPos.Y <= buttonRect.Bottom)
                    {
                        Console.WriteLine("Clicked"); // Mouse clicked on start menu button
                    }
                }

            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(ref Win32Point p);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };
        /***********************************************************************End of mouse hook***********************************************************************/


        /***********************************************************************Implementing functions for hoooks***********************************************************************/
        private enum MouseMessages { WM_LBUTTONUP = 0x0202 }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int hookId, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        /***********************************************************************End of implementing functions***********************************************************************/


        //Import some functions for start menu tooltip 
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string ClassName, string WindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string windowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public MainWindow()
        {
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IntPtr TaskBar = FindWindow("Shell_TrayWnd", null); // Connect to taskbar
            if (TaskBar != (IntPtr)0)
            {
                IntPtr hwndButton = FindWindowEx(TaskBar, IntPtr.Zero, "Start", null); // Connect to start menu tooltip
                if (hwndButton != (IntPtr)0)
                {
                    GetWindowRect(hwndButton, ref buttonRect);
                    Hook();
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
            UnHook(); // Unhook on window close
        }
    }
}
