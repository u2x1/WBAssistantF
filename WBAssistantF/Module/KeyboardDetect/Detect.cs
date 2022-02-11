using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;
using WBAssistantF.Util;

namespace WBAssistantF.Module.KeyboardDetect
{
    public class KeyboardDetect
    {
        private KeyboardHook kh;
        private Key? pressingKey;
        private readonly List<string> pressingModifiers = new();
        Config _cfg;
        Logger _logger;

        public KeyboardDetect(Config cfg, Logger logger)
        {
            _cfg = cfg;
            _logger = logger;
        }

        public void StartHook()
        {
            kh = new KeyboardHook();
            kh.KeyPress += kh_KeyPress;
            switch (kh.Hook_Start())
            {
                case -1:
                    _logger.LogE("键盘钩子初始化失败");
                    break;
                case 0:
                    _logger.LogC("键盘钩子初始化成功");
                    break;
                case 1:
                    _logger.LogW("检测到已存在的键盘钩子");
                    break;
            }
        }

        private void kh_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Pressing)
            {
                if (e.Modifier != "None" && !pressingModifiers.Contains(e.Modifier))
                    pressingModifiers.Add(e.Modifier);
                if (e.PressingKey != null && pressingKey != e.PressingKey)
                    pressingKey = e.PressingKey;
            }
            else
            {
                if (e.Modifier != "None")
                    pressingModifiers.Remove(e.Modifier);
                if (e.PressingKey != null && e.PressingKey == pressingKey)
                    pressingKey = null;
            }

            if (pressingKey == (Key)_cfg.ShortCut)
            {
                var p = new Process
                {
                    StartInfo = { FileName = "cmd.exe", Arguments = $"/c {_cfg.ShortCutCmd}", CreateNoWindow = true },

                };
                p.Start();
            }
        }
    }
}
