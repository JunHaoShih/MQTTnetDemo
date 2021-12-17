using MQTTDataAccessLib.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.DAOs
{
    /// <summary>
    /// 聊天室訊息的DAO介面
    /// </summary>
    public interface IChatRoomMessageDao
    {
        /// <summary>
        /// Insert一筆聊天室訊息
        /// </summary>
        /// <param name="chatRoomMessage">聊天室訊息</param>
        /// <returns>帶有ID的聊天室訊息</returns>
        ChatRoomMessage InsertChatRoomMessage(ChatRoomMessage chatRoomMessage);
    }
}
