using MQTTBrokerAspDotNetWebSocket.Models.Settings;
using MQTTDataAccessLib.DAOs;
using MQTTDataAccessLib.Models;
using MQTTDataAccessLib.Models.DataTypes;
using MQTTnet;
using MQTTnet.AspNetCore;
using MQTTnet.Client.Receiving;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTBrokerAspDotNetWebSocket.MQTT
{
    public class MQTTChatService
    {
        private IMqttServer mqttServer;

        private readonly AppSettings appSettings;

        private readonly IChatUserDao chatUserDao;

        private readonly IChatRoomMessageDao chatRoomMessageDao;

        public MQTTChatService(AppSettings appSettings, IChatUserDao chatUserDao, IChatRoomMessageDao chatRoomMessageDao)
        {
            this.appSettings = appSettings;
            this.chatUserDao = chatUserDao;
            this.chatRoomMessageDao = chatRoomMessageDao;
        }

        public void BuildMqttServerOptions(AspNetMqttServerOptionsBuilder optionsBuilder)
        {
            optionsBuilder.WithoutDefaultEndpoint()
                .WithPersistentSessions()
                .WithConnectionBacklog(appSettings.MqttBacklogs)
                .WithConnectionValidator(ValidateConnector)
                .WithSubscriptionInterceptor(InterCeptSubscription)
                .WithApplicationMessageInterceptor(InterceptMessage)
                //.WithStorage(new RetainedMessageHandler())
                .Build();
        }

        public void ConfigureMqttServer(IMqttServer mqttServer)
        {
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

            this.mqttServer = mqttServer;
        }

        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="context"></param>
        private void ValidateConnector(MqttConnectionValidatorContext context)
        {
            var isUserNameValid = UserName.TryParse(context.Username, out UserName userName);
            var isPasswordValid = Password.TryParse(context.Password, out Password password);
            if (!isUserNameValid || !isPasswordValid)
            {
                context.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return;
            }

            var chatUser = chatUserDao.GetChatUser(userName);
            
            if (chatUser == null)
            {
                chatUserDao.InsertChatUser(new ChatUser { UserName = userName, Password = password });
            }
            else if (chatUser != null && chatUser.Password.Equals(password))
            {
                Console.WriteLine($"使用者: { userName } 為合法使用者");
            }
            else
            {
                Console.WriteLine($"使用者: { userName } 為非法使用者");
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

            chatRoomMessage = chatRoomMessageDao.InsertChatRoomMessage(chatRoomMessage);

            string returnString = JsonConvert.SerializeObject(chatRoomMessage);
            e.ApplicationMessage.Payload = Encoding.UTF8.GetBytes(returnString);

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

        private void PublishMessage(string topic, string payload, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.ExactlyOnce, bool retain = false)
        {
            ChatRoomMessage chatRoomMessage = new ChatRoomMessage { UserName = new UserName("System"), Topic = topic, ChatMessage = new ChatText(payload) };
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
