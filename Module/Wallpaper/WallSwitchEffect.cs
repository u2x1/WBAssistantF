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
        private readonly MainWindow _window;

        public WallSwitchEffect()
        {
            _window = new MainWindow
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

            _window.Loaded += (s, e) => { W32.SetParent(new WindowInteropHelper(_window).Handle, workerw); };

            //// Start the Application Loop for the Form.
            _window.Show();
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
                Bitmap bm;
                using (var fs = new FileStream("WBAData\\wall.bmp", FileMode.Open))
                {
                    var bmp = new Bitmap(fs);
                    bm = (Bitmap) bmp.Clone();
                }

                _window.Background = new ImageBrush(ToBitmapSource(bm)) {Stretch = Stretch.Uniform};
            }
            catch (Exception)
            {
            }
        }

        public void InEffect(Action action)
        {
            _window.Height = SystemParameters.PrimaryScreenHeight + 14;
            _window.Width = SystemParameters.PrimaryScreenWidth + 14;
            _window.Left = -7;
            _window.Top = -7;

            _window.InAnimation(action);
        }

        public void OutEffect()
        {
            _window.OutAnimation();
        }
    }
}