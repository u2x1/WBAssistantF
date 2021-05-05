using System;
using System.IO;

namespace WBAssistantF
{
    internal class Config
    {
        private Logger logger;
        public string SavePath = Environment.CurrentDirectory + "\\save";
        public string[] Extension = new string[] { "doc", "docx", "xls", "xlsx", "ppt", "pptx", "pdf", "txt" };
        public bool RefreshWallpaper = false;
        public bool AutoOpenExplorer = true;
        public bool AutoPlay_FTP = false;
        public bool AutoPlay_EnAudio = false;
        public string AutoPlay_EnAudio_Unit = "1";
        public string AutoPlay_EnAudio_FileName = "words and expressions";
        public bool RejectNewDevice = true;

        public static Config ParseConfig(string path, Logger logger)
        {
            try
            {
                string[] cfg = File.ReadAllText(path).Split('\n');
                return new Config
                {
                    logger = logger,
                    SavePath = cfg[0],
                    Extension = cfg[1].Split(','),
                    RefreshWallpaper = bool.Parse(cfg[2]),
                    AutoOpenExplorer = bool.Parse(cfg[3]),
                    AutoPlay_FTP = bool.Parse(cfg[4]),
                    AutoPlay_EnAudio = bool.Parse(cfg[5]),
                    AutoPlay_EnAudio_Unit = cfg[6],
                    AutoPlay_EnAudio_FileName = cfg[7],
                    RejectNewDevice = bool.Parse(cfg[8]),
                };
            }
            catch (Exception e)
            {
                logger.LogE("解析配置文件时出错:\n" + e.Message);
                return new Config
                {
                    logger = logger
                };
            }
        }

        public void SaveConfig()
        {
            const string path = "WBAData\\config.txt";
            SaveConfig(path);
        }

        public void SaveConfig(string path)
        {
            string content = Utils.Intersperse(new string[]
                {
                    SavePath,
                    Utils.Intersperse(Extension, ','),
                    RefreshWallpaper.ToString(),
                    AutoOpenExplorer.ToString(),
                    AutoPlay_FTP.ToString(),
                    AutoPlay_EnAudio.ToString(),
                    AutoPlay_EnAudio_Unit,
                    AutoPlay_EnAudio_FileName,
                    RejectNewDevice.ToString(),
                }, '\n');

            try
            {
                File.WriteAllText(path, content);
            }
            catch (Exception e)
            {
                logger.LogE("保存配置文件时出错:\n" + e.Message);
            }

        }
    }
}
