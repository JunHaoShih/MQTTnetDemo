﻿using MQTTChatClient.Enumerations;
using MQTTChatClient.MQTT;
using MQTTChatClient.Service;
using MQTTChatClient.View;
using MQTTDataAccessLib.Models;
using MQTTDataAccessLib.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQTTChatClient
{
    /// <summary>
    /// 聊天室的主視窗
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// 紀錄topic與其對應之ChatControl
        /// </summary>
        private Dictionary<string, ChatControl> topicControls;

        /// <summary>
        /// MQTT服務，由Autofac注入
        /// </summary>
        public IMqttService MqttService { get; set; }

        /// <summary>
        /// 聊天室主視窗的建構子，初始化UI與topicControls
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            topicControls = new Dictionary<string, ChatControl>();
            cbProtocol.DataSource = Enum.GetValues(typeof(MQTTProtocol));
            tbUserName.MaxLength = UserName.MaxLength;
            tbPassword.MaxLength = Password.MaxLength;
        }

        /// <summary>
        /// 設定連線UI是否可以使用
        /// </summary>
        /// <param name="enable"></param>
        public void EnableConnectionUI(bool enable)
        {
            var result = tlpConnection.BeginInvoke((MethodInvoker)delegate
            {
                tlpConnection.Enabled = enable;
                btnConnect.Enabled = enable;
            });
            tlpConnection.EndInvoke(result);
        }

        /// <summary>
        /// 設定panelCenter是否可以使用
        /// </summary>
        /// <param name="enable"></param>
        public void EnablePanelCenter(bool enable)
        {
            var result = panelCenter.BeginInvoke((MethodInvoker)delegate
            {
                panelCenter.Enabled = enable;
            });
            panelCenter.EndInvoke(result);
        }

        /// <summary>
        /// 嘗試為topic新增一個tabpage，若該topic不存在，則新增tabpage、並用chatControl回傳<br/>
        /// 反之method回傳false
        /// </summary>
        /// <param name="topic">特定topic</param>
        /// <param name="chatControl">若新增成功，則回傳該ChatControl</param>
        /// <returns>如果新增成功，則回傳true，反之則為</returns>
        public bool TryAddChatTabPage(string topic, out ChatControl chatControl)
        {
            chatControl = null;
            if (!topicControls.ContainsKey(topic))
            {
                chatControl = new ChatControl() { Topic = topic, Dock = DockStyle.Fill };
                topicControls.Add(topic, chatControl);

                TabPage tabPage = new TabPage(topic);
                tabPage.Controls.Add(chatControl);
                AddTabPage(tabPage);
                return true;
            }
            chatControl = topicControls[topic];
            return false;
        }

        private void AddTabPage(TabPage tabPage)
        {
            var result = tabControlChat.BeginInvoke((MethodInvoker)delegate
            {
                tabControlChat.TabPages.Add(tabPage);
            });
            tabControlChat.EndInvoke(result);
        }

        /// <summary>
        /// 將特定Topic的訊息加到到對應的聊天頁面
        /// </summary>
        /// <param name="topic">topic</param>
        /// <param name="chatMessage">topic的聊天訊息</param>
        public void AppendTopicMessage(string topic, ChatRoomMessage chatMessage)
        {
            topicControls[topic].AppendChatMessage(chatMessage);
        }

        /// <summary>
        /// 清除聊天的tabpage與dictionary
        /// </summary>
        public void ClearAllTopics()
        {
            var result = tabControlChat.BeginInvoke((MethodInvoker)delegate
            {
                tabControlChat.TabPages.Clear();
                topicControls.Clear();
            });
            tabControlChat.EndInvoke(result);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            MQTTProtocol protocol = (MQTTProtocol)cbProtocol.SelectedItem;
            MqttService.ConnectToMqttServer(protocol, tbPath.Text, tbIp.Text, ((int)nudPort.Value), tbUserName.Text, tbPassword.Text);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            MqttService.DisconnectFromMqttServer();
        }

        private void btnJoinChat_Click(object sender, EventArgs e)
        {
            using (var joinChatDialog = new JoinChatDialog())
            {
                // 開啟對話框並取得DialogResult
                var dialogResult = joinChatDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    MqttService.JoinChat(joinChatDialog.Topic);
                }
            }
        }

        private void tbIp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnConnect.PerformClick();
            }
        }

        private void cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            MQTTProtocol protocol = (MQTTProtocol)cbProtocol.SelectedItem;
            switch (protocol)
            {
                case MQTTProtocol.TCP:
                    tbPath.Enabled = false;
                    break;
                case MQTTProtocol.WebSocket:
                    tbPath.Enabled = true;
                    break;
            }
            HandleUrlDisplay();
        }

        private void tbIp_TextChanged(object sender, EventArgs e)
        {
            HandleUrlDisplay();
        }

        /// <summary>
        /// 處理顯示URL
        /// </summary>
        private void HandleUrlDisplay()
        {
            if (((MQTTProtocol)cbProtocol.SelectedItem) == MQTTProtocol.TCP)
            {
                lblUrlDisplay.Text = $"{tbIp.Text}:{nudPort.Value}";
            }
            else
            {
                lblUrlDisplay.Text = $"ws://{tbIp.Text}:{nudPort.Value}/{tbPath.Text}";
            }
        }
    }
}
