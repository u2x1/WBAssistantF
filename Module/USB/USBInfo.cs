using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WBAssistantF.Module.USB
{
    internal struct UsbInfo
    {
        public string PnpDeviceId;
        public string VolumeLabel;
        public string SavePath;
        public uint PlugInTimes;
        public long LastModified;
        public int FileCount;
        public int CopiedFileCount;
        public bool Excluded;
        public string[] FileTreeVersions;
        public string Remark;

        private const string DataPath = "WBAData\\UsbInfos.txt";
        public static IEnumerable<UsbInfo> ReadBasicInfos()
        {
            var raw = "";
            try
            {
                raw = File.ReadAllText(DataPath).Trim();
            }
            catch (Exception)
            {
                // ignored
            }

            if (raw == "") return Array.Empty<UsbInfo>();

            var infoStr = Utils.FMap(raw.Split('\n'), x => x.Split('|'));
            return Utils.FMap(infoStr, x => new UsbInfo
            {
                PnpDeviceId = x[0],
                VolumeLabel = x[1],
                PlugInTimes = uint.Parse(x[2]),
                LastModified = long.Parse(x[3]),
                FileCount = int.Parse(x[4]),
                CopiedFileCount = int.Parse(x[5]),
                SavePath = x[6],
                FileTreeVersions = x[7].Split(',').Where(a => a != "").ToArray(),
                Excluded = bool.Parse(x[8]),
                Remark = x[9]
            });
        }

        public static void SaveBasicInfos(UsbInfo[] infos)
        {
            var infoStr = Utils.Concat(Utils.FMap(infos, x => Utils.Intersperse(new[]
            {
                x.PnpDeviceId,
                x.VolumeLabel,
                x.PlugInTimes.ToString(),
                x.LastModified.ToString(),
                x.FileCount.ToString(),
                x.CopiedFileCount.ToString(),
                x.SavePath,
                Utils.Intersperse(x.FileTreeVersions, ','),
                x.Excluded.ToString(),
                x.Remark
            }, '|') + "\n"));
            File.WriteAllText(DataPath, infoStr);
        }
    }

    internal class WFileNode
    {
        public string[] ChildLeafs;
        public WFileNode[] ChildNodes;
        public string Name;

        public static string GenFileTree(WFileNode root)
        {
            var files = Utils.Concat(Utils.FMap(root.ChildLeafs, a => a + "|"));
            var dirs = Utils.Concat(Utils.FMap(root.ChildNodes, GenFileTreeChilds));
            return files + dirs + "}";
        }

        private static string GenFileTreeChilds(WFileNode root)
        {
            var files = Utils.Concat(Utils.FMap(root.ChildLeafs, a => a + "|"));
            var dirs = Utils.Concat(Utils.FMap(root.ChildNodes, GenFileTreeChilds));
            return "*" + root.Name + "|" + (files.Length + dirs.Length) + "{" + files + dirs + "}";
        }
    }

    /// <summary>
    ///     Lazy Read File Tree
    /// </summary>
    internal class RFileNodeL
    {
        private bool _childReady;

        private ulong _offset;
        public string[] ChildLeafs;
        public RFileNodeL[] ChildNodes;
        public string Name;
        public string RawData;

        public RFileNodeL(string raw)
        {
            RawData = raw;
        }

        public unsafe void EvaluateRNode()
        {
            if (_childReady)
                return;
            var dirs = new List<RFileNodeL>();
            var files = new List<string>();
            fixed (char* pString = RawData)
            {
                var pChar = pString + _offset;

                // offset for reference of children
                var offset = _offset;
                while (*pChar != '\0')
                {
                    if (*pChar == '}') break;

                    // directory names begin with *
                    if (*pChar == '*')
                    {
                        ++pChar;
                        ++offset;
                        var dirName = GetUntil(ref pChar, ref offset, '|');
                        var dirSize = ulong.Parse(GetUntil(ref pChar, ref offset, '{'));
                        dirs.Add(new RFileNodeL(RawData) {Name = dirName, _offset = offset});
                        pChar += dirSize + 1;
                        offset += dirSize + 1;
                    }
                    else
                        files.Add(GetUntil(ref pChar, ref offset, '|'));
                }
            }

            ChildNodes = dirs.ToArray();
            ChildLeafs = files.ToArray();
            _childReady = true;
        }

        public void ModifyRNode(int[] depth, RFileNodeL node)
        {
            if (depth == null)
                return;
            ChildNodes[depth[0]] = GetModifiedRNode(ChildNodes[depth[0]], depth[1..], node);
        }

        private RFileNodeL GetModifiedRNode(RFileNodeL root, int[] depth, RFileNodeL node)
        {
            if (depth == null || depth.Length == 0)
                return node;

            root.ChildNodes[depth[0]] = GetModifiedRNode(root.ChildNodes[depth[0]], depth[1..], node);
            return root;
        }

        private static unsafe string GetUntil(ref char* pChar, ref ulong offset, char ch)
        {
            var str = "";
            while (true)
            {
                str += *pChar;
                ++pChar;
                ++offset;
                if (*pChar == ch)
                {
                    ++pChar;
                    ++offset;
                    break;
                }
            }

            return str;
        }
    }

    /// <summary>
    ///     Strict Read File Tree
    /// </summary>
    internal class RFileNode
    {
        public string[] ChildLeaves;
        public RFileNode[] ChildNodes;
        public string Name;

        public RFileNode Search(string text)
        {
            if (ChildLeaves == null && ChildNodes == null) return null;
            var rstLeaves = new List<string>();
            if (ChildLeaves != null)
                foreach (var leaf in ChildLeaves)
                    if (leaf.IndexOf(text, StringComparison.Ordinal) != -1)
                        rstLeaves.Add(leaf);
            var rstChildren = new List<RFileNode>();
            if (ChildNodes != null)
                rstChildren.AddRange(ChildNodes.Select(child => child.Search(text))
                    .Where(c => c.ChildLeaves.Length != 0 || c.ChildNodes.Length != 0));
            return new RFileNode {Name = Name, ChildLeaves = rstLeaves.ToArray(), ChildNodes = rstChildren.ToArray()};
        }

        public string[] GetFullPath()
        {
            return Utils.FMap(
                Utils.Merge(
                    ChildLeaves,
                    Utils.Concat(Utils.FMap(ChildNodes, x=>x.GetFullPath()))),
                x => Name + "\\" + x);
        }

        public static RFileNode FromLazy(RFileNodeL lazyNode)
        {
            lazyNode.EvaluateRNode();
            var ret = new RFileNode
            {
                ChildLeaves = lazyNode.ChildLeafs,
                Name = lazyNode.Name
            };
            
            var children = new List<RFileNode>();
            // lazyNode.EvaluateRNode();
            foreach (var lazyChild in lazyNode.ChildNodes)
            {
                lazyChild.EvaluateRNode();
                children.Add(FromLazy(lazyChild));
            }

            ret.ChildNodes = children.ToArray();
            return ret;
        }
    }
}