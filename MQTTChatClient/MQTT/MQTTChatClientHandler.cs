using MQTTDataAccessLib.Models;
using MQTTDataAccessLib.Models.DataTypes;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Unsubscribing;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MQTTChatClient.MQTT
{
    /// <summary>
    /// MQTT客戶端聊天室
    /// </summary>
    public class MQTTChatClientHandler
    {
        /// <summary>
        /// MQTT客戶端
        /// </summary>
        private IMqttClient mqttClient = null;

        /// <summary>
        /// MQTT server的IP
        /// </summary>
        private readonly string ip;

        /// <summary>
        /// MQTT server的Port
        /// </summary>
        private readonly int port;

        /// <summary>
        /// 使用者名稱
        /// </summary>
        private readonly UserName userName;

        /// <summary>
        /// 密碼
        /// </summary>
        private readonly Password password;

        /// <summary>
        /// MQTT客戶端聊天室的建構子，初始化連線資訊
        /// </summary>
        /// <param name="ip">MQTT server的IP</param>
        /// <param name="port">MQTT server的Port</param>
        /// <param name="userName">使用者名稱</param>
        /// <param name="password">密碼</param>
        public MQTTChatClientHandler(string ip, int port, UserName userName, Password password)
        {
            this.ip = ip;
            this.port = port;
            this.userName = userName;
            this.password = password;
        }
        
        /// <summary>
        /// 以TCP非同步啟動客戶端
        /// </summary>
        /// <param name="OnMessageReceived">處理收到server訊息的method</param>
        /// <param name="OnClientConnected">處理客戶端成功連線的method</param>
        /// <param name="OnClientDisconnected">處理客戶端離線的method</param>
        /// <param name="OnError">處理發生異常的method</param>
        /// <returns></returns>
        public async Task ClientStartAsync(Action<MqttApplicationMessageReceivedEventArgs> OnMessageReceived,
            Action<MqttClientConnectedEventArgs> OnClientConnected,
            Action<MqttClientDisconnectedEventArgs> OnClientDisconnected,
            Action<string> OnError)
        {
            try
            {
                // 建立客戶端options
                var options = new MqttClientOptionsBuilder()
                    .WithTcpServer(ip, port)
                    .WithCredentials(userName, password)
                    // ClientId不給的話，系統會自動生成
                    .WithClientId(userName)
                    .WithCleanSession(false)
                    .WithCommunicationTimeout(TimeSpan.FromSeconds(2))
                    .Build();

                await LaunchClient(options, OnMessageReceived, OnClientConnected, OnClientDisconnected);
            }
            catch (Exception e)
            {
                OnError(e.Message);
            }
        }

        /// <summary>
        /// 以WebSocket非同步啟動客戶端
        /// </summary>
        /// <param name="path">WebSocket路徑</param>
        /// <param name="OnMessageReceived">處理收到server訊息的method</param>
        /// <param name="OnClientConnected">處理客戶端成功連線的method</param>
        /// <param name="OnClientDisconnected">處理客戶端離線的method</param>
        /// <param name="OnError">處理發生異常的method</param>
        /// <returns></returns>
        public async Task WebSocketClientStartAsync(
            string path,
            Action<MqttApplicationMessageReceivedEventArgs> OnMessageReceived,
            Action<MqttClientConnectedEventArgs> OnClientConnected,
            Action<MqttClientDisconnectedEventArgs> OnClientDisconnected,
            Action<string> OnError)
        {
            try
            {
                // 建立客戶端options
                var options = new MqttClientOptionsBuilder()
                    .WithWebSocketServer($"ws://{ip}:{port}/{path}")
                    .WithCredentials(userName, password)
                    // ClientId不給的話，系統會自動生成
                    .WithClientId(userName)
                    .WithCleanSession(false)
                    .WithCommunicationTimeout(TimeSpan.FromSeconds(2))
                    .Build();

                await LaunchClient(options, OnMessageReceived, OnClientConnected, OnClientDisconnected);
            }
            catch (Exception e)
            {
                OnError(e.Message);
            }
        }

        /// <summary>
        /// 依據MQTT client的啟動選項來啟動MqttClient
        /// </summary>
        /// <param name="options">MQTT client的啟動選項</param>
        /// <param name="OnMessageReceived">處理收到server訊息的method</param>
        /// <param name="OnClientConnected">處理客戶端成功連線的method</param>
        /// <param name="OnClientDisconnected">處理客戶端離線的method</param>
        /// <returns></returns>
        private async Task LaunchClient(
            IMqttClientOptions options,
            Action<MqttApplicationMessageReceivedEventArgs> OnMessageReceived,
            Action<MqttClientConnectedEventArgs> OnClientConnected,
            Action<MqttClientDisconnectedEventArgs> OnClientDisconnected)
        {
            mqttClient = new MqttFactory().CreateMqttClient();

            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnMessageReceived);
            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnClientConnected);
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnClientDisconnected);
            // 開始非同步連線
            // PS: 非同步連線(有await)出錯會直接拋出Exception，同步則是會觸發OnClientDisconnected
            await mqttClient.ConnectAsync(options);
        }

        /// <summary>
        /// 非同步終止客戶端
        /// </summary>
        /// <param name="OnError">處理發生異常的method</param>
        /// <returns></returns>
        public async Task ClientEndAsync(Action<string> OnError)
        {
            try
            {
                await mqttClient.DisconnectAsync();
            }
            catch (Exception e)
            {
                OnError(e.ToString());
            }
        }

        /// <summary>
        /// 向MQTT伺服器訂閱Topic
        /// </summary>
        /// <param name="topic">Topic名稱</param>
        /// <returns></returns>
        public async Task SubscribeAsync(string topic, Action<string> OnError)
        {
            try
            {
                var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();
                await mqttClient.SubscribeAsync(topicFilter);
            }
            catch (Exception e)
            {
                OnError(e.ToString());
            }
        }

        /// <summary>
        /// 向MQTT伺服器取消訂閱Topic，目前沒用到
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="OnError"></param>
        /// <returns></returns>
        public async Task UnSubscribeAsync(string topic, Action<string> OnError)
        {
            try
            {
                var options = new MqttClientUnsubscribeOptionsBuilder()
                    .WithTopicFilter(topic)
                    .Build();
                await mqttClient.UnsubscribeAsync(options);
            }
            catch (Exception e)
            {
                OnError(e.ToString());
            }
        }

        /// <summary>
        /// 非同步發布訊息
        /// </summary>
        /// <param name="topic">向指定Topic發布</param>
        /// <param name="chatMessage">發布訊息</param>
        /// <returns></returns>
        public async Task PublishAsync(string topic, string userInput, Action<string> OnError)
        {
            try
            {
                // 將ChatMessage轉換成json字串
                ChatRoomMessage chatRoomMessage = new ChatRoomMessage { UserName = userName, ChatMessage = new ChatText(userInput), Topic = topic };
                //ChatMessage chatMessage = new ChatMessage() { UserName = userName, Message = userInput };
                var message = JsonConvert.SerializeObject(chatRoomMessage);
                var applicationMessage = new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(Encoding.UTF8.GetBytes(message))
                    .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                    //.WithRetainFlag(true)
                    .Build();
                await mqttClient.PublishAsync(applicationMessage);
            }
            catch (Exception e)
            {
                OnError(e.ToString());
            }
        }
    }
}
