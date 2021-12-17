

using Newtonsoft.Json;
using System;
using System.Reflection;

namespace MQTTDataAccessLib.Json
{
    /// <summary>
    /// 使用ToString的JsonConverter，使用此JsonConverter的class一定要實作TryParse
    /// </summary>
    public class ToStringJsonConverter : JsonConverter
    {
        /// <summary>
        /// 此method決定Newtonsoft.JsonConvert會不會去用ToStringJsonConverter將物件轉換成json string<br/>
        /// 所以一定要return true
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        /// <summary>
        /// 因為我們要用ToString()當作json值，所以直接回傳value.ToString()
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        /// <summary>
        /// 此method決定Newtonsoft.JsonConvert會不會去用ToStringJsonConverter將json string轉換成物件<br/>
        /// 因為我們要把json string轉換成物件，所以return true
        /// </summary>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// 實作json string轉換成物件的方法<br/>
        /// 取得TryParse的method後將json string丟入做轉換
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            MethodInfo parse = objectType.GetMethod("TryParse");
            if (parse != null && parse.IsStatic && parse.ReturnType == typeof(bool))
            {
                object[] parameters = new object[] { (string)reader.Value, null };
                var isValid = parse.Invoke(null, parameters);
                return parameters[1];
            }

            throw new JsonException(string.Format(
                "The {0} type does not have a public static Parse(string) method that returns a {0}.",
                objectType.Name));
        }
    }
}
