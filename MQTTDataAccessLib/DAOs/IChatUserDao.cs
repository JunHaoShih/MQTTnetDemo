using MQTTDataAccessLib.Models;
using MQTTDataAccessLib.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.DAOs
{
    /// <summary>
    /// 聊天使用者的DAO 介面
    /// </summary>
    public interface IChatUserDao
    {
        /// <summary>
        /// 用id取聊天使用者
        /// </summary>
        /// <param name="id">聊天使用者id</param>
        /// <returns></returns>
        ChatUser GetChatUser(int id);

        /// <summary>
        /// 用使用者名稱取聊天使用者(使用者名稱是唯一值)
        /// </summary>
        /// <param name="userName">使用者名稱</param>
        /// <returns></returns>
        ChatUser GetChatUser(UserName userName);

        /// <summary>
        /// Insert一個聊天使用者
        /// </summary>
        /// <param name="chatUser">聊天使用者</param>
        /// <returns></returns>
        ChatUser InsertChatUser(ChatUser chatUser);
    }
}
