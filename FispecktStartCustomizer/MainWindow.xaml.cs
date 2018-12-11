using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
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


        /***********************************************************************Mouse hook for start button***********************************************************************/

        //Variables for mousehook

        private static IntPtr mHookId = IntPtr.Zero;

        private static FunctionsImplementations.HookProc mouseProc = mouseCallback;

        private static Win32Point curPos = new Win32Point();
        private static FunctionsImplementations.Rect buttonRect = new FunctionsImplementations.Rect();

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
        private enum MouseMessages { WM_LBUTTONUP = 0x0202 }

        private static IntPtr HookMouse(FunctionsImplementations.HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return FunctionsImplementations.SetWindowsHookEx(WH_MOUSE_LL, proc, FunctionsImplementations.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr mouseCallback(int nCode, IntPtr wParam, FunctionsImplementations.KBDLLHOOKSTRUCT lParam)
        {
            if (nCode >= 0 && (MouseMessages)wParam == MouseMessages.WM_LBUTTONUP)
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
            return FunctionsImplementations.CallNextHookEx(mHookId, nCode, wParam, lParam);
        }

        /***********************************************************************End of mouse hook***********************************************************************/


        /***********************************************************************Keyboard hook***********************************************************************/

        private static IntPtr keyHookId = IntPtr.Zero;

        private const int WH_KEYBOARD_LL = 13; // Low-level keyboard hook number
        private static FunctionsImplementations.HookProc keyboardProc = keyboardCallback;

        private enum KeyMessages { WM_KEYUP = 0x0101, VK_LWIN=0x5B, VK_RWIN=0x5C }

        private static IntPtr HookKeyboard(FunctionsImplementations.HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return FunctionsImplementations.SetWindowsHookEx(WH_KEYBOARD_LL, proc, FunctionsImplementations.GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private static IntPtr keyboardCallback(int nCode, IntPtr wParam, FunctionsImplementations.KBDLLHOOKSTRUCT lParam)
        {
            if ((KeyMessages)lParam.vkCode == KeyMessages.VK_LWIN | (KeyMessages)lParam.vkCode == KeyMessages.VK_RWIN)
            {
                Console.WriteLine("Start pressed");
                return (IntPtr)1;
            }
            return FunctionsImplementations.CallNextHookEx(keyHookId, nCode, wParam, lParam);
        }

        /***********************************************************************End of keyboard hook***********************************************************************/

        IntPtr hwndButton;
        /***********************************************************************Hook control functions***********************************************************************/
        public void Hook()
        {
            mHookId = HookMouse(mouseProc);
            keyHookId = HookKeyboard(keyboardProc);
        }

        private void UnHook()
        {
            FunctionsImplementations.UnhookWindowsHookEx(mHookId);
            FunctionsImplementations.UnhookWindowsHookEx(keyHookId);
        }

        public MainWindow()
        {
            StartMenu startMenu = new StartMenu();
            startMenu.Show();
            FunctionsImplementations.Size s = new FunctionsImplementations.Size();
            s.height = 2;
            s.width = 2;
            startMenu.AddButton("C:\\d.png", "fff", s, 0, 0);
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
                    FunctionsImplementations.GetWindowRect(hwndButton, ref buttonRect);
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
            FunctionsImplementations.EnableWindow(hwndButton, true);
        }
    }
}
