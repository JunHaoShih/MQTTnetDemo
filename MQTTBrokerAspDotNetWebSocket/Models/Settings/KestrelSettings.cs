

namespace MQTTBrokerAspDotNetWebSocket.Models.Settings
{
    /// <summary>
    /// 對應appsettings.json中的KestrelSettings，儲存Kestrel設定
    /// </summary>
    public class KestrelSettings
    {
        /// <summary>
        /// MQTT的TCP通訊Port
        /// </summary>
        public int MqttPort { get; private set; }

        /// <summary>
        /// HttpPort，同時也是MQTT的WebSocket Port
        /// </summary>
        public int HttpPort { get; private set; }

        /// <summary>
        /// Https Port
        /// </summary>
        public int HttpsPort { get; private set; }

        public KestrelSettings()
        {

        }

        public KestrelSettings(int mqttPort, int httpPort, int httpsPort)
        {
            MqttPort = mqttPort;
            HttpPort = httpPort;
            HttpsPort = httpsPort;
        }
    }
}
