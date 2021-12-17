using MQTTDataAccessLib.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.Models
{
    /// <summary>
    /// 聊天室訊息
    /// </summary>
    public class ChatRoomMessage
    {
        /// <summary>
        /// 聊天室訊息
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Topic
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public UserName UserName { get; set; }

        /// <summary>
        /// 聊天文字
        /// </summary>
        public ChatText ChatMessage { get; set; }

        /// <summary>
        /// 轉換成聊天室的字串
        /// </summary>
        /// <returns></returns>
        public string ToChatString()
        {
            return UserName + ": " + ChatMessage.ToString();
        }
    }
}
