using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace WBAssistantF.Module.USB
{
    internal class Copier
    {
        private readonly Logger _logger;
        private readonly Config _cfg;
        private readonly List<UsbInfo> _infos;

        public Copier(Logger lgr, ref Config config, ref List<UsbInfo> info)
        {
            _logger = lgr;
            _cfg = config;
            _infos = info;
        }

        public void StartCopierListen()
        {
            _logger.LogC("USB监控已启动");
            var watcher = new ManagementEventWatcher();
            var query = new WqlEventQuery(
                "SELECT * FROM __InstanceOperationEvent WITHIN 1 WHERE TargetInstance ISA 'Win32_DiskDrive' AND TargetInstance.InterfaceType = 'USB'");
            watcher.EventArrived += Watcher_EventArrived;
            watcher.Query = query;
            watcher.Start();
            while (true)
                watcher.WaitForNextEvent();
        }
        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (!(e.NewEvent.Properties["TargetInstance"].Value is ManagementBaseObject mbo)) return;
            var pnpDeviceId = mbo.Properties["PNPDeviceID"].Value.ToString();
            var deviceId = mbo.Properties["DeviceID"].Value.ToString();
            string driveLetter = null;

            try
            {
                foreach (ManagementBaseObject partition in new ManagementObjectSearcher(
                    "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + deviceId
                                                                 + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                {
                    foreach (ManagementBaseObject disk in new ManagementObjectSearcher(
                        "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='"
                        + partition["DeviceID"]
                        + "'} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
                    {
                        driveLetter = disk["Name"].ToString();
                        break;
                    }
                    break;
                }
            }
            catch (Exception) { return; }

            if (driveLetter == null)
                return;

            PrepareDrive(driveLetter, pnpDeviceId);
        }

        private void CopyFiles(int index, string driveLetter)
        {
            UsbInfo info = _infos[index];

            if (!Directory.Exists(info.SavePath))
            {
                _logger.LogI("正在创建新文件夹：" + info.SavePath);
                Directory.CreateDirectory(info.SavePath + "\\diskInfos");
            }

            Tuple<List<string>, Dictionary<string, int>> allFiles = GetSpecfiedFiles(driveLetter, _cfg.Extension);
            if (allFiles == null)
            {
                _logger.WriteE("发生了未知错误");
                return;
            }


            List<string> files = allFiles.Item1;
            Dictionary<string, int> filesSkiped = allFiles.Item2;


            Dictionary<string, int> filesToCopy = new Dictionary<string, int>();
            foreach (string filename in files)
            {
                string ext = filename.Split('.')[^1].ToLower();
                if (filesToCopy.ContainsKey(ext))
                    ++filesToCopy[ext];
                else
                    filesToCopy[ext] = 1;
            }

            int errorCnt = 0, copyCnt = 0, existCnt = 0;

            foreach (string file in files)
            {
                try
                {
                    string dest = info.SavePath + file[3..];
                    string destDir = Path.GetDirectoryName(dest);
                    if (!Directory.Exists(destDir)) Directory.CreateDirectory(destDir);
                    FileInfo fi = new FileInfo(dest);
                    if (!fi.Exists || fi.LastWriteTime < new FileInfo(file).LastWriteTime)
                    {
                        File.Copy(file, dest, true);
                        ++copyCnt;
                    }
                    else
                        ++existCnt;
                }
                catch (DirectoryNotFoundException) { ++errorCnt; }
                catch (Exception err)
                {
                    _logger.WriteE("错误 " + err.GetType().Name + " 发生在文件 " + file + "上");
                    ++errorCnt;
                }
            }



            _logger.LogI($"复制了 {copyCnt} 个文件，{existCnt} 个文件已存在无需覆盖，发生了 {errorCnt} 个错误");


            _logger.LogI("写入复制文件信息并转储文件树");
            File.WriteAllText(info.SavePath + "\\diskInfos\\CopiedInfo.txt",
                "Copied files:\r\n" + new string('=', 40) + "\r\n" + GetFileCount(filesToCopy) + "\r\n\r\n"
                + "Skiped files:\r\n" + new string('=', 40) + "\r\n" + GetFileCount(filesSkiped)
                );

            string fileTree = WFileNode.GenFileTree(GetFileNode(driveLetter));
            string ver = DateTime.Now.ToString("yyyy-MM-dd HHmm");

            string previous = "";
            if (info.FileTreeVersions.Length > 0)
            {
                try { previous = File.ReadAllText(info.SavePath + "\\diskInfos\\FileTree " + info.FileTreeVersions[0] + ".txt"); }
                catch (Exception)
                {
                    // ignored
                }
            }
            if (previous != fileTree)
            {
                string[] vers = new string[info.FileTreeVersions.Length + 1];
                Array.Copy(info.FileTreeVersions, 0, vers, 1, info.FileTreeVersions.Length);
                vers[0] = ver;
                info.FileTreeVersions = vers;
                File.WriteAllText(info.SavePath + "\\diskInfos\\FileTree " + ver + ".txt", fileTree);
            }
            else
                _logger.LogI("新文件树与最近一次保存的文件树相同，不再重复写入");

            ++info.PlugInTimes;
            info.FileCount = files.Count + Utils.Concat(Utils.FMap(filesSkiped.ToArray(), (x) => x.Value));
            info.CopiedFileCount = files.Count;
            info.LastModified = DateTimeOffset.Now.ToUnixTimeSeconds();

            _infos[index] = info;

            UsbInfo.SaveBasicInfos(_infos.ToArray());
        }
        private void PrepareDrive(string driveLetter, string pnpDeviceId)
        {
            DriveInfo drive = new DriveInfo(driveLetter);
            if (drive.IsReady)
            {
                if (_cfg.AutoOpenExplorer)
                    StartExplorer(driveLetter);

                _logger.LogTrigger("检测到设备： " + drive.VolumeLabel);

                if (_cfg.RejectNewDevice)
                {
                    bool skip = true;
                    foreach (var info in _infos)
                    {
                        if (info.PnpDeviceId == pnpDeviceId)
                        {
                            skip = false;
                            break;
                        }
                    }
                    if (skip)
                    {
                        _logger.LogI("该USB设备不在已有列表内，跳过");
                        return;
                    }
                }

                for (int i = 0; i < _infos.Count; ++i)
                {
                    if (_infos[i].PnpDeviceId == pnpDeviceId)
                    {
                        if (_infos[i].Excluded)
                            _logger.LogI("该USB设备在白名单内，跳过");
                        else
                            CopyFiles(i, driveLetter);
                        return;
                    }
                }

                _infos.Add(new UsbInfo
                {
                    PnpDeviceId = pnpDeviceId,
                    VolumeLabel = drive.VolumeLabel,
                    SavePath = GetNewDirName(drive.VolumeLabel, _cfg.SavePath) + "\\",
                    PlugInTimes = 0,
                    Excluded = false,
                    FileTreeVersions = Array.Empty<string>(),
                    LastModified = DateTimeOffset.Now.ToUnixTimeSeconds()
                });
                CopyFiles(_infos.Count - 1, driveLetter);
            }
        }


        private Tuple<List<string>, Dictionary<string, int>> GetSpecfiedFiles(string path, string[] exts)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                List<string> ret = new List<string>();
                Dictionary<string, int> skiped = new Dictionary<string, int>();
                foreach (string dir in dirs)
                {
                    Tuple<List<string>, Dictionary<string, int>> childs = GetSpecfiedFiles(dir, exts);
                    if (childs == null)
                        continue;
                    foreach (string retChild in childs.Item1)
                        ret.Add(retChild);
                    foreach (KeyValuePair<string, int> skipChild in childs.Item2)
                        if (skiped.ContainsKey(skipChild.Key))
                            skiped[skipChild.Key] += skipChild.Value;
                        else
                            skiped[skipChild.Key] = skipChild.Value;
                }
                foreach (string file in files)
                    if (Array.IndexOf(exts, file.Split('.')[^1].ToLower()) >= 0)
                        ret.Add(file);
                    else
                    {
                        string ext = file.Split('.')[^1].ToLower();
                        if (skiped.ContainsKey(ext))
                            ++skiped[ext];
                        else
                            skiped[ext] = 1;
                    }

                return new Tuple<List<string>, Dictionary<string, int>>(ret, skiped);
            }
            catch (UnauthorizedAccessException) { return null; }
            catch (Exception) { _logger.LogE($"分析 {path} 时发生错误"); return null; }
        }

        private WFileNode GetFileNode(string path)
        {
            try
            {
                string[] dirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                WFileNode[] childs = Utils.FMap(dirs, GetFileNode);

                return new WFileNode
                {
                    Name = Path.GetFileName(path),
                    ChildLeafs = Utils.FMap(files, Path.GetFileName),
                    ChildNodes = childs
                };

            }
            catch (UnauthorizedAccessException) { return null; }
            catch (Exception) { _logger.LogE($"在{path}下取得文件列表失败"); return null; }
        }

        private static string GetNewDirName(string volumeName, string saveDir)
        {
            string vName = GetValidName(volumeName);
            string ret = saveDir + "\\" + vName;
            uint i = 1;
            while (true)
            {
                if (!Directory.Exists(ret))
                    return ret;
                ret = saveDir + "\\" + vName + " [" + i + "]";
                ++i;
            }

        }

        public static void StartExplorer(string loc)
        {
            Process p = new Process
            {
                StartInfo = { FileName = "cmd.exe", Arguments = "/c explorer.exe " + loc, CreateNoWindow = true }
            };
            p.Start();
        }

        private static string GetFileCount(Dictionary<string, int> dict)
        {
            IOrderedEnumerable<KeyValuePair<string, int>> orderedDict = dict.OrderByDescending(i => i.Value);
            var ret = "";
            foreach ((string ext, int value) in orderedDict)
            {
                string lines, count;
                if (value > 80)
                {
                    lines = new string('*', Math.Min(80, value / 10));
                    count = value.ToString() + "/10";
                }
                else
                {
                    lines = new string('|', value);
                    count = value.ToString();
                }

                ret += $"*.{ext}{new string(' ', Math.Max(0, 15 - ext.Length))}{lines}{new string(' ', Math.Max(0, 80 - lines.Length))}({count})\r\n";
            }
            return ret;
        }

        private static string GetValidName(string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"[^\w\-. ]", "",
                                RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (RegexMatchTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}
