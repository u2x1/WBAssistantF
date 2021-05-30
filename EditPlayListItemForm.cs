using System;
using System.Windows.Forms;
using WBAssistantF.Module.AutoPlay;

namespace WBAssistantF
{
    public partial class EditPlayListItemForm : Form
    {
        private readonly Action<PlayListItem> callback;
        private readonly PlayListItem item;

        public EditPlayListItemForm(string filePath, Action<PlayListItem> _callback)
        {
            InitializeComponent();
            callback = _callback;
            item = new PlayListItem {FilePath = filePath};
        }

        private void EditPlayListItemForm_Load(object sender, EventArgs e)
        {
        }

        private void confirm_button_Click(object sender, EventArgs e)
        {
            callback(item);
        }
    }
}