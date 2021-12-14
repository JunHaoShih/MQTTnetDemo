using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.Data
{
    /// <summary>
    /// 聊天訊息
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 聊天訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 轉換成聊天室的字串
        /// </summary>
        /// <returns></returns>
        public string ToChatString()
        {
            return UserName + ": " + Message;
        }
    }
}
