using System;
using System.Windows.Input;

namespace WBAssistantF.Module.KeyboardDetect
{
    public class KeyEventArgs : EventArgs
    {
        public Key? PressingKey { get; private set; }
        public string Modifier { get; private set; }
        public KeyboardHook.KeyBoardHookStruct kbh { get; private set; }
        public bool Pressing { get; set; }

        private const int WM_KEYDOWN = 0x0100, WM_KEYUP = 0x0101;
        private readonly string[] modifiersKey = { "Control", "Shift", "Alt", "Windows" };

        public KeyEventArgs(int vkCode, int state, string modifier)
        {
            Modifier = modifier;
            PressingKey = KeyInterop.KeyFromVirtualKey(vkCode);
            foreach (var str in modifiersKey)
            {
                if (PressingKey.ToString().Contains(str))
                {
                    PressingKey = null; // remove modifiers from pressingKeys
                    break;
                }
            }
            switch (state)
            {
                case WM_KEYDOWN:
                    Pressing = true;
                    break;
                case WM_KEYUP:
                    Pressing = false;
                    break;
            }
        }
    }
}
