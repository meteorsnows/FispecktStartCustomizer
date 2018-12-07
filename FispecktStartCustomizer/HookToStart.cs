
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace FispecktStartCustomizer
{

    class HookToStart
    {

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);


        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);



        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string ClassName, string WindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string windowName);

        [DllImport("user32.dll")]
        static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

        IntPtr hWndShellTrayWindow = FindWindow("Shell_TrayWnd", null);
        IntPtr hWndB;
        IntPtr StartButtonHWnd;






        static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (idObject != 0 || idChild != 0) return;

        }

        public const uint sig = 0x040C; // Default start button event signal number
        public const uint tSig = 0x419;
        MSG msg = new MSG();
        public async void Hook()
        {
            await loop();
            async Task loop()
            {
                while (true)
                {
                    Console.WriteLine("Running...");
                    StartButtonHWnd = FindWindow("Shell_TrayWnd", null);
                    hWndB = FindWindowEx(StartButtonHWnd, IntPtr.Zero, "TrayNotifyWnd", null);
                    Console.WriteLine(SetWinEventHook(tSig, tSig, hWndB, new WinEventDelegate(WinEventProc), 0, 0, (uint)4).ToString());
                    Console.WriteLine(msg.message);
                    Task.Delay(100);
                }
            }
        }

    }

}
