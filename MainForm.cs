using Microsoft.Win32;
using Shell32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using WBAssistantF.Module.AutoPlay;
using WBAssistantF.Module.USB;
using WBAssistantF.Module.Wallpaper;
using Xabe.FFmpeg;

namespace WBAssistantF
{
    public partial class MainForm : Form
    {
        public readonly RichTextBox LogTextBox;
        private const string Version = "2.5.0";
        private const string AppName = "WBAssistant";
        private bool exitFlag = false;
        private readonly Logger logger;
        private readonly Copier copier;
        private readonly Config cfg;
        private readonly AutoPlay autoPlay;
        private readonly DesktopArrange desktopArrange;
        private List<UsbInfo> infos;
        private UsbInfo selectedInfo;
        private RFileNodeL fileNodeL; // file node of currently selected usb
        private RFileNode fileNodeS;  // same as above

        public MainForm()
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

            InitializeComponent();
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width,
                     Screen.PrimaryScreen.WorkingArea.Height - Height);
            LogTextBox = logTextBox;
            logger = new Logger(this);
            cfg = Config.ParseConfig("WBAData\\config.txt", logger);
            infos = new List<UsbInfo>(UsbInfo.ReadBasicInfos("WBAData\\UsbInfos.txt"));
            copier = new Copier(logger, ref cfg, ref infos);
            autoPlay = new AutoPlay(logger);
            if (cfg.EnableDesktopArrange) { desktopArrange = new DesktopArrange(copier, logger); }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            logger.LogC($"版本 {Version}, 由 Nutr1t07 (Nelson Xiao) 制作");

            if (!Directory.Exists("WBAData"))
            {
                Directory.CreateDirectory("WBAData");
                Directory.CreateDirectory("WBAData\\log");
            }
            if (cfg.RefreshWallpaper) { Task.Factory.StartNew(() => new WallpaperMain(logger).softPickWall()); }
            if (cfg.AutoPlay_FTP) { Task.Factory.StartNew(() => new AutoPlay(logger).CheckFtp(this)); }
            if (cfg.AutoPlay_EnAudio) { Task.Factory.StartNew(() => new AutoPlay(logger).checkEnglishAudio(cfg.AutoPlay_EnAudio_Unit, cfg.AutoPlay_EnAudio_FileName)); }
            Task.Factory.StartNew(() => copier.StartCopierListen());

            InitialConfigPage();
            RefreshUsbInfos();
        }

        private void InitialConfigPage()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            checkBox1.Checked = registryKey.GetValue(AppName) != null;
            checkBox2.Checked = cfg.RefreshWallpaper;
            checkBox3.Checked = cfg.AutoOpenExplorer;
            checkBox4.Checked = cfg.AutoPlay_FTP;
            checkBox5.Checked = cfg.AutoPlay_EnAudio;
            reject_new_device_checkBox.Checked = cfg.RejectNewDevice;
            autoArrange_checkBox.Checked = cfg.EnableDesktopArrange;

            extension_textBox.Text = Utils.Intersperse(cfg.Extension, ',');
            savePath_textBox.Text = cfg.SavePath;
            enUnit_textBox.Text = cfg.AutoPlay_EnAudio_Unit;
            enFileName_textBox.Text = cfg.AutoPlay_EnAudio_FileName;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exitFlag)
            {
                e.Cancel = true;
                Hide();
            }
        }

        #region USBInfo UI logic

        /// <summary>
        /// refresh USBInfo tabpage
        /// </summary>
        public void RefreshUsbInfos()
        {
            try
            {
                infos = new List<UsbInfo>(UsbInfo.ReadBasicInfos("WBAData\\UsbInfos.txt"));
                usbInfo_TreeView.BeginUpdate();
                usbInfo_TreeView.Nodes.Clear();

                for (int i = 0; i < infos.Count; ++i)
                {
                    UsbInfo info = infos[i];
                    DateTime lastMdTime = DateTimeOffset.FromUnixTimeSeconds(info.LastModified).LocalDateTime;

                    usbInfo_TreeView.Nodes.Add($"{info.Remark} ({info.VolumeLabel}) ({calculateLastTime(lastMdTime)})");
                    usbInfo_TreeView.Nodes[i].Nodes.Add("最后修改: " + lastMdTime.ToString("yyyy-MM-dd HH:mm"));
                    usbInfo_TreeView.Nodes[i].Nodes.Add("插入次数: " + info.PlugInTimes);
                    usbInfo_TreeView.Nodes[i].Nodes.Add("白名单: " + info.Excluded);
                    usbInfo_TreeView.Nodes[i].Nodes.Add("总文件数: " + info.FileCount);
                    usbInfo_TreeView.Nodes[i].Nodes.Add("已复制文件数: " + info.CopiedFileCount);
                    usbInfo_TreeView.Nodes[i].Nodes.Add("保存路径: " + info.SavePath);
                    usbInfo_TreeView.Nodes[i].Nodes.Add("设备PNP: " + info.PnpDeviceId);
                    usbInfo_TreeView.Nodes[i].Nodes.Add("文件树");
                    foreach (string x in info.FileTreeVersions)
                    {
                        usbInfo_TreeView.Nodes[i].Nodes[7].Nodes.Add(x[..^2] + ":" + x[^2..]);
                    }
                }
                usbInfo_TreeView.EndUpdate();
            }
            catch (Exception ex)
            {
                usbInfo_TreeView.EndUpdate();
                logger.LogE($"刷新USB信息时出现了错误：\n{ex.Message}");
            }
        }

        private void usbInfo_TreeView_DoubleClick(object sender, EventArgs e)
        {
            fileNodeL = null;
            fileNodeS = null;
            no_result_label.Visible = false;
            TreeNode node = usbInfo_TreeView.SelectedNode;
            if (node != null && node.Parent != null && node.Parent.Text == "文件树")
            {
                tabControl.SelectedTab = fileTree_tabPage;
                jump_to_usbinfo_label.Visible = false;

                string ver = node.Text[..^3] + node.Text[^2..];
                string pnp = node.Parent.Parent.Nodes[6].Text[7..];

                int index = Utils.FirstWhich(infos.ToArray(), (a) => a.PnpDeviceId == pnp);
                if (index == -1)
                    return;

                selectedInfo = infos[index];

                volumeName_label.Text = selectedInfo.VolumeLabel;
                fileCount_label.Text = $"文件数: ({selectedInfo.CopiedFileCount}/{selectedInfo.FileCount})";
                lastModified_label.Text = node.Text;
                string fileTreeRaw;
                try
                {
                    fileTreeRaw = File.ReadAllText(selectedInfo.SavePath + "\\diskInfos\\FileTree " + ver + ".txt");
                }
                catch (Exception) { MessageBox.Show("打开文件树时出现错误"); return; }
                fileNodeL = new RFileNodeL(fileTreeRaw);
                fileNodeL.EvaluateRNode();

                refreshFileTreeView(fileNodeL);
            }
        }

        private void usbInfo_TreeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = usbInfo_TreeView.GetNodeAt(e.X, e.Y);
                usbInfo_TreeView.SelectedNode = node;
                if (node == null)
                    return;

                Utils.ForAll(usbInfo_menu.Items, (x) => ((ToolStripItem)x).Enabled = false);
                if (node.Parent == null)
                {
                    exclude_usbInfo_menuItem.Enabled = true;
                    remove_usbInfo_menuItem.Enabled = true;
                    removeAll_usbInfo_menuItem.Enabled = true;
                }
                else if (node.Parent.Text == "文件树")
                {
                    removeFileTree_usbInfo_menuItem.Enabled = true;
                }

                usbInfo_menu.Show(usbInfo_TreeView, e.Location);
            }
        }

        private void usbInfo_menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;
            int index = usbInfo_TreeView.SelectedNode.Index;

            if (item == exclude_usbInfo_menuItem)
            {
                UsbInfo a = infos[index];
                a.Excluded = !a.Excluded;
                infos[index] = a;
                UsbInfo.SaveBasicInfos(infos.ToArray());
            }
            else if (item == remove_usbInfo_menuItem)
            {
                DialogResult rst = MessageBox.Show("删除保存的USB信息数据？(已复制的文件不会被删除)", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rst == DialogResult.OK)
                {
                    infos.RemoveAt(index);
                    UsbInfo.SaveBasicInfos(infos.ToArray());
                }
                else return;
            }
            else if (item == removeFileTree_usbInfo_menuItem)
            {
                int usbIndex = usbInfo_TreeView.SelectedNode.Parent.Parent.Index;
                string ver = infos[usbIndex].FileTreeVersions[index];

                DialogResult rst = MessageBox.Show("删除文件树 [" + ver + "] ？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rst == DialogResult.OK)
                {
                    infos.RemoveAt(index);
                    UsbInfo.SaveBasicInfos(infos.ToArray());
                }
                else return;
            }
            else if (item == removeAll_usbInfo_menuItem)
            {
                DialogResult rst = MessageBox.Show("删除有关该USB设备的所有数据？(所有已复制文件都会被删除)", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rst == DialogResult.OK)
                {
                    try
                    {
                        Directory.Delete(infos[index].SavePath, true);
                    }
                    catch (Exception ex) { logger.LogE("在删除文件时出现了错误：\n" + ex.Message); }

                    infos.RemoveAt(index);
                    UsbInfo.SaveBasicInfos(infos.ToArray());
                }
                else return;
            }
            RefreshUsbInfos();
        }
        #endregion

        #region FileTree UI logic

        private void fileTree_treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes[0].Text != "|")
                return;
            fileTree_treeView.BeginUpdate();
            e.Node.Nodes.Clear();
            List<string> parents = GetParents(e.Node);
            RFileNodeL rn = fileNodeL;
            int[] index = new int[parents.Count];
            for (int i = 0; i < parents.Count; ++i)
            {
                index[i] = Utils.FirstWhich(rn.ChildNodes, (a) => a.Name == parents[i]);
                rn = rn.ChildNodes[index[i]];
            }
            rn.EvaluateRNode();
            for (int i = 0; i < rn.ChildNodes.Length; ++i)
            {
                e.Node.Nodes.Add(rn.ChildNodes[i].Name);
                e.Node.Nodes[i].Nodes.Add("|"); //placeholder
            }
            for (int i = 0; i < rn.ChildLeafs.Length; ++i)
            {
                e.Node.Nodes.Add(rn.ChildLeafs[i]);
            }
            if (e.Node.Nodes.Count == 0)
            {
                e.Node.Nodes.Add("=== 空文件夹 ===");
            }
            fileTree_treeView.EndUpdate();
        }

        private void fileTree_treeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = fileTree_treeView.SelectedNode;
            if (node == null || node.Nodes.Count != 0) { return; }
            string path = "";
            do
            {
                path = node.Text + "\\" + path;
                node = node.Parent;
            } while (node != null);
            path = selectedInfo.SavePath + path[..^1];
            if (!File.Exists(path))
            {
                MessageBox.Show("文件不存在，请检查该类型的文件拓展名是否已被写入配置。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OpenFile(path);
        }

        private void search_textBox_TextChanged(object sender, EventArgs e)
        {
            no_result_label.Visible = false;
            string text = search_textBox.Text;
            if (text == "")
            {
                refreshFileTreeView(fileNodeL);
                return;
            }

            if (fileNodeL.RawData.IndexOf(text) == -1)
            {
                fileTree_treeView.BeginUpdate();
                fileTree_treeView.Nodes.Clear();
                fileTree_treeView.EndUpdate();
                no_result_label.Visible = true;
                return;
            }
            if (fileNodeS == null)
            {
                fileNodeS = RFileNode.FromLazy(fileNodeL);
            }
            refreshFileTreeView(fileNodeS.Search(text));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Copier.StartExplorer(selectedInfo.SavePath);
        }

        #endregion

        #region General UI logic

        private void notifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { Show(); }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Hide();
            Opacity = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshUsbInfos();
        }

        private void notify_menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem item = e.ClickedItem;

            if (item == mainWindow_notify_menuItem)
            {
                Show();
            }
            else if (item == refreshWall_notify_menuItem)
            {
                new WallpaperMain(logger).pickWall();
            }
            else if (item == exit_toolStripMenuItem)
            {
                exitFlag = true;
                Application.Exit();
            }
            else if (item == playVideo_menuItem)
            {
                Task.Factory.StartNew(() => autoPlay.CheckFtp(this, true));
            }
            else if (item == playAudio_menuItem)
            {
                Task.Factory.StartNew(() => autoPlay.checkEnglishAudio(cfg.AutoPlay_EnAudio_Unit, cfg.AutoPlay_EnAudio_FileName, true));
            }
            else if (item == EnglishUnitPlusMenuItem)
            {
                cfg.AutoPlay_EnAudio_Unit = cfg.AutoPlay_EnAudio_Unit[0] >= '8'
                    ? "0"
                    : (cfg.AutoPlay_EnAudio_Unit[0] - '0' + 1).ToString();
                cfg.SaveConfig();
                enUnit_textBox.Text = cfg.AutoPlay_EnAudio_Unit;
            }
        }

        #endregion

        #region Helper


        private void refreshFileTreeView(RFileNodeL node)
        {
            fileTree_treeView.BeginUpdate();
            fileTree_treeView.Nodes.Clear();
            for (int i = 0; i < node.ChildNodes.Length; ++i)
            {
                fileTree_treeView.Nodes.Add(node.ChildNodes[i].Name);
                fileTree_treeView.Nodes[i].Nodes.Add("|");
            }
            for (int i = 0; i < node.ChildLeafs.Length; ++i)
            {
                fileTree_treeView.Nodes.Add(node.ChildLeafs[i]);
            }
            fileTree_treeView.EndUpdate();
        }

        private void refreshFileTreeView(RFileNode node)
        {
            fileTree_treeView.BeginUpdate();
            fileTree_treeView.Nodes.Clear();
            addNodes(fileTree_treeView.Nodes, node);
            fileTree_treeView.EndUpdate();
            fileTree_treeView.ExpandAll();
        }

        /// <summary>
        /// helper function for `refreshFileTreeView` to add node recursively
        /// </summary>
        /// <param name="treeNodeCollection"></param>
        /// <param name="node"></param>
        private void addNodes(TreeNodeCollection treeNodeCollection, RFileNode node)
        {
            for (int i = 0; i < node.ChildNodes.Length; ++i)
            {
                treeNodeCollection.Add(node.ChildNodes[i].Name);
                addNodes(treeNodeCollection[i].Nodes, node.ChildNodes[i]);
            }
            for (int i = 0; i < node.ChildLeafs.Length; ++i)
            {
                treeNodeCollection.Add(node.ChildLeafs[i]);
            }
        }

        /// <summary>
        /// helper function for `fileTree_treeView_BeforeExpand` to get a chain of parents
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static List<string> GetParents(TreeNode root)
        {
            if (root != null)
            {
                List<string> ret = GetParents(root.Parent);
                ret.Add(root.Text);
                return ret;
            }
            else
                return new List<string>();
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

        private string timeSpan2String(TimeSpan interval)
        {
            if (interval.TotalDays > 365)
            {
                int year = (int)interval.TotalDays / 365;
                int month = (int)interval.TotalDays % 365 / 31;
                return $"{year}年{month}月";
            }
            if (interval.TotalDays > 31)
            {
                int month = (int)interval.TotalDays / 31;
                int day = (int)interval.TotalDays % 31;
                return $"{month}月{day}天";
            }
            if (interval.TotalHours > 23)
            {
                int day = (int)interval.TotalHours / 24;
                int hour = (int)interval.TotalHours % 24;
                return $"{day}天{hour}小时";
            }
            if (interval.TotalMinutes > 59)
            {
                int hour = (int)interval.TotalMinutes / 59;
                int min = (int)interval.TotalMinutes % 59;
                return $"{hour}小时{min}分钟";
            }
            if (interval.TotalSeconds > 59)
            {
                int min = (int)interval.TotalSeconds / 59;
                int sec = (int)interval.TotalSeconds % 59;
                return $"{min}分钟{sec}秒";
            }
            return $"{interval.TotalSeconds}秒";
        }

        private string calculateLastTime(DateTime dateTime)
        {
            TimeSpan interval = DateTime.Now - dateTime;
            return timeSpan2String(interval) + "前";
        }
        #endregion

        #region Config

        private void autoArrange_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            cfg.EnableDesktopArrange = autoArrange_checkBox.Checked;
            if (cfg.EnableDesktopArrange) { desktopArrange.Start(); } else { desktopArrange.Stop(); }
            cfg.SaveConfig();
        }

        private void reject_new_device_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            cfg.RejectNewDevice = reject_new_device_checkBox.Checked;
            cfg.SaveConfig();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            cfg.RefreshWallpaper = checkBox2.Checked;
            cfg.SaveConfig();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            cfg.AutoOpenExplorer = checkBox3.Checked;
            cfg.SaveConfig();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (checkBox1.Checked)
            {
                registryKey.SetValue(AppName, Process.GetCurrentProcess().MainModule.FileName);
            }
            else
            {
                registryKey.DeleteValue(AppName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cfg.SavePath = savePath_textBox.Text;
            cfg.SaveConfig();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] extensions = extension_textBox.Text.Split(',');
                cfg.Extension = extensions;
            }
            catch (Exception)
            {
                MessageBox.Show("格式错误，请确保各个拓展名间有英文逗号(,)分隔");
            }
            cfg.SaveConfig();
        }
        private void saveEnUnitSave_btn_Click(object sender, EventArgs e)
        {
            cfg.AutoPlay_EnAudio_Unit = enUnit_textBox.Text;
            cfg.SaveConfig();
        }

        private void EnFileNameSave_btn_Click(object sender, EventArgs e)
        {
            string filenames = enFileName_textBox.Text;
            cfg.AutoPlay_EnAudio_FileName = filenames;
            cfg.SaveConfig();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            cfg.AutoPlay_EnAudio = checkBox5.Checked;
            cfg.SaveConfig();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            cfg.AutoPlay_FTP = checkBox4.Checked;
            cfg.SaveConfig();
        }
        #endregion

        private async void listView1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length < 1) return;
            string filePath = files[0];
            try
            {
                string fileName = Path.GetFileName(filePath);
                IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(filePath);
                string duration = timeSpan2String(mediaInfo.Duration);
            }
            catch (Exception ex)
            {
                logger.LogE("拖拽文件时出现错误：\n" + ex.Message);
            }

        }


        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }


    }
}
