using MQTTDataAccessLib.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.Models
{
    /// <summary>
    /// 聊天使用者
    /// </summary>
    public class ChatUser
    {
        /// <summary>
        /// 使用者ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public UserName UserName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public Password Password { get; set; }
    }
}
