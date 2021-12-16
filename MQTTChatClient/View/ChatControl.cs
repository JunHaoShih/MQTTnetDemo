using MQTTDataAccessLib.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MQTTChatClient.View
{
    public partial class ChatControl : UserControl
    {
        /// <summary>
        /// 送出按鈕的click事件
        /// </summary>
        public Action<string, string> OnBtnMessageSendClicked;

        /// <summary>
        /// 聊天室Topic(也就是MQTT的Topic)
        /// </summary>
        public string Topic
        {
            get { return lblTopic.Text; }
            set { lblTopic.Text = value; }
        }

        public ChatControl()
        {
            InitializeComponent();
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            SendMessageAndClear();
        }

        /// <summary>
        /// 發送訊息並清空tbMessageInput
        /// </summary>
        private void SendMessageAndClear()
        {
            OnBtnMessageSendClicked?.Invoke(lblTopic.Text, tbMessageInput.Text);
            tbMessageInput.Clear();
        }

        /// <summary>
        /// 將ChatMessage加入對話框
        /// </summary>
        /// <param name="chatMessage"></param>
        public void AppendChatMessage(ChatMessage chatMessage)
        {
            var result = tbChatBox.BeginInvoke((MethodInvoker)delegate
            {
                tbChatBox.AppendText(chatMessage.ToChatString() + Environment.NewLine);
            });
            tbChatBox.EndInvoke(result);
        }

        private void tbMessageInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 檢查是否是鍵盤Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendMessageAndClear();
            }
        }
    }
}
