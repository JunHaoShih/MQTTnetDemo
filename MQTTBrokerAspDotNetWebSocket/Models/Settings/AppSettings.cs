

namespace MQTTBrokerAspDotNetWebSocket.Models.Settings
{
    /// <summary>
    /// 對應appsettings.json中的AppSettings
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// MQTT的Backlog數量
        /// </summary>
        public int MqttBacklogs { get; private set; }

        /// <summary>
        /// Kestrel設定
        /// </summary>
        public KestrelSettings KestrelSettings { get; private set; }

        public AppSettings()
        {

        }

        public AppSettings(int mqttBacklogs, KestrelSettings kestrelSettings)
        {
            MqttBacklogs = mqttBacklogs;
            KestrelSettings = kestrelSettings;
        }
    }
}
