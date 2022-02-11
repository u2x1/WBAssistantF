using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WallEffect;
using WPFWindow;

namespace WBAssistantF.Module.Wallpaper
{
    internal class WallSwitchEffect
    {
        public readonly MainWindow window;

        public WallSwitchEffect()
        {
            window = new MainWindow
            {
                Left = -9999,
                Top = -9999,
                Width = 0,
                Height = 0
            };
            SetBackground();

            var progman = W32.FindWindow("Progman", null);

            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            W32.SendMessageTimeout(progman,
                0x052C,
                new IntPtr(0),
                IntPtr.Zero,
                W32.SendMessageTimeoutFlags.SMTO_NORMAL,
                1000,
                out var _);

            var workerw = IntPtr.Zero;

            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
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

            window.Loaded += (s, e) => { W32.SetParent(new WindowInteropHelper(window).Handle, workerw); };

            //// Start the Application Loop for the Form.
            window.Show();
        }

        private static BitmapSource ToBitmapSource(Bitmap btmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(btmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        public void SetBackground()
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

                window.Background = new ImageBrush(ToBitmapSource(bm)) { Stretch = Stretch.UniformToFill };
            }
            catch (Exception)
            {
            }
        }

        public void InEffect(Action action)
        {
            window.Height = SystemParameters.PrimaryScreenHeight;
            window.Width = SystemParameters.PrimaryScreenWidth;
            window.Left = 0;
            window.Top = 0;

            window.InAnimation(action);
        }

        public void OutEffect()
        {
            window.OutAnimation();
        }
    }
}