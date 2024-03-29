﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WBAssistantF.Module.USB;
using WPFWindow;

namespace WBAssistantF.Module.Wallpaper
{
    internal class WallpaperMain
    {
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;
        private readonly Copier _copier;
        private readonly Logger _logger;
        private bool _blackedScreen;


        private int _insertedCount;
        private bool _notProceedingScreen = true;


        public WallpaperMain(Logger lgr, Copier copier)
        {
            _logger = lgr;
            _copier = copier;
            _copier.UsbChangeWithoutInfo += _copier_USBChangeWithoutInfo;
        }

        private void _copier_USBChangeWithoutInfo(bool isInsert)
        {
            if (isInsert)
            {
                ++_insertedCount;
                if (!_blackedScreen || !_notProceedingScreen) BlackIn();
            }
            else
            {
                --_insertedCount;
                if (_insertedCount < 0) _insertedCount = 0;
                if (_insertedCount == 0 && _blackedScreen) BlackOut();
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        public void PickWallIfTimePermit()
        {
            if (DateTime.Now.Hour is < 14 or > 15)
            {
                _logger.LogW("只应在 14:00~15:00 期间切换新壁纸");
                return;
            }

            PickWall();
        }

        public void PickWall()
        {
            if (!Directory.Exists("WBAData\\walls"))
            {
                _logger.LogE("未找到壁纸文件夹");
                return;
            }
            if (!File.Exists("WBAData\\usedWalls.txt"))
                File.WriteAllText("WBAData\\usedWalls.txt", "");

            var usedWalls = File.ReadAllText("WBAData\\usedWalls.txt").Split('|');
            List<string> paddingWalls = GetUnusedWalls("WBAData\\walls", usedWalls);
            if (paddingWalls.Count == 0)
            {
                File.WriteAllText("WBAData\\usedWalls.txt", "");
                _logger.LogW("所有壁纸已经用完，开始新一次壁纸轮换");
                paddingWalls = GetUnusedWalls("WBAData\\walls", Array.Empty<string>());
            }

            _logger.LogI($"已用{usedWalls.Length}张壁纸，还剩下{paddingWalls.Count}张壁纸");

            var pickedIndex = new Random().Next(0, paddingWalls.Count - 1);
            var wallPath = paddingWalls[pickedIndex];

            _logger.LogI($"选择了新壁纸：{wallPath}");
            var effectThread = new Thread(() =>
            {
                var switchEffect = new WallSwitchEffect();

                switchEffect.InEffect(() =>
                {
                    SetImage(wallPath);
                    switchEffect.SetBackground();
                    switchEffect.OutEffect();
                });

                Dispatcher.Run();
            });
            effectThread.SetApartmentState(ApartmentState.STA);
            effectThread.Start();

            File.AppendAllText("WBAData\\usedWalls.txt", wallPath + "|");
        }

        private static List<string> GetUnusedWalls(string path, string[] usedWalls)
        {
            var files = new List<string>(Directory.GetFiles(path));
            files = new List<string>(files.Where(path => !usedWalls.Contains(path)));
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs) files.AddRange(GetUnusedWalls(dir, usedWalls));
            return files;
        }

        private static void SetImage(string filename)
        {
            var bmpAbsPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName) +
                             "\\WBAData\\wall.bmp";
            new Bitmap(filename).Save(bmpAbsPath);
            _ = SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, bmpAbsPath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }

        private static BitmapSource ToBitmapSource(Bitmap btmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(btmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        private static void SetBackground(MainWindow mw)
        {
            try
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                              + "\\Microsoft\\Windows\\Themes\\";
                var path = string.Empty;
                foreach (var p in Directory.GetFiles(folder))
                {
                    if (p.Contains("TranscodedWallpaper") && File.Exists(p))
                    {
                        path = p;
                    }
                }
                if (path == string.Empty)
                {
                    return;
                }
                Bitmap bm;
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    var bmp = new Bitmap(fs);
                    bm = (Bitmap)bmp.Clone();
                }
                mw.Background = new ImageBrush(ToBitmapSource(bm)) { Stretch = Stretch.UniformToFill };
            }
            catch (Exception)
            {
            }
        }

        private Thread _thread;
        private void BlackIn()
        {
            _thread = new Thread(startWindow);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
        }

        private MainWindow _window;
        private void startWindow()
        {
            while (_blackedScreen) Thread.Sleep(1000);

            _window = new MainWindow
            {
                Left = -9999,
                Top = -9999,
                Width = 0,
                Height = 0
            };
            SetBackground(_window);
            var progman = W32.FindWindow("Progman", null);
            W32.SendMessageTimeout(progman,
                0x052C,
                new IntPtr(0),
                IntPtr.Zero,
                W32.SendMessageTimeoutFlags.SMTO_NORMAL,
                1000,
                out IntPtr _);

            var workerw = IntPtr.Zero;
            W32.EnumWindows((tophandle, topparamhandle) =>
            {
                var p = W32.FindWindowEx(tophandle,
                    IntPtr.Zero,
                    "SHELLDLL_DefView",
                    IntPtr.Zero);

                if (p != IntPtr.Zero)
                    // Gets the WorkerW Window after the current one.
                    workerw = W32.FindWindowEx(IntPtr.Zero,
                        tophandle,
                        "WorkerW",
                        IntPtr.Zero);

                return true;
            }, IntPtr.Zero);

            _window.Loaded += (s, e) => { W32.SetParent(new WindowInteropHelper(_window).Handle, workerw); };

            //// Start the Application Loop for the Form.
            _window.Show();
            _window.BlackOut();
            _window.Height = SystemParameters.PrimaryScreenHeight;
            _window.Width = SystemParameters.PrimaryScreenWidth;
            _window.Left = 0;
            _window.Top = 0;
            _blackedScreen = true;
            Dispatcher.Run();
        }

        private void BlackOut()
        {
            _notProceedingScreen = false;
            _window.Dispatcher.Invoke(() =>
            {
                _window.OutAnim(() =>
                {
                    _window.Dispatcher.InvokeShutdown();
                    GC.Collect();
                    _blackedScreen = false;
                    _notProceedingScreen = true;
                });
            });
        }
    }
}