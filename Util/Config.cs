using System;
using System.IO;

namespace WBAssistantF.Util
{
    internal class Config
    {
        private Logger _logger;
        public bool AutoOpenExplorer = true;
        public bool AutoPlayEnAudio;
        public string AutoPlayEnAudioFileName = "words and expressions";
        public string AutoPlayEnAudioUnit = "1";
        public bool AutoPlayFtp;
        public bool EnableDesktopArrange;
        public string[] Extension = {"doc", "docx", "xls", "xlsx", "ppt", "pptx", "pdf", "txt"};
        public bool RefreshWallpaper;
        public bool RejectNewDevice = true;
        public string SavePath = Environment.CurrentDirectory + "\\save";

        public static Config ParseConfig(string path, Logger logger)
        {
            try
            {
                var cfg = File.ReadAllText(path).Split('\n');
                return new Config
                {
                    _logger = logger,
                    SavePath = cfg[0],
                    Extension = cfg[1].Split(','),
                    RefreshWallpaper = bool.Parse(cfg[2]),
                    AutoOpenExplorer = bool.Parse(cfg[3]),
                    AutoPlayFtp = bool.Parse(cfg[4]),
                    AutoPlayEnAudio = bool.Parse(cfg[5]),
                    AutoPlayEnAudioUnit = cfg[6],
                    AutoPlayEnAudioFileName = cfg[7],
                    RejectNewDevice = bool.Parse(cfg[8]),
                    EnableDesktopArrange = bool.Parse(cfg[9])
                };
            }
            catch (Exception e)
            {
                logger.LogE("解析配置文件时出错:\n" + e.Message);
                return new Config
                {
                    _logger = logger
                };
            }
        }

        public void SaveConfig()
        {
            const string path = "WBAData\\config.txt";
            SaveConfig(path);
        }

        private void SaveConfig(string path)
        {
            var content = Utils.Intersperse(new[]
            {
                SavePath,
                Utils.Intersperse(Extension, ','),
                RefreshWallpaper.ToString(),
                AutoOpenExplorer.ToString(),
                AutoPlayFtp.ToString(),
                AutoPlayEnAudio.ToString(),
                AutoPlayEnAudioUnit,
                AutoPlayEnAudioFileName,
                RejectNewDevice.ToString(),
                EnableDesktopArrange.ToString()
            }, '\n');

            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                _logger.LogE("保存配置文件时出错:\n" + e.Message);
            }
        }
    }
}