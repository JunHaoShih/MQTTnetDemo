using Dapper;
using Microsoft.Data.Sqlite;
using MQTTDataAccessLib.Models;
using MQTTDataAccessLib.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MQTTDataAccessLib.DAOs.Implements
{
    /// <summary>
    /// Sqlite聊天使用者的DAO
    /// </summary>
    public class SqliteChatUserDao : IChatUserDao
    {
        /// <summary>
        /// Sqlite的連線字串
        /// </summary>
        private readonly string connectionString;

        public SqliteChatUserDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 從chat_users資料表用id取得聊天使用者
        /// </summary>
        /// <param name="id">聊天使用者id</param>
        /// <returns></returns>
        public ChatUser GetChatUser(int id)
        {
            ChatUser chatUser = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                string cmd = "SELECT * FROM chat_users WHERE id = @Id";
                var chatUsers = connection.Query<ChatUser>(cmd, new { Id = id }).ToList();
                chatUser = chatUsers.Count == 0 ? null : chatUsers[0];
            }
            return chatUser;
        }

        /// <summary>
        /// 從chat_users資料表用使用者名稱取得聊天使用者
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <returns></returns>
        public ChatUser GetChatUser(UserName userName)
        {
            ChatUser chatUser = null;
            using (var connection = new SqliteConnection(connectionString))
            {
                string cmd = @"SELECT * FROM chat_users WHERE user_name = @UserName";
                var chatUsers = connection.Query<ChatUser>(cmd, new { UserName = userName }).ToList();
                chatUser = chatUsers.Count == 0 ? null : chatUsers[0];
            }
            return chatUser;
        }

        /// <summary>
        /// 新增一個聊天使用者到chat_users資料表
        /// </summary>
        /// <param name="chatUser"></param>
        /// <returns>帶有ID的聊天使用者</returns>
        public ChatUser InsertChatUser(ChatUser chatUser)
        {
            string cmd = @"
INSERT INTO chat_users (user_name, password) VALUES (@UserName, @Password);
SELECT seq FROM sqlite_sequence WHERE name = 'chat_users'
";
            using (var connection = new SqliteConnection(connectionString))
            {
                var id = connection.ExecuteScalar<int>(cmd, chatUser);
                chatUser.Id = id;
            }
            return chatUser;
        }
    }
}
