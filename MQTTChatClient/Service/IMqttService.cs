using MQTTChatClient.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTChatClient.Service
{
    /// <summary>
    /// MqttService interface
    /// </summary>
    public interface IMqttService
    {
        /// <summary>
        /// 與MQTT server連線
        /// </summary>
        /// <param name="protocol">MQTT通訊協定類型</param>
        /// <param name="path">WebSocket路徑</param>
        /// <param name="ip">MQTT伺服器的IP</param>
        /// <param name="port">MQTT伺服器的PORT</param>
        /// <param name="userName">帳號</param>
        /// <param name="password">密碼</param>
        void ConnectToMqttServer(MQTTProtocol protocol, string path, string ip, int port, string userName, string password);

        /// <summary>
        /// 與MQTT server中斷連線
        /// </summary>
        void DisconnectFromMqttServer();

        /// <summary>
        /// 加入聊天室
        /// </summary>
        /// <param name="topic">較加入的聊天室名稱(同時也是mqtt的Topic)</param>
        void JoinChat(string topic);
    }
}
