using MQTTDataAccessLib.Json;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace MQTTDataAccessLib.Models.DataTypes
{
    /// <summary>
    /// 密碼，專門處理任密碼以及相關資訊的Model<br/>
    /// 此model使用ToStringJsonConverter
    /// </summary>
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class Password
    {
        private readonly string password;

        public const int MaxLength = 30;

        public const int MinLength = 6;

        /// <summary>
        /// 初始化密碼，若格式不合會拋出ArgumentException
        /// </summary>
        /// <param name="password">密碼</param>
        public Password(string password)
        {
            if (!IsValid(password))
                throw new ArgumentException($"Invalid Password {password}.");

            this.password = password;
        }

        /// <summary>
        /// 檢查Password是否合法
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public static bool IsValid(string candidate)
        {
            if (String.IsNullOrWhiteSpace(candidate))
                return false;
            if (candidate.Length > MaxLength || candidate.Length < MinLength)
                return false;
            /*Regex regex = new Regex(regexConstraints);
            if (regex.IsMatch(candidate))
                return false;*/

            return true;
        }

        /// <summary>
        /// 嘗試將字串轉換成密碼
        /// </summary>
        /// <param name="candidate">要轉換成密碼的字串</param>
        /// <param name="password">密碼</param>
        /// <returns>若轉換成功則回傳true，反之為false</returns>
        public static bool TryParse(string candidate, out Password password)
        {
            password = null;
            if (!IsValid(candidate))
                return false;

            password = new Password(candidate);

            return true;
        }

        /// <summary>
        /// 轉換成字串的operator(conversion operator)
        /// </summary>
        /// <param name="password"></param>
        public static implicit operator string(Password password)
        {
            return password.password;
        }

        public override string ToString()
        {
            return this.password;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Password;
            if (other == null)
                return base.Equals(obj);

            return object.Equals(this.password, other.password);
        }

        public override int GetHashCode()
        {
            return this.password.GetHashCode();
        }
    }
}
