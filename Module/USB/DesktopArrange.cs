using System;
using System.IO;
using System.Threading;
using WPFWindow;
using System.Windows.Forms;
using System.Diagnostics;

namespace WBAssistantF.Module.USB
{
    internal class DesktopArrange
    {
        Logger _logger;
        FileSystemWatcher watcher;

        public DesktopArrange(Copier copier, Logger logger)
        {
            _logger = logger;
            copier.USBChange += Copier_USBChange;
            watcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
            watcher.Created += Watcher_Created;
            Start();
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void Start()
        {
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath) && !Directory.Exists(e.FullPath)) return;
            bool isFile = File.Exists(e.FullPath) ? true : false;

            int retry = 1200;
            while (IsFileOrDirLocked(isFile, e.FullPath))
            {
                Thread.Sleep(500);
                if (--retry == 0) return;
            }

            string destFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            if (hasInserted)
            {
                destFolder += "\\" + currentInfo.Remark;
            }
            else
            {
                destFolder += "\\其他";
            }

            Thread thread = new Thread(() =>
            {
                try
                {
                    MovingMsgBox msgBox = new MovingMsgBox(e.FullPath, destFolder);
                    msgBox.Left = Cursor.Position.X - (msgBox.Width / 2);
                    msgBox.Top = Cursor.Position.Y - (msgBox.Height * 1.5);
                    msgBox.Show();
                    msgBox.Activate();
                    msgBox.ShowMovingAnim();
                    if (destFolder == e.FullPath) { return; }
                    if (!Directory.Exists(destFolder))
                    {
                        Directory.CreateDirectory(destFolder);
                    }
                    if (isFile)
                    {
                        File.Move(e.FullPath, destFolder + "\\" + e.Name);
                    }
                    else
                    {
                        Directory.Move(e.FullPath, destFolder + "\\" + e.Name);
                    }

                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(2000);
                        msgBox.Dispatcher.Invoke(() => { msgBox.Close(); });
                        foreach (Process pList in Process.GetProcesses())
                        {
                            if (pList.MainWindowTitle.Contains(Path.GetFileName(destFolder)))
                            { return; }
                        }
                        OpenFile(destFolder);
                    });
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();

                    System.Windows.Threading.Dispatcher.Run();
                }
                catch (Exception ex)
                {
                    _logger.LogE("整理文件时出现了错误：\n" + ex.Message);
                    // msgBox.ShowErrorAnim();
                }
                //msgBox.ShowDoneAnim();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        private UsbInfo currentInfo;
        /// <summary>
        /// whether there is inserted usb recorded.
        /// </summary>
        private bool hasInserted = false;

        private void Copier_USBChange(bool IsInsert, UsbInfo info)
        {
            if (IsInsert && !hasInserted)
            {
                currentInfo = info;
                hasInserted = true;
            }
            else
            {
                hasInserted = false;
            }
        }

        private bool IsFileOrDirLocked(bool isFile, string path)
        {
            if (isFile)
            {
                try
                {
                    using FileStream stream = new FileInfo(path).Open(FileMode.Open, FileAccess.Read, FileShare.None);
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
            else
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    if (IsFileOrDirLocked(true, file))
                        return true;
                }
                foreach (var dir in Directory.GetDirectories(path))
                {
                    if (IsFileOrDirLocked(false, dir))
                        return true;
                }
                return false;
            }
        }

        private static void OpenFile(string filename)
        {
            Process p = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe", Arguments = $"/c start \"\" \"{filename}\"", CreateNoWindow = true
                }
            };
            p.Start();
        }
    }
}
