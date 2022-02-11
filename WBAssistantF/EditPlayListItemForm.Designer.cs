
namespace WBAssistantF
{
    partial class EditPlayListItemForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.filePath_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.playTime_checkedListBox = new System.Windows.Forms.CheckedListBox();
            this.confirm_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件(夹)位置: ";
            // 
            // filePath_textBox
            // 
            this.filePath_textBox.Location = new System.Drawing.Point(102, 20);
            this.filePath_textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.filePath_textBox.Name = "filePath_textBox";
            this.filePath_textBox.Size = new System.Drawing.Size(385, 23);
            this.filePath_textBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "播放时间: ";
            // 
            // playTime_checkedListBox
            // 
            this.playTime_checkedListBox.ColumnWidth = 100;
            this.playTime_checkedListBox.FormattingEnabled = true;
            this.playTime_checkedListBox.Items.AddRange(new object[] {
            "第一节",
            "第二节",
            "第三节",
            "第四节",
            "第五节",
            "第六节",
            "第七节",
            "第八节",
            "夜修一"});
            this.playTime_checkedListBox.Location = new System.Drawing.Point(102, 54);
            this.playTime_checkedListBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.playTime_checkedListBox.MultiColumn = true;
            this.playTime_checkedListBox.Name = "playTime_checkedListBox";
            this.playTime_checkedListBox.Size = new System.Drawing.Size(385, 76);
            this.playTime_checkedListBox.TabIndex = 3;
            // 
            // confirm_button
            // 
            this.confirm_button.Location = new System.Drawing.Point(110, 185);
            this.confirm_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.confirm_button.Name = "confirm_button";
            this.confirm_button.Size = new System.Drawing.Size(82, 22);
            this.confirm_button.TabIndex = 4;
            this.confirm_button.Text = "确定";
            this.confirm_button.UseVisualStyleBackColor = true;
            this.confirm_button.Click += new System.EventHandler(this.confirm_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(318, 185);
            this.cancel_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(82, 22);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "取消";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(102, 136);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(78, 19);
            this.checkBox2.TabIndex = 7;
            this.checkBox2.Text = "断点续播";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(191, 136);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(143, 19);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "随机播放文件夹内容";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // EditPlayListItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 224);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.confirm_button);
            this.Controls.Add(this.playTime_checkedListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.filePath_textBox);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "EditPlayListItemForm";
            this.Text = "编辑播放对象";
            this.Load += new System.EventHandler(this.EditPlayListItemForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox filePath_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox playTime_checkedListBox;
        private System.Windows.Forms.Button confirm_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}