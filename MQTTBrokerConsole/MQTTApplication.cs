using MQTTBrokerConsole.MQTT;
using MQTTDataAccessLib.DAOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTBrokerConsole
{
    /// <summary>
    /// 初始化SQL資料表並啟動MQTT server
    /// </summary>
    class MQTTApplication
    {
        private readonly MQTTChatService chatHandler;

        private readonly IChatServerSchemaDao chatServerSchemaDao;

        public MQTTApplication(IChatServerSchemaDao chatServerSchemaDao, MQTTChatService chatHandler)
        {
            this.chatHandler = chatHandler;
            this.chatServerSchemaDao = chatServerSchemaDao;
        }

        /// <summary>
        /// 啟動MQTT server
        /// </summary>
        public void Run()
        {
            // 初始化SQL資料表
            chatServerSchemaDao.InitializeSchema();
            chatHandler.Start();
        }
    }
}
