using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WBAssistantF.Module.USB
{
    internal class DesktopArrange
    {
        private readonly Logger _logger;

        private UsbInfo currentInfo;
        private int insertedCount;
        private readonly FileSystemWatcher watcher;
        //System.Timers.Timer timer = new System.Timers.Timer(2400000);

        System.Timers.Timer timer = new System.Timers.Timer(1400);

        public DesktopArrange(Copier copier, Logger logger)
        {
            _logger = logger;
            copier.UsbChange += Copier_USBChange;
            watcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            watcher.Created += Watcher_Created;
            timer.Elapsed += (s, e) => { tidyDesktop(); };
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        private ImageSource toSource(Icon ico)
        {
            return Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        public Dictionary<string, string> scheculedMove = new Dictionary<string, string>();
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath) && !Directory.Exists(e.FullPath)) return;

            var destFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (insertedCount > 0)
                destFolder += "\\" + currentInfo.Remark;
            else
                destFolder += "\\其他";

            if (destFolder == e.FullPath) return;
            timer.Stop(); timer.Start();    // reset timer.
            scheculedMove.TryAdd(destFolder, e.FullPath);
            
            //var thread = new Thread(() =>
            //{
            //    var oriSource = File.Exists(e.FullPath)
            //        ? Icon.ExtractAssociatedIcon(e.FullPath)
            //        : DefaultIcons.FolderLarge;
            //    var msgBox = new MovingMsgBox(
            //        e.FullPath,
            //        destFolder,
            //        toSource(oriSource),
            //        toSource(DefaultIcons.FolderLarge)
            //    )
            //    {
            //        WindowStartupLocation = WindowStartupLocation.Manual
            //    };
            //    msgBox.Left = Cursor.Position.X - msgBox.Width / 2;
            //    msgBox.Top = Cursor.Position.Y - msgBox.Height * 1.5;
            //    if (msgBox.Left < 0) msgBox.Left = 0;
            //    if (msgBox.Top < 0) msgBox.Top = 0;
            //    msgBox.Show();
            //    msgBox.ShowMovingAnim();
        

            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
        }

        private void Copier_USBChange(bool IsInsert, UsbInfo? info)
        {
            if (IsInsert)
            {
                currentInfo = (UsbInfo)info;
                ++insertedCount;
            }
            else
                --insertedCount;
        }

        private bool IsFileOrDirLocked(bool isFile, string path)
        {
            if (isFile)
            {
                try
                {
                    using var stream = new FileInfo(path).Open(FileMode.Open, FileAccess.Read, FileShare.None);
                    stream.Close();
                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)
                    return true;
                }

                //file is not locked
                return false;
            }

            foreach (var file in Directory.GetFiles(path))
                if (IsFileOrDirLocked(true, file))
                    return true;
            foreach (var dir in Directory.GetDirectories(path))
                if (IsFileOrDirLocked(false, dir))
                    return true;
            return false;
        }

        public void tidyDesktop()
        {
            var thread = new Thread(() =>
            {
                foreach (var (destFolder, fullPath) in scheculedMove)
                {
                    try
                    {
                        var isFile = File.Exists(fullPath);

                        var retry = 1200;
                        while (IsFileOrDirLocked(isFile, fullPath))
                        {
                            Thread.Sleep(500);
                            if (--retry == 0) return;
                        }

                        if (destFolder == fullPath) return;
                        if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

                        string filename = Path.GetFileName(fullPath);
                        if (isFile)
                        {
                            if (File.Exists(fullPath)) File.Delete(destFolder + "\\" + filename);
                            File.Move(fullPath, destFolder + "\\" + filename);
                        }
                        else
                            Directory.Move(fullPath, destFolder + "\\" + filename);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogE("移动文件时出现了错误：\n" + ex.Message);
                    }
                }
                scheculedMove.Clear();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }

    // get it from https://stackoverflow.com/a/59129804
    public static class DefaultIcons
    {
        private const uint SHSIID_FOLDER = 0x3;
        private const uint SHGSI_ICON = 0x100;
        private const uint SHGSI_LARGEICON = 0x0;
        private const uint SHGSI_SMALLICON = 0x1;


        private static Icon folderIcon;

        public static Icon FolderLarge => folderIcon ?? (folderIcon = GetStockIcon(SHSIID_FOLDER, SHGSI_LARGEICON));

        private static Icon GetStockIcon(uint type, uint size)
        {
            var info = new SHSTOCKICONINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            SHGetStockIconInfo(type, SHGSI_ICON | size, ref info);

            var icon = (Icon)Icon.FromHandle(info.hIcon).Clone(); // Get a copy that doesn't use the original handle
            DestroyIcon(info.hIcon); // Clean up native icon to prevent resource leak

            return icon;
        }

        [DllImport("shell32.dll")]
        public static extern int SHGetStockIconInfo(uint siid, uint uFlags, ref SHSTOCKICONINFO psii);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr handle);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct SHSTOCKICONINFO
        {
            public uint cbSize;
            public IntPtr hIcon;
            public int iSysIconIndex;
            public int iIcon;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szPath;
        }
    }
}