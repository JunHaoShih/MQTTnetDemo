using MQTTClientFormTest.MQTT;
using MQTTClientFormTest.View;
using MQTTDataAccessLib.Data;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MQTTClientFormTest.Presenter
{
    /// <summary>
    /// 聊天室主視窗的Presenter
    /// </summary>
    public class MainPresenter
    {
        /// <summary>
        /// MQTT聊天室的客戶端
        /// </summary>
        private MQTTChatClientHandler handler = null;

        /// <summary>
        /// 聊天室的主視窗
        /// </summary>
        private readonly MainForm view;

        /// <summary>
        /// 聊天室主視窗Presenter的建構子，將主視窗的event與presenter連結
        /// </summary>
        /// <param name="form"></param>
        public MainPresenter(MainForm form)
        {
            view = form;
            SetEventLinks();
        }

        /// <summary>
        /// 與MainForm的事件做連結
        /// </summary>
        private void SetEventLinks()
        {
            // 將MainForm的連線按鈕click事件與ConnectToMqttServer連結
            view.OnConnectionClicked += ConnectToMqttServer;
            // 將MainForm的登出按鈕click事件與DisconnectFromMqttServer連結
            view.OnDisconnectClicked += DisconnectFromMqttServer;
            // 將MainForm的加入聊天室按鈕click事件與OpenJoinChatDialog連結
            view.OnJoinChatClicked += OpenJoinChatDialog;
        }

        /// <summary>
        /// 與MQTT伺服器連線
        /// </summary>
        /// <param name="ip">MQTT伺服器的IP</param>
        /// <param name="port">MQTT伺服器的PORT</param>
        /// <param name="userName">帳號</param>
        /// <param name="password">密碼</param>
        private void ConnectToMqttServer(string ip, int port, string userName, string password)
        {
            if (handler == null)
            {
                handler = new MQTTChatClientHandler(ip, port, userName, password);
                _ = handler.ClientStartAsync(OnMessageReceived, OnClientConnected, OnClientDisconnected, OnError);
            }
        }

        /// <summary>
        /// 處理未知錯誤
        /// </summary>
        /// <param name="message"></param>
        private void OnError(string message)
        {
            MessageBox.Show($"發生錯誤{Environment.NewLine}{message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            handler = null;
        }

        /// <summary>
        /// 處理客戶端離線時要處理的事情，主要的事情有<br/>
        /// 1. enable連線UI、disable聊天UI<br/>
        /// 2. 處理中斷狀態與server的exception
        /// </summary>
        /// <param name="e"></param>
        private void OnClientDisconnected(MqttClientDisconnectedEventArgs e)
        {
            // 將連線的UI給enable
            view.EnableConnectionUI(true);
            // 將聊天的UI給disable
            view.EnablePanelCenter(false);
            // 清除聊天的tabpage與dictionary
            view.ClearAllTopics();
            if (e.Exception != null && e.ClientWasConnected)
            {
                MessageBox.Show($"發生錯誤{Environment.NewLine}{e.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            handler = null;
        }

        /// <summary>
        /// 處理客戶端連線成功時要處理的事情，主要的事情有<br/>
        /// 1. disable連線UI、enable聊天UI
        /// </summary>
        /// <param name="e"></param>
        private void OnClientConnected(MqttClientConnectedEventArgs e)
        {
            // 將連線的UI給disable
            view.EnableConnectionUI(false);
            // 將聊天的UI給enable
            view.EnablePanelCenter(true);
        }

        /// <summary>
        /// 處理收到server傳來的訊息要怎麼處理，主要的事情有<br/>
        /// 1. 將server的訊息轉換成ChatMessage並加到ChatUI上
        /// </summary>
        /// <param name="e"></param>
        private void OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]);
            ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage>(message);
            view.AppendTopicMessage(e.ApplicationMessage.Topic, chatMessage);
        }

        /// <summary>
        /// 與server中斷連線
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectFromMqttServer(object sender, EventArgs e)
        {
            if (handler != null)
            {
                _ = handler.ClientEndAsync(OnError);
            }
        }

        /// <summary>
        /// 開啟加入聊天室對話框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenJoinChatDialog(object sender, EventArgs e)
        {
            using (var joinChatDialog = new JoinChatDialog())
            {
                // 開啟對話框並取得DialogResult
                var dialogResult = joinChatDialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    // 訂閱對話框輸入的Topic
                    _ = handler.SubscribeAsync(joinChatDialog.Topic, OnError);
                    // 嘗試為該Topic新增TabPage
                    var IsAdded = view.TryAddChatTabPage(joinChatDialog.Topic, out ChatControl chatControl);
                    // 若有新增，則把回傳的ChatControl的發送按鈕click事件與ChatControlPublish連結
                    if (IsAdded)
                    {
                        chatControl.OnBtnMessageSendClicked += ChatControlPublish;
                    }
                }
            }
        }

        /// <summary>
        /// 將指定topic的訊息publish到server
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="message"></param>
        private void ChatControlPublish(string topic, string message)
        {
            _ = handler.PublishAsync(topic, message, OnError);
        }
    }
}
