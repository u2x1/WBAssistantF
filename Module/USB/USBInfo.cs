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

        public static UsbInfo[] ReadBasicInfos(string path)
        {
            string raw = "";
            try
            {
                raw = File.ReadAllText(path).Trim();
            }
            catch (Exception)
            {
                // ignored
            }

            if (raw == "")
            {
                return Array.Empty<UsbInfo>();
            }

            string[][] infoStr = Utils.FMap(raw.Split('\n'), (x) => x.Split('|'));
            return Utils.FMap(infoStr, (x) => new UsbInfo
            {
                PnpDeviceId = x[0],
                VolumeLabel = x[1],
                PlugInTimes = uint.Parse(x[2]),
                LastModified = long.Parse(x[3]),
                FileCount = int.Parse(x[4]),
                CopiedFileCount = int.Parse(x[5]),
                SavePath = x[6],
                FileTreeVersions = x[7].Split(',').Where(a => a != "").ToArray(),
                Excluded = bool.Parse(x[8])
            });
        }

        public static void SaveBasicInfos(UsbInfo[] infos)
        {
            string path = "WBAData\\UsbInfos.txt";
            string infoStr = Utils.Concat(Utils.FMap(infos, (x) =>
                x.PnpDeviceId + "|" +
                x.VolumeLabel + "|" +
                x.PlugInTimes + "|" +
                x.LastModified + "|" +
                x.FileCount + "|" +
                x.CopiedFileCount + "|" +
                x.SavePath + "|" +
                Utils.Intersperse(x.FileTreeVersions, ',') + "|" +
                x.Excluded + "\n"
                ));
            File.WriteAllText(path, infoStr);
        }
    }

    internal class WFileNode
    {
        public WFileNode[] ChildNodes;
        public string[] ChildLeafs;
        public string Name;

        public static string GenFileTree(WFileNode root)
        {
            string files = Utils.Concat(Utils.FMap(root.ChildLeafs, (a) => a + "|"));
            string dirs = Utils.Concat(Utils.FMap(root.ChildNodes, GenFileTreeChilds));
            return files + dirs + "}";
        }

        private static string GenFileTreeChilds(WFileNode root)
        {
            string files = Utils.Concat(Utils.FMap(root.ChildLeafs, (a) => a + "|"));
            string dirs = Utils.Concat(Utils.FMap(root.ChildNodes, GenFileTreeChilds));
            return "*" + root.Name + "|" + (files.Length + dirs.Length) + "{" + files + dirs + "}";
        }
    }

    /// <summary>
    /// Lazy Read File Tree
    /// </summary>
    internal class RFileNodeL
    {
        public RFileNodeL[] ChildNodes;
        public string[] ChildLeafs;
        public string Name;
        public string RawData;

        private ulong _offset = 0;
        private bool _childReady = false;

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
                char* pChar = pString + _offset;

                // offset for reference of childs
                ulong offset = _offset;
                while (*pChar != '\0')
                {
                    if (*pChar == '}')
                    {
                        break;
                    }

                    // directory names begin with *
                    if (*pChar == '*')
                    {
                        ++pChar;
                        ++offset;
                        string dirName = GetUntil(ref pChar, ref offset, '|');
                        ulong dirSize = ulong.Parse(GetUntil(ref pChar, ref offset, '{'));
                        dirs.Add(new RFileNodeL(RawData) { Name = dirName, _offset = offset });
                        pChar += dirSize + 1;
                        offset += dirSize + 1;
                    }
                    else
                    {
                        files.Add(GetUntil(ref pChar, ref offset, '|'));
                    }
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
            else
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
            string str = "";
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
    /// Strict Read File Tree
    /// </summary>
    internal class RFileNode
    {
        public RFileNode[] ChildNodes;
        public string[] ChildLeafs;
        public string Name;

        public RFileNode Search(string text)
        {
            if (ChildLeafs == null && ChildNodes == null) { return null; }
            List<string> rstLeafs = new List<string>();
            if (ChildLeafs != null)
            {
                foreach (var leaf in ChildLeafs)
                {
                    if (leaf.IndexOf(text) != -1)
                    {
                        rstLeafs.Add(leaf);
                    }
                }
            }
            List<RFileNode> rstChilds = new List<RFileNode>();
            if (ChildNodes != null)
            {
                foreach (var child in ChildNodes)
                {
                    RFileNode c = child.Search(text);
                    if (c.ChildLeafs.Length == 0 && c.ChildNodes.Length == 0) { continue; }
                    rstChilds.Add(c);

                }
            }
            return new RFileNode() { Name = Name, ChildLeafs = rstLeafs.ToArray(), ChildNodes = rstChilds.ToArray() };
        }

        public static RFileNode FromLazy(RFileNodeL lazyNode)
        {
            RFileNode ret = new RFileNode
            {
                ChildLeafs = lazyNode.ChildLeafs,
                Name = lazyNode.Name
            };
            List<RFileNode> childs = new List<RFileNode>();
            lazyNode.EvaluateRNode();
            foreach (RFileNodeL lazyChild in lazyNode.ChildNodes)
            {
                lazyChild.EvaluateRNode();
                childs.Add(FromLazy(lazyChild));
            }
            ret.ChildNodes = childs.ToArray();
            return ret;
        }
    }
}
