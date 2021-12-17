using System;
using System.Collections.Generic;
using System.Text;

namespace MQTTDataAccessLib.DAOs
{
    /// <summary>
    /// 初始化資料庫的DAO介面
    /// </summary>
    public interface IChatServerSchemaDao
    {
        /// <summary>
        /// 初始化Schema
        /// </summary>
        void InitializeSchema();
    }
}
