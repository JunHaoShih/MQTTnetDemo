using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MQTTChatClient.View
{
    public partial class JoinChatDialog : Form
    {
        public string Topic
        {
            get { return tbTopic.Text; }
        }

        public JoinChatDialog()
        {
            InitializeComponent();
            ActiveControl = tbTopic;
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Topic))
            {
                MessageBox.Show("聊天室名稱不可為空", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tbTopic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnJoin.PerformClick();
            }
        }
    }
}
