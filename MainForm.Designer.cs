using System;

namespace WBAssistantF
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notify_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mainWindow_notify_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshWall_notify_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playAudio_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EnglishUnitPlusMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playVideo_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exit_toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.log_tabPage = new System.Windows.Forms.TabPage();
            this.config_tabPage = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.reject_new_device_checkBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.extension_textBox = new System.Windows.Forms.TextBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.savePathSave_btn = new System.Windows.Forms.Button();
            this.extensionSave_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.savePath_textBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EnFileNameSave_btn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.enFileName_textBox = new System.Windows.Forms.TextBox();
            this.saveEnUnitSave_btn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.enUnit_textBox = new System.Windows.Forms.TextBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.usbInfos_tabPage = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.usbInfo_TreeView = new System.Windows.Forms.TreeView();
            this.fileTree_tabPage = new System.Windows.Forms.TabPage();
            this.jump_to_usbinfo_label = new System.Windows.Forms.Label();
            this.no_result_label = new System.Windows.Forms.Label();
            this.search_textBox = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lastModified_label = new System.Windows.Forms.Label();
            this.fileCount_label = new System.Windows.Forms.Label();
            this.volumeName_label = new System.Windows.Forms.Label();
            this.fileTree_treeView = new System.Windows.Forms.TreeView();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.usbInfo_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exclude_usbInfo_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remove_usbInfo_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.removeFileTree_usbInfo_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.removeAll_usbInfo_menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoArrange_checkBox = new System.Windows.Forms.CheckBox();
            this.notify_menu.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.log_tabPage.SuspendLayout();
            this.config_tabPage.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.usbInfos_tabPage.SuspendLayout();
            this.fileTree_tabPage.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.usbInfo_menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notify_menu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "WBAssistantF";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseUp);
            // 
            // notify_menu
            // 
            this.notify_menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.notify_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainWindow_notify_menuItem,
            this.refreshWall_notify_menuItem,
            this.playAudio_menuItem,
            this.EnglishUnitPlusMenuItem,
            this.playVideo_menuItem,
            this.exit_toolStripMenuItem});
            this.notify_menu.Name = "contextMenuStrip_notifyIcon";
            this.notify_menu.Size = new System.Drawing.Size(192, 136);
            this.notify_menu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.notify_menu_ItemClicked);
            // 
            // mainWindow_notify_menuItem
            // 
            this.mainWindow_notify_menuItem.Name = "mainWindow_notify_menuItem";
            this.mainWindow_notify_menuItem.Size = new System.Drawing.Size(191, 22);
            this.mainWindow_notify_menuItem.Text = "打开主窗口";
            // 
            // refreshWall_notify_menuItem
            // 
            this.refreshWall_notify_menuItem.Name = "refreshWall_notify_menuItem";
            this.refreshWall_notify_menuItem.Size = new System.Drawing.Size(191, 22);
            this.refreshWall_notify_menuItem.Text = "刷新壁纸";
            // 
            // playAudio_menuItem
            // 
            this.playAudio_menuItem.Name = "playAudio_menuItem";
            this.playAudio_menuItem.Size = new System.Drawing.Size(191, 22);
            this.playAudio_menuItem.Text = "播放听力";
            // 
            // EnglishUnitPlusMenuItem
            // 
            this.EnglishUnitPlusMenuItem.Name = "EnglishUnitPlusMenuItem";
            this.EnglishUnitPlusMenuItem.Size = new System.Drawing.Size(191, 22);
            this.EnglishUnitPlusMenuItem.Text = "切换至下一听力单元";
            // 
            // playVideo_menuItem
            // 
            this.playVideo_menuItem.Name = "playVideo_menuItem";
            this.playVideo_menuItem.Size = new System.Drawing.Size(191, 22);
            this.playVideo_menuItem.Text = "播放电视台节目";
            // 
            // exit_toolStripMenuItem
            // 
            this.exit_toolStripMenuItem.Name = "exit_toolStripMenuItem";
            this.exit_toolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.exit_toolStripMenuItem.Text = "退出";
            // 
            // logTextBox
            // 
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTextBox.DetectUrls = false;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Location = new System.Drawing.Point(3, 3);
            this.logTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(626, 359);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.log_tabPage);
            this.tabControl.Controls.Add(this.config_tabPage);
            this.tabControl.Controls.Add(this.usbInfos_tabPage);
            this.tabControl.Controls.Add(this.fileTree_tabPage);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(663, 393);
            this.tabControl.TabIndex = 2;
            // 
            // log_tabPage
            // 
            this.log_tabPage.Controls.Add(this.logTextBox);
            this.log_tabPage.Location = new System.Drawing.Point(4, 24);
            this.log_tabPage.Name = "log_tabPage";
            this.log_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.log_tabPage.Size = new System.Drawing.Size(632, 365);
            this.log_tabPage.TabIndex = 0;
            this.log_tabPage.Text = "日志";
            this.log_tabPage.UseVisualStyleBackColor = true;
            // 
            // config_tabPage
            // 
            this.config_tabPage.AutoScroll = true;
            this.config_tabPage.Controls.Add(this.groupBox3);
            this.config_tabPage.Controls.Add(this.groupBox2);
            this.config_tabPage.Controls.Add(this.groupBox1);
            this.config_tabPage.Location = new System.Drawing.Point(4, 24);
            this.config_tabPage.Name = "config_tabPage";
            this.config_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.config_tabPage.Size = new System.Drawing.Size(655, 365);
            this.config_tabPage.TabIndex = 1;
            this.config_tabPage.Text = "配置";
            this.config_tabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.checkBox2);
            this.groupBox3.Location = new System.Drawing.Point(13, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(611, 74);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "通用";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(10, 22);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(205, 19);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Windows启动时自动打开本程序";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(10, 47);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(143, 19);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "程序启动时切换壁纸";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.autoArrange_checkBox);
            this.groupBox2.Controls.Add(this.reject_new_device_checkBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.extension_textBox);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.savePathSave_btn);
            this.groupBox2.Controls.Add(this.extensionSave_btn);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.savePath_textBox);
            this.groupBox2.Location = new System.Drawing.Point(13, 83);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(611, 157);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "USB";
            // 
            // reject_new_device_checkBox
            // 
            this.reject_new_device_checkBox.AutoSize = true;
            this.reject_new_device_checkBox.Location = new System.Drawing.Point(11, 46);
            this.reject_new_device_checkBox.Name = "reject_new_device_checkBox";
            this.reject_new_device_checkBox.Size = new System.Drawing.Size(117, 19);
            this.reject_new_device_checkBox.TabIndex = 9;
            this.reject_new_device_checkBox.Text = "拒绝新设备写入";
            this.reject_new_device_checkBox.UseVisualStyleBackColor = true;
            this.reject_new_device_checkBox.CheckedChanged += new System.EventHandler(this.reject_new_device_checkBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "拓展名:";
            // 
            // extension_textBox
            // 
            this.extension_textBox.Location = new System.Drawing.Point(79, 91);
            this.extension_textBox.Name = "extension_textBox";
            this.extension_textBox.Size = new System.Drawing.Size(300, 23);
            this.extension_textBox.TabIndex = 2;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(11, 22);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(229, 19);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "USB插入时自动打开文件资源管理器";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // savePathSave_btn
            // 
            this.savePathSave_btn.Location = new System.Drawing.Point(389, 120);
            this.savePathSave_btn.Name = "savePathSave_btn";
            this.savePathSave_btn.Size = new System.Drawing.Size(75, 23);
            this.savePathSave_btn.TabIndex = 7;
            this.savePathSave_btn.Text = "保存";
            this.savePathSave_btn.UseVisualStyleBackColor = true;
            this.savePathSave_btn.Click += new System.EventHandler(this.button3_Click);
            // 
            // extensionSave_btn
            // 
            this.extensionSave_btn.Location = new System.Drawing.Point(389, 91);
            this.extensionSave_btn.Name = "extensionSave_btn";
            this.extensionSave_btn.Size = new System.Drawing.Size(75, 23);
            this.extensionSave_btn.TabIndex = 4;
            this.extensionSave_btn.Text = "保存";
            this.extensionSave_btn.UseVisualStyleBackColor = true;
            this.extensionSave_btn.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "保存路径:";
            // 
            // savePath_textBox
            // 
            this.savePath_textBox.Location = new System.Drawing.Point(79, 120);
            this.savePath_textBox.Name = "savePath_textBox";
            this.savePath_textBox.Size = new System.Drawing.Size(300, 23);
            this.savePath_textBox.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EnFileNameSave_btn);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.enFileName_textBox);
            this.groupBox1.Controls.Add(this.saveEnUnitSave_btn);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.enUnit_textBox);
            this.groupBox1.Controls.Add(this.checkBox5);
            this.groupBox1.Controls.Add(this.checkBox4);
            this.groupBox1.Location = new System.Drawing.Point(13, 259);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(611, 136);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "自动播放";
            // 
            // EnFileNameSave_btn
            // 
            this.EnFileNameSave_btn.Location = new System.Drawing.Point(385, 100);
            this.EnFileNameSave_btn.Name = "EnFileNameSave_btn";
            this.EnFileNameSave_btn.Size = new System.Drawing.Size(75, 23);
            this.EnFileNameSave_btn.TabIndex = 13;
            this.EnFileNameSave_btn.Text = "保存";
            this.EnFileNameSave_btn.UseVisualStyleBackColor = true;
            this.EnFileNameSave_btn.Click += new System.EventHandler(this.EnFileNameSave_btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "英语材料文件名:";
            // 
            // enFileName_textBox
            // 
            this.enFileName_textBox.Location = new System.Drawing.Point(115, 100);
            this.enFileName_textBox.Name = "enFileName_textBox";
            this.enFileName_textBox.Size = new System.Drawing.Size(264, 23);
            this.enFileName_textBox.TabIndex = 14;
            // 
            // saveEnUnitSave_btn
            // 
            this.saveEnUnitSave_btn.Location = new System.Drawing.Point(165, 72);
            this.saveEnUnitSave_btn.Name = "saveEnUnitSave_btn";
            this.saveEnUnitSave_btn.Size = new System.Drawing.Size(75, 23);
            this.saveEnUnitSave_btn.TabIndex = 9;
            this.saveEnUnitSave_btn.Text = "保存";
            this.saveEnUnitSave_btn.UseVisualStyleBackColor = true;
            this.saveEnUnitSave_btn.Click += new System.EventHandler(this.saveEnUnitSave_btn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 15);
            this.label4.TabIndex = 12;
            this.label4.Text = "英语单元:";
            // 
            // enUnit_textBox
            // 
            this.enUnit_textBox.Location = new System.Drawing.Point(115, 72);
            this.enUnit_textBox.Name = "enUnit_textBox";
            this.enUnit_textBox.Size = new System.Drawing.Size(38, 23);
            this.enUnit_textBox.TabIndex = 11;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(10, 47);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(195, 19);
            this.checkBox5.TabIndex = 4;
            this.checkBox5.Text = "周二、周四播放英语听力材料";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(11, 22);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(195, 19);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "周一自动下载播放电视台节目";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
            // 
            // usbInfos_tabPage
            // 
            this.usbInfos_tabPage.Controls.Add(this.button2);
            this.usbInfos_tabPage.Controls.Add(this.usbInfo_TreeView);
            this.usbInfos_tabPage.Location = new System.Drawing.Point(4, 24);
            this.usbInfos_tabPage.Name = "usbInfos_tabPage";
            this.usbInfos_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.usbInfos_tabPage.Size = new System.Drawing.Size(632, 365);
            this.usbInfos_tabPage.TabIndex = 2;
            this.usbInfos_tabPage.Text = "USB信息";
            this.usbInfos_tabPage.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(536, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "刷新";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // usbInfo_TreeView
            // 
            this.usbInfo_TreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usbInfo_TreeView.Location = new System.Drawing.Point(3, 3);
            this.usbInfo_TreeView.Name = "usbInfo_TreeView";
            this.usbInfo_TreeView.Size = new System.Drawing.Size(626, 359);
            this.usbInfo_TreeView.TabIndex = 0;
            this.usbInfo_TreeView.DoubleClick += new System.EventHandler(this.usbInfo_TreeView_DoubleClick);
            this.usbInfo_TreeView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.usbInfo_TreeView_MouseClick);
            // 
            // fileTree_tabPage
            // 
            this.fileTree_tabPage.Controls.Add(this.jump_to_usbinfo_label);
            this.fileTree_tabPage.Controls.Add(this.no_result_label);
            this.fileTree_tabPage.Controls.Add(this.search_textBox);
            this.fileTree_tabPage.Controls.Add(this.linkLabel1);
            this.fileTree_tabPage.Controls.Add(this.lastModified_label);
            this.fileTree_tabPage.Controls.Add(this.fileCount_label);
            this.fileTree_tabPage.Controls.Add(this.volumeName_label);
            this.fileTree_tabPage.Controls.Add(this.fileTree_treeView);
            this.fileTree_tabPage.Location = new System.Drawing.Point(4, 24);
            this.fileTree_tabPage.Name = "fileTree_tabPage";
            this.fileTree_tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.fileTree_tabPage.Size = new System.Drawing.Size(632, 365);
            this.fileTree_tabPage.TabIndex = 3;
            this.fileTree_tabPage.Text = "文件树";
            this.fileTree_tabPage.UseVisualStyleBackColor = true;
            // 
            // jump_to_usbinfo_label
            // 
            this.jump_to_usbinfo_label.BackColor = System.Drawing.Color.WhiteSmoke;
            this.jump_to_usbinfo_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jump_to_usbinfo_label.Location = new System.Drawing.Point(3, 3);
            this.jump_to_usbinfo_label.Name = "jump_to_usbinfo_label";
            this.jump_to_usbinfo_label.Size = new System.Drawing.Size(626, 359);
            this.jump_to_usbinfo_label.TabIndex = 5;
            this.jump_to_usbinfo_label.Text = "请转到USB信息并选择一个文件树版本。";
            this.jump_to_usbinfo_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // no_result_label
            // 
            this.no_result_label.AutoSize = true;
            this.no_result_label.Location = new System.Drawing.Point(260, 159);
            this.no_result_label.Name = "no_result_label";
            this.no_result_label.Size = new System.Drawing.Size(98, 15);
            this.no_result_label.TabIndex = 7;
            this.no_result_label.Text = "找不到该文件。";
            this.no_result_label.Visible = false;
            // 
            // search_textBox
            // 
            this.search_textBox.Location = new System.Drawing.Point(274, 5);
            this.search_textBox.Name = "search_textBox";
            this.search_textBox.PlaceholderText = "搜索";
            this.search_textBox.Size = new System.Drawing.Size(74, 23);
            this.search_textBox.TabIndex = 6;
            this.search_textBox.TextChanged += new System.EventHandler(this.search_textBox_TextChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(353, 10);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(150, 15);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "在文件资源浏览器中打开";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lastModified_label
            // 
            this.lastModified_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lastModified_label.AutoSize = true;
            this.lastModified_label.Location = new System.Drawing.Point(516, 10);
            this.lastModified_label.Name = "lastModified_label";
            this.lastModified_label.Size = new System.Drawing.Size(95, 15);
            this.lastModified_label.TabIndex = 3;
            this.lastModified_label.Text = "2020-12-31 16:23";
            // 
            // fileCount_label
            // 
            this.fileCount_label.AutoSize = true;
            this.fileCount_label.Location = new System.Drawing.Point(116, 10);
            this.fileCount_label.Name = "fileCount_label";
            this.fileCount_label.Size = new System.Drawing.Size(114, 15);
            this.fileCount_label.TabIndex = 2;
            this.fileCount_label.Text = "File Count: 121/1244";
            // 
            // volumeName_label
            // 
            this.volumeName_label.AutoSize = true;
            this.volumeName_label.Location = new System.Drawing.Point(6, 10);
            this.volumeName_label.Name = "volumeName_label";
            this.volumeName_label.Size = new System.Drawing.Size(46, 15);
            this.volumeName_label.TabIndex = 1;
            this.volumeName_label.Text = "设备名";
            // 
            // fileTree_treeView
            // 
            this.fileTree_treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTree_treeView.Location = new System.Drawing.Point(3, 30);
            this.fileTree_treeView.Name = "fileTree_treeView";
            this.fileTree_treeView.Size = new System.Drawing.Size(626, 332);
            this.fileTree_treeView.TabIndex = 0;
            this.fileTree_treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.fileTree_treeView_BeforeExpand);
            this.fileTree_treeView.DoubleClick += new System.EventHandler(this.fileTree_treeView_DoubleClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(632, 365);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "播放列表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.AllowDrop = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 2);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(626, 361);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.listView1_DragDrop);
            this.listView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.listView1_DragEnter);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "文件名";
            this.columnHeader1.Width = 160;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "时长";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "播放时间";
            this.columnHeader3.Width = 200;
            // 
            // usbInfo_menu
            // 
            this.usbInfo_menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.usbInfo_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exclude_usbInfo_menuItem,
            this.remove_usbInfo_menuItem,
            this.toolStripSeparator1,
            this.removeFileTree_usbInfo_menuItem,
            this.toolStripSeparator2,
            this.removeAll_usbInfo_menuItem});
            this.usbInfo_menu.Name = "usbInfo_menu";
            this.usbInfo_menu.Size = new System.Drawing.Size(187, 104);
            this.usbInfo_menu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.usbInfo_menu_ItemClicked);
            // 
            // exclude_usbInfo_menuItem
            // 
            this.exclude_usbInfo_menuItem.Name = "exclude_usbInfo_menuItem";
            this.exclude_usbInfo_menuItem.Size = new System.Drawing.Size(186, 22);
            this.exclude_usbInfo_menuItem.Text = "移除/加入 白名单";
            // 
            // remove_usbInfo_menuItem
            // 
            this.remove_usbInfo_menuItem.Name = "remove_usbInfo_menuItem";
            this.remove_usbInfo_menuItem.Size = new System.Drawing.Size(186, 22);
            this.remove_usbInfo_menuItem.Text = "删除信息";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // removeFileTree_usbInfo_menuItem
            // 
            this.removeFileTree_usbInfo_menuItem.Name = "removeFileTree_usbInfo_menuItem";
            this.removeFileTree_usbInfo_menuItem.Size = new System.Drawing.Size(186, 22);
            this.removeFileTree_usbInfo_menuItem.Text = "移除此文件树版本";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(183, 6);
            // 
            // removeAll_usbInfo_menuItem
            // 
            this.removeAll_usbInfo_menuItem.Name = "removeAll_usbInfo_menuItem";
            this.removeAll_usbInfo_menuItem.Size = new System.Drawing.Size(186, 22);
            this.removeAll_usbInfo_menuItem.Text = "移除该USB所有数据";
            // 
            // autoArrange_checkBox
            // 
            this.autoArrange_checkBox.AutoSize = true;
            this.autoArrange_checkBox.Location = new System.Drawing.Point(10, 70);
            this.autoArrange_checkBox.Name = "autoArrange_checkBox";
            this.autoArrange_checkBox.Size = new System.Drawing.Size(130, 19);
            this.autoArrange_checkBox.TabIndex = 10;
            this.autoArrange_checkBox.Text = "自动整理桌面图标";
            this.autoArrange_checkBox.UseVisualStyleBackColor = true;
            this.autoArrange_checkBox.CheckedChanged += new System.EventHandler(this.autoArrange_checkBox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 393);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Opacity = 0D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WBAssistantF";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.notify_menu.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.log_tabPage.ResumeLayout(false);
            this.config_tabPage.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.usbInfos_tabPage.ResumeLayout(false);
            this.fileTree_tabPage.ResumeLayout(false);
            this.fileTree_tabPage.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.usbInfo_menu.ResumeLayout(false);
            this.ResumeLayout(false);

        }






        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notify_menu;
        private System.Windows.Forms.ToolStripMenuItem exit_toolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshWall_notify_menuItem;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage log_tabPage;
        private System.Windows.Forms.TabPage config_tabPage;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox extension_textBox;
        private System.Windows.Forms.Button extensionSave_btn;
        private System.Windows.Forms.TabPage usbInfos_tabPage;
        private System.Windows.Forms.TreeView usbInfo_TreeView;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage fileTree_tabPage;
        private System.Windows.Forms.TreeView fileTree_treeView;
        private System.Windows.Forms.Label fileCount_label;
        private System.Windows.Forms.Label volumeName_label;
        private System.Windows.Forms.Label lastModified_label;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button savePathSave_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox savePath_textBox;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label jump_to_usbinfo_label;
        private System.Windows.Forms.ContextMenuStrip usbInfo_menu;
        private System.Windows.Forms.ToolStripMenuItem exclude_usbInfo_menuItem;
        private System.Windows.Forms.ToolStripMenuItem remove_usbInfo_menuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem removeFileTree_usbInfo_menuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem removeAll_usbInfo_menuItem;
        private System.Windows.Forms.ToolStripMenuItem mainWindow_notify_menuItem;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox enUnit_textBox;
        private System.Windows.Forms.Button saveEnUnitSave_btn;
        private System.Windows.Forms.Button EnFileNameSave_btn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox enFileName_textBox;
        private System.Windows.Forms.ToolStripMenuItem playAudio_menuItem;
        private System.Windows.Forms.ToolStripMenuItem playVideo_menuItem;
        private System.Windows.Forms.ToolStripMenuItem EnglishUnitPlusMenuItem;
        private System.Windows.Forms.TextBox search_textBox;
        private System.Windows.Forms.Label no_result_label;
        private System.Windows.Forms.CheckBox reject_new_device_checkBox;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.CheckBox autoArrange_checkBox;
    }
}

