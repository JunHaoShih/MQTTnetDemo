using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTChatClient.Enumerations
{
    /// <summary>
    /// MQTT連線所使用的通訊協定
    /// </summary>
    public enum MQTTProtocol
    {
        TCP = 1,
        WebSocket = 2
    }
}
