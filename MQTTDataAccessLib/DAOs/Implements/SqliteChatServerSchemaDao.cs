using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.DAOs.Implements
{
    /// <summary>
    /// Sqlite初始化資料庫的DAO
    /// </summary>
    public class SqliteChatServerSchemaDao : IChatServerSchemaDao
    {
        private readonly string connectionString;

        public SqliteChatServerSchemaDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void InitializeSchema()
        {
            CreateChatUsersTable();
            CreateChatRoomMessagesTable();
        }

        /// <summary>
        /// 建立chat_users資料表(如果不存在)
        /// </summary>
        private void CreateChatUsersTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string cmd = @"
CREATE TABLE IF NOT EXISTS chat_users (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  user_name NVARCHAR(20) NOT NULL UNIQUE,
  password NVARCHAR(30) NOT NULL
);
";
                var result = connection.Execute(cmd);
            }
        }

        /// <summary>
        /// 建立chatroom_messages資料表(如果不存在)
        /// </summary>
        private void CreateChatRoomMessagesTable()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                string cmd = @"
CREATE TABLE IF NOT EXISTS chatroom_messages (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  topic NVARCHAR(20) NOT NULL,
  user_name NVARCHAR(20) NOT NULL,
  chat_message NVARCHAR(255) NOT NULL
);
";
                var result = connection.Execute(cmd);
            }
        }
    }
}
