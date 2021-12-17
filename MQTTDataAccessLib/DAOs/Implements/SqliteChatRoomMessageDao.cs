using Dapper;
using Microsoft.Data.Sqlite;
using MQTTDataAccessLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.DAOs.Implements
{
    /// <summary>
    /// Sqlite聊天室訊息的DAO
    /// </summary>
    public class SqliteChatRoomMessageDao : IChatRoomMessageDao
    {
        private readonly string connectionString;

        public SqliteChatRoomMessageDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ChatRoomMessage InsertChatRoomMessage(ChatRoomMessage chatRoomMessage)
        {
            string cmd = @"
INSERT INTO chatroom_messages (topic, user_name, chat_message) VALUES (@Topic, @UserName, @ChatMessage);
SELECT seq FROM sqlite_sequence WHERE name = 'chatroom_messages'
";
            using (var connection = new SqliteConnection(connectionString))
            {
                var id = connection.ExecuteScalar<int>(cmd, chatRoomMessage);
                chatRoomMessage.Id = id;
            }
            return chatRoomMessage;
        }
    }
}
