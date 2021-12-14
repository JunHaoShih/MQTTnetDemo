using MQTTBrokerConsole.MQTT;
using System;

namespace MQTTBrokerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MQTTChatHandler mQTTHandler = new MQTTChatHandler();
            mQTTHandler.Start();
            Console.WriteLine("Hello World!");
        }
    }
}
