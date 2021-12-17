using MQTTDataAccessLib.Models;
using MQTTDataAccessLib.Models.DataTypes;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTBrokerConsole.MQTT
{
    /// <summary>
    /// MQTT Broker聊天室伺服器
    /// </summary>
    public class MQTTChatHandler
    {
        private IMqttServer mqttServer;

        /// <summary>
        /// 非同步啟動MQTT Broker
        /// </summary>
        /// <returns></returns>
        private async Task StartMQTTServerAsync()
        {
            Console.WriteLine("Starting MQTT server...");
            var serverOptions = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(1883)
                .WithPersistentSessions()
                // 設定連線者的驗證
                .WithConnectionValidator(ValidateConnector)
                // 設定訂閱的攔截事件
                .WithSubscriptionInterceptor(InterCeptSubscription)
                // 設定訊息的攔截事件
                .WithApplicationMessageInterceptor(InterceptMessage)
                .Build();
            mqttServer = new MqttFactory().CreateMqttServer();
            // 設定server接收到客戶端發送的訊息的事件
            mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnApplicationMessageReceived);
            // 設定客戶端成功連線server的事件
            mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(OnClientConnected);
            // 設定客戶端斷線的事件
            mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(OnClientDisconnected);
            // 設定客戶端向server訂閱特定Topic的事件
            mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(OnTopicSubscribe);
            // 客戶端向server取消訂閱特定Topic的事件
            mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(OnTopicUnsubscribe);
            // 開啟MQTT server
            await mqttServer.StartAsync(serverOptions);
            Console.WriteLine("MQTT server started!");
        }

        /// <summary>
        /// 啟動MQTT Broker並用ManualResetEvent避免server直接結束
        /// </summary>
        public void Start()
        {
            // 啟動server
            StartMQTTServerAsync().Wait();

            // 初始化ManualResetEvent
            ManualResetEvent quitEvent = new ManualResetEvent(false);

            // 在CancelKeyPress事件觸發時結束quitEvent的WaitOne
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                quitEvent.Set();
                eArgs.Cancel = true;
            };

            quitEvent.WaitOne(Timeout.Infinite);

            Console.WriteLine("ManualResetEvent triggered");
        }

        /// <summary>
        /// 客戶端向server訂閱特定Topic
        /// </summary>
        /// <param name="e"></param>
        private void OnTopicSubscribe(MqttServerClientSubscribedTopicEventArgs e)
        {
            string message = $"客戶端: { e.ClientId } 已進入「{ e.TopicFilter.Topic }」!";
            Console.WriteLine(message);
            PublishMessage(e.TopicFilter.Topic, message);
        }

        /// <summary>
        /// 客戶端向server取消訂閱特定Topic
        /// </summary>
        /// <param name="e"></param>
        private void OnTopicUnsubscribe(MqttServerClientUnsubscribedTopicEventArgs e)
        {
            string message = $"客戶端: { e.ClientId } 已離開「{ e.TopicFilter }」!";
            Console.WriteLine(message);
            PublishMessage(e.TopicFilter, message);
        }

        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="context"></param>
        private void ValidateConnector(MqttConnectionValidatorContext context)
        {
            if (context.Password == "Password" && !String.IsNullOrWhiteSpace(context.Username))
            {
                Console.WriteLine($"使用者: { context.Username } 為合法使用者");
            }
            else
            {
                Console.WriteLine($"使用者: { context.Username } 為非法使用者");
                context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
            }
        }

        /// <summary>
        /// 攔截訂閱
        /// </summary>
        /// <param name="context"></param>
        private void InterCeptSubscription(MqttSubscriptionInterceptorContext context)
        {
            context.AcceptSubscription = true;
        }

        /// <summary>
        /// 攔截訊息
        /// </summary>
        /// <param name="context"></param>
        private void InterceptMessage(MqttApplicationMessageInterceptorContext context)
        {
            context.AcceptPublish = true;
        }

        /// <summary>
        /// 處理server接收到客戶端發送的訊息
        /// </summary>
        /// <param name="e"></param>
        private void OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            // 客戶針對的Topic
            string topic = e.ApplicationMessage.Topic;
            // Payload是客戶端發送過來的訊息，為byte[]，請依照自己的需求轉換
            string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]);
            //ChatMessage chatMessage = JsonConvert.DeserializeObject<ChatMessage>(message);
            ChatRoomMessage chatRoomMessage = JsonConvert.DeserializeObject<ChatRoomMessage>(message);
            Console.WriteLine(topic + Environment.NewLine + chatRoomMessage.ToChatString());
        }

        /// <summary>
        /// 客戶端成功連線server的事件
        /// </summary>
        /// <param name="e"></param>
        private void OnClientConnected(MqttServerClientConnectedEventArgs e)
        {
            Console.WriteLine($"客戶端: { e.ClientId } 已連接!");
        }

        /// <summary>
        /// 客戶端斷線的事件
        /// </summary>
        /// <param name="e"></param>
        private void OnClientDisconnected(MqttServerClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"客戶端: { e.ClientId } 已離線!");
        }

        private void PublishMessage(string topic, string payload, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.ExactlyOnce, bool retain = false)
        {
            ChatRoomMessage chatRoomMessage = new ChatRoomMessage { UserName = new UserName("系統訊息"), Topic = topic, ChatMessage = new ChatText(payload) };
            //ChatMessage chatMessage = new ChatMessage() { UserName = "系統訊息", Message = payload };
            var jsonStr = JsonConvert.SerializeObject(chatRoomMessage);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(jsonStr)
                .WithQualityOfServiceLevel(qos)
                .WithRetainFlag(retain)
                .Build();

            mqttServer.PublishAsync(message);
        }
    }
}
