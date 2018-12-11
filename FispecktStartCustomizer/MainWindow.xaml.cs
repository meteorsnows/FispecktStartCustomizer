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

        private static HookProc mouseProc = mouseCallback;

        private static Win32Point curPos = new Win32Point();
        private static Rect buttonRect = new Rect();

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

        private static IntPtr HookMouse(HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr mouseCallback(int nCode, IntPtr wParam, KBDLLHOOKSTRUCT lParam)
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
            return CallNextHookEx(mHookId, nCode, wParam, lParam);
        }

        /***********************************************************************End of mouse hook***********************************************************************/


        /***********************************************************************Keyboard hook***********************************************************************/

        private static IntPtr keyHookId = IntPtr.Zero;

        private const int WH_KEYBOARD_LL = 13; // Low-level keyboard hook number
        private static HookProc keyboardProc = keyboardCallback;

        private enum KeyMessages { WM_KEYUP = 0x0101, VK_LWIN=0x5B, VK_RWIN=0x5C }

        private static IntPtr HookKeyboard(HookProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }


        private static IntPtr keyboardCallback(int nCode, IntPtr wParam, KBDLLHOOKSTRUCT lParam)
        {
            if ((KeyMessages)lParam.vkCode == KeyMessages.VK_LWIN | (KeyMessages)lParam.vkCode == KeyMessages.VK_RWIN)
            {
                Console.WriteLine("Start pressed");
                return (IntPtr)1;
            }
            return CallNextHookEx(keyHookId, nCode, wParam, lParam);
        }

        /***********************************************************************End of keyboard hook***********************************************************************/


        /***********************************************************************Implementing functions for hoooks***********************************************************************/

        IntPtr hwndButton;

        delegate IntPtr HookProc(int code, IntPtr wParam, KBDLLHOOKSTRUCT lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum KBDLLHOOKSTRUCTFlags : uint
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int hookId, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, KBDLLHOOKSTRUCT lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        /***********************************************************************End of implementing functions***********************************************************************/

        /***********************************************************************Hook control functions***********************************************************************/
        public void Hook()
        {
            mHookId = HookMouse(mouseProc);
            keyHookId = HookKeyboard(keyboardProc);
        }

        private void UnHook()
        {
            UnhookWindowsHookEx(mHookId);
            UnhookWindowsHookEx(keyHookId);
        }

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
                hwndButton = FindWindowEx(TaskBar, IntPtr.Zero, "Start", null); // Connect to start menu tooltip
                if (hwndButton != (IntPtr)0)
                {
                    EnableWindow(hwndButton, false);
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
            EnableWindow(hwndButton, true);
        }
    }
}
