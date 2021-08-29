using WBAssistantF.Util;
using WPFWindow;

#nullable enable
namespace WBAssistantF.Module.USB
{
    internal class FileWatcher
    {
        public delegate void RecentFileAddedHandler(RecentFile recentFile);
        public event RecentFileAddedHandler? RecentFileAdded;

        private readonly List<string> _pptFiles = new();
        private readonly List<RecentFile> _recentFiles;
        private readonly Copier _copier;
        private readonly Config _config;

        private readonly WindowDetect _windowDetect = new();

        public FileWatcher(Copier copier, Config config, List<RecentFile> recentFiles)
        {
            _config = config;
            _copier = copier;
            _recentFiles = recentFiles ?? new List<RecentFile>();
        }

        public void Listen()
        {
            _windowDetect.NewWindowOpened += WindowDetectOnNewWindowOpened;
            _windowDetect.Listen();
        }

        private void WindowDetectOnNewWindowOpened(string title)
        {
            if (string.IsNullOrEmpty(title))
                return;

            bool returnFlag = true;
            string ext = "";
            foreach (string str in _config.Extension)
            {
                if (title.IndexOf(str, StringComparison.CurrentCulture) != -1)
                {
                    returnFlag = false;
                    ext = str;
                }
            }
            if (returnFlag) return;

            if (_pptFiles.Contains(title))
                return;

            _pptFiles.Add(title);

            List<UsbInfo> infos;
            if (_copier.LastInsertedUsbInfo != null)
            {
                infos = new List<UsbInfo> { (UsbInfo)_copier.LastInsertedUsbInfo };
                infos.AddRange(_copier.Infos);
            }
            else
                infos = _copier.Infos;

            string owner = "-";
            string file = title[0..title.LastIndexOf(ext)] + ext;
            string? path = null;

            // search the file on desktop first
            path = searchDesktopFiles(file);
            if (path != null)
            {
                owner = "桌面";
            }
            else
            {
                foreach (var info in infos)
                {
                    string ver = info.FileTreeVersions[0];
                    RFileNode rFileNode;
                    try
                    {
                        var raw = File.ReadAllText(
                            info.SavePath + "\\diskInfos\\FileTree " + ver + ".txt");
                        rFileNode = RFileNode.FromLazy(new RFileNodeL(raw));
                    }
                    catch (Exception) { continue; }

                    var paths = rFileNode.Search(file).GetFullPath();
                    if (paths.Length != 0)
                    {
                        path = info.SavePath + paths[0];
                        owner = info.Remark;
                        break;
                    }
                }
            }
            if (path == null) path = "-";

            RecentFile recentFile = new()
            {
                FileName = file,
                OpenTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Owner = owner,
                FilePath = path
            };
            _recentFiles.Add(recentFile);
            RecentFileAdded?.Invoke(recentFile);
            RecentFile.SaveRecentFIles(_recentFiles.ToArray());
        }

        private string? searchDesktopFiles(string query)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return deepSearchDir(desktopPath, query);
        }

        private string? deepSearchDir(string path, string query)
        {
            if (string.IsNullOrEmpty(path)) { return null; }
            var files = Directory.GetFiles(path);
            var dirs = Directory.GetDirectories(path);
            foreach (var i in files)
            {
                if (Path.GetFileName(i) == query)
                {
                    return i;
                }
            }
            foreach (string i in dirs)
            {
                var ret = deepSearchDir(i, query);
                if (ret != null) return ret;
            }
            return null;
        }

        public struct RecentFile
        {
            public string FileName;
            public long OpenTime;
            public string Owner;
            public string FilePath;

            //public RecentFile(string fileName, long openTime, string filePath, string owner)
            //{
            //    FileName = fileName;
            //    OpenTime = openTime;
            //    Owner = owner;
            //    FilePath = filePath;
            //}

            private const string DataPath = "WBAData\\RecentFiles.txt";

            public static void SaveRecentFIles(RecentFile[] recentFiles)
            {
                var fileStr = Utils.Concat(Utils.FMap(recentFiles, x => Utils.Intersperse(new[]
                {
                    x.FileName,
                    x.OpenTime.ToString(),
                    x.Owner,
                    x.FilePath
                }, '|') + "\n"));
                File.WriteAllText(DataPath, fileStr);
            }

            public static IEnumerable<RecentFile> ReadRecentFiles()
            {
                var raw = "";
                try
                {
                    raw = File.ReadAllText(DataPath).Trim();

                    if (raw == "") return Array.Empty<RecentFile>();

                    var infoStr = Utils.FMap(raw.Split('\n'), x => x.Split('|'));
                    return Utils.FMap(infoStr, x => new RecentFile
                    {
                        FileName = x[0],
                        OpenTime = long.Parse(x[1]),
                        Owner = x[2],
                        FilePath = x[3],
                    });
                }
                catch (Exception)
                {
                    return Array.Empty<RecentFile>();
                }
            }
        }
    }
}
#nullable disable