using Dapper;
using MQTTDataAccessLib.Models.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MQTTDataAccessLib.DAOs.TypeHandlers
{
    /// <summary>
    /// 給Dapper知道如何轉換UserName的TypeHandler
    /// </summary>
    public class UserNameTypeHandler : SqlMapper.TypeHandler<UserName>
    {
        public override UserName Parse(object value)
        {
            return new UserName((string)value);
        }

        public override void SetValue(IDbDataParameter parameter, UserName value)
        {
            parameter.Value = value.ToString();
        }
    }
}
