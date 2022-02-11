using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Input;

namespace WBAssistantF.Module.KeyboardDetect
{
    public class KeyboardHook
    {
        public event EventHandler<KeyEventArgs> KeyPress;

        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        private static int hHook;

        public const int WH_KEYBOARD_LL = 13;
        // set as LowLevel keyboard hook

        static HookProc KeyBoardHookProcedure;

        [DllImport("user32.dll")]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]

        public static extern bool UnhookWindowsHookEx(int idHook);
        [DllImport("user32.dll")]

        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);
        [DllImport("kernel32.dll")]
        public static extern int GetCurrentThreadId();
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        public int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var modifier = Keyboard.Modifiers.ToString();
                var kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
                KeyPress(this, new KeyEventArgs(kbh.vkCode, wParam, modifier));
            }
            return CallNextHookEx(hHook, nCode, wParam, lParam);
        }

        public int Hook_Start()
        {
            if (hHook == 0)
            {
                KeyBoardHookProcedure = new HookProc(KeyBoardHookProc);
                hHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyBoardHookProcedure, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
                if (hHook == 0) // if still fails then cancel.
                {
                    Hook_Clear();
                    return -1; // failed
                }
                else
                {
                    return 0;  // success
                }
            }
            else
            {
                return 1;      // already hooked
            }
        }

        public int Hook_Clear()
        {
            if (hHook != 0)
            {
                if (UnhookWindowsHookEx(hHook))
                {
                    hHook = 0;
                    return 0;    // success
                }
                else
                    return -1;   // failed
            }
            else
            {
                return 1;        // already unhooked
            }
        }
    }
}
