using Dapper;
using MQTTDataAccessLib.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MQTTDataAccessLib.DAOs.TypeHandlers
{
    /// <summary>
    /// 給Dapper知道如何轉換Password的TypeHandler
    /// </summary>
    public class PasswordTypeHandler : SqlMapper.TypeHandler<Password>
    {
        public override Password Parse(object value)
        {
            return new Password((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, Password value)
        {
            parameter.Value = value.ToString();
        }
    }
}
