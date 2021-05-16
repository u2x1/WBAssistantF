using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WBAssistantF.Module.Wallpaper
{
    internal class WallSwitchEffect
    {
        public static BitmapSource ToBitmapSource(Bitmap btmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(btmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        private readonly WallEffect.MainWindow window;

        public WallSwitchEffect()
        {
            window = new WallEffect.MainWindow
            {
                Left = -9999,
                Top = -9999,
                Width = 0,
                Height = 0
            };
            setBackground();

            IntPtr progman = W32.FindWindow("Progman", null);

            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            W32.SendMessageTimeout(progman,
                                   0x052C,
                                   new IntPtr(0),
                                   IntPtr.Zero,
                                   W32.SendMessageTimeoutFlags.SMTO_NORMAL,
                                   1000,
                                   out IntPtr _);

            IntPtr workerw = IntPtr.Zero;

            // We enumerate all Windows, until we find one, that has the SHELLDLL_DefView 
            // as a child. 
            // If we found that window, we take its next sibling and assign it to workerw.
            W32.EnumWindows(new W32.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = W32.FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = W32.FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               IntPtr.Zero);
                }

                return true;
            }), IntPtr.Zero);

            window.Loaded += new RoutedEventHandler((s, e) =>
            {
                W32.SetParent(new WindowInteropHelper(window).Handle, workerw);
            });

            //// Start the Application Loop for the Form.
            window.Show();
        }

        public void setBackground()
        {
            try
            {
                Bitmap bm;
                using (var fs = new System.IO.FileStream("WBAData\\wall.bmp", System.IO.FileMode.Open))
                {
                    var bmp = new Bitmap(fs);
                    bm = (Bitmap)bmp.Clone();
                }
                window.Background = new ImageBrush(ToBitmapSource(bm)) { Stretch = Stretch.Uniform };
            }
            catch (Exception) { return; }

        }

        public void inEffect(Action action)
        {

            window.Height = SystemParameters.PrimaryScreenHeight + 14;
            window.Width = SystemParameters.PrimaryScreenWidth + 14;
            window.Left = -7;
            window.Top = -7;

            window.InAnimation(action);
        }

        public void outEffect()
        {
            window.OutAnimation();
        }

        public void release()
        {
            window.Close();
        }
    }
}
