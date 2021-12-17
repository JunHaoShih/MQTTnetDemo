using MQTTDataAccessLib.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.Models.DataTypes
{
    /// <summary>
    /// 聊天文字，專門處理任聊天文字以及相關資訊的Model<br/>
    /// 此model使用ToStringJsonConverter
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class ChatText
    {
        private readonly string chatText;

        public const int MaxLength = 255;

        public const int MinLength = 1;

        /// <summary>
        /// 初始化聊天文字，若格式不合會拋出ArgumentException
        /// </summary>
        /// <param name="chatMessage"></param>
        public ChatText(string chatMessage)
        {
            if (!IsValid(chatMessage))
                throw new ArgumentException($"Invalid ChatMessage {chatMessage}.");

            this.chatText = chatMessage;
        }

        /// <summary>
        /// 檢查聊天文字是否合法
        /// </summary>
        /// <param name="candidate">聊天文字</param>
        /// <returns></returns>
        public static bool IsValid(string candidate)
        {
            if (String.IsNullOrWhiteSpace(candidate))
                return false;
            if (candidate.Length > MaxLength || candidate.Length < MinLength)
                return false;

            return true;
        }

        /// <summary>
        /// 嘗試轉換成聊天文字
        /// </summary>
        /// <param name="candidate">要轉換的聊天文字</param>
        /// <param name="chatMessage">聊天文字物件</param>
        /// <returns></returns>
        public static bool TryParse(string candidate, out ChatText chatMessage)
        {
            chatMessage = null;
            if (!IsValid(candidate))
                return false;
            chatMessage = new ChatText(candidate);
            return true;
        }

        /// <summary>
        /// 轉換成字串的operator(conversion operator)
        /// </summary>
        /// <param name="chatMessage"></param>
        public static implicit operator string(ChatText chatMessage)
        {
            return chatMessage.chatText;
        }

        public override string ToString()
        {
            return this.chatText;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ChatText;
            if (other == null)
                return base.Equals(obj);

            return object.Equals(this.chatText, other.chatText);
        }

        public override int GetHashCode()
        {
            return this.chatText.GetHashCode();
        }
    }
}
