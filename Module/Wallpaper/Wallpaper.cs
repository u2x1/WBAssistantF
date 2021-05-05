using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace WBAssistantF.Module.Wallpaper
{
    internal class WallpaperMain
    {
        private readonly Logger logger;
        public WallpaperMain(Logger lgr)
        {
            logger = lgr;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        public void softPickWall()
        {
            if (DateTime.Now.Hour >= 10)
            {
                logger.LogW("已过早上十点，将不会切换新壁纸");
                return;
            }
            pickWall();
        }

        public void pickWall()
        {
            if (!Directory.Exists("walls"))
                return;
            if (!File.Exists("WBAData\\usedWalls.txt"))
                File.WriteAllText("WBAData\\usedWalls.txt", "");

            string[] usedWalls = File.ReadAllText("WBAData\\usedWalls.txt").Split('|');
            List<string> paddingWalls = getUnusedWalls("walls", usedWalls);
            if (paddingWalls.Count == 0)
            {
                File.WriteAllText("WBAData\\usedWalls.txt", "");
                logger.LogW($"所有壁纸已经用完，开始新一次壁纸轮换");
                return;
            }

            logger.LogI($"已用{usedWalls.Length}张壁纸，还剩下{paddingWalls.Count}张壁纸");

            int pickedIndex = new Random().Next(0, paddingWalls.Count - 1);
            string wallPath = paddingWalls[pickedIndex];

            logger.LogI($"已选择壁纸：{wallPath}");
            Thread effectThread = new Thread(() =>
            {
                WallSwitchEffect switchEffect = new WallSwitchEffect();

                switchEffect.inEffect(() =>
                {
                    SetImage(wallPath);
                    switchEffect.setBackground();
                    switchEffect.outEffect();
                });

                System.Windows.Threading.Dispatcher.Run();

            });
            effectThread.SetApartmentState(ApartmentState.STA);
            effectThread.Start();

            File.AppendAllText("WBAData\\usedWalls.txt", wallPath + "|");
        }

        private static List<string> getUnusedWalls(string path, string[] usedWalls)
        {
            List<string> files = new List<string>(Directory.GetFiles(path));
            files = new List<string>(files.Where(path => !usedWalls.Contains(path)));
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                files.AddRange(getUnusedWalls(dir, usedWalls));
            }
            return files;
        }

        private static void SetImage(string filename)
        {
            string bmpAbsPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\WBAData\\wall.bmp";
            new Bitmap(filename).Save(bmpAbsPath);
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, bmpAbsPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
