using MQTTBrokerConsole.MQTT;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTBrokerConsole
{
    class MQTTApplication
    {
        private readonly MQTTChatService chatHandler;

        public MQTTApplication(MQTTChatService chatHandler)
        {
            this.chatHandler = chatHandler;
        }

        public void Run()
        {
            chatHandler.Start();
        }
    }
}
