using MQTTChatClient.Enumerations;
using MQTTChatClient.MQTT;
using MQTTChatClient.View;
using MQTTDataAccessLib.Models;
using MQTTDataAccessLib.Models.DataTypes;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MQTTChatClient.Service
{
    public class MqttService : IMqttService
    {
        /// <summary>
        /// MQTT聊天室的客戶端
        /// </summary>
        private MQTTChatClientHandler handler = null;

        /// <summary>
        /// 主視窗
        /// </summary>
        private MainForm mainForm;

        /// <summary>
        /// MqttService的建構子，Autofac會在這裡注入主視窗MainForm
        /// </summary>
        /// <param name="mainForm"></param>
        public MqttService(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        /// <summary>
        /// 與MQTT伺服器連線
        /// </summary>
        /// <param name="protocol">MQTT通訊協定類型</param>
        /// <param name="path">WebSocket路徑</param>
        /// <param name="ip">MQTT伺服器的IP</param>
        /// <param name="port">MQTT伺服器的PORT</param>
        /// <param name="userName">帳號</param>
        /// <param name="password">密碼</param>
        public void ConnectToMqttServer(MQTTProtocol protocol, string path, string ip, int port, string userName, string password)
        {
            if (handler != null)
            {
                return;
            }
            var isUserNameValid = UserName.TryParse(userName, out UserName userNameObj);
            var isPasswordValid = Password.TryParse(password, out Password passwordObj);
            if (isUserNameValid && isPasswordValid)
            {
                handler = new MQTTChatClientHandler(ip, port, userNameObj, passwordObj);
                switch (protocol)
                {
                    case MQTTProtocol.TCP:
                        _ = handler.ClientStartAsync(OnMessageReceived, OnClientConnected, OnClientDisconnected, OnError);
                        break;
                    case MQTTProtocol.WebSocket:
                        _ = handler.WebSocketClientStartAsync(path, OnMessageReceived, OnClientConnected, OnClientDisconnected, OnError);
                        break;
                }
            }
            else
            {
                OnError("帳號密碼格式不正確");
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
            mainForm.EnableConnectionUI(true);
            // 將聊天的UI給disable
            mainForm.EnablePanelCenter(false);
            // 清除聊天的tabpage與dictionary
            mainForm.ClearAllTopics();
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
            mainForm.EnableConnectionUI(false);
            // 將聊天的UI給enable
            mainForm.EnablePanelCenter(true);
        }

        /// <summary>
        /// 處理收到server傳來的訊息要怎麼處理，主要的事情有<br/>
        /// 1. 將server的訊息轉換成ChatMessage並加到ChatUI上
        /// </summary>
        /// <param name="e"></param>
        private void OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]);
            ChatRoomMessage chatRoomMessage = JsonConvert.DeserializeObject<ChatRoomMessage>(message);
            var IsAdded = mainForm.TryAddChatTabPage(e.ApplicationMessage.Topic, out ChatControl chatControl);
            if (IsAdded)
            {
                chatControl.OnBtnMessageSendClicked += ChatControlPublish;
            }
            mainForm.AppendTopicMessage(e.ApplicationMessage.Topic, chatRoomMessage);
        }

        /// <summary>
        /// 與server中斷連線
        /// </summary>
        public void DisconnectFromMqttServer()
        {
            if (handler != null)
            {
                _ = handler.ClientEndAsync(OnError);
            }
        }

        /// <summary>
        /// 加入聊天室
        /// </summary>
        /// <param name="topic">較加入的聊天室名稱(同時也是mqtt的Topic)</param>
        public void JoinChat(string topic)
        {
            // 訂閱對話框輸入的Topic
            _ = handler.SubscribeAsync(topic, OnError);
            // 嘗試為該Topic新增TabPage
            var IsAdded = mainForm.TryAddChatTabPage(topic, out ChatControl chatControl);
            // 若有新增，則把回傳的ChatControl的發送按鈕click事件與ChatControlPublish連結
            if (IsAdded)
            {
                chatControl.OnBtnMessageSendClicked += ChatControlPublish;
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
