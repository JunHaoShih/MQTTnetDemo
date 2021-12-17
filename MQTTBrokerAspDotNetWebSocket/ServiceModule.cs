using Autofac;
using Microsoft.Extensions.Configuration;
using MQTTDataAccessLib.DAOs;
using MQTTDataAccessLib.DAOs.Implements;
using MQTTDataAccessLib.DAOs.TypeHandlers;
using MQTTDataAccessLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MQTTBrokerAspDotNetWebSocket
{
    public class ServiceModule : Module
    {
        IConfiguration Configuration { get; set; }

        /// <summary>
        /// 為了要取得appsettings而建立的constructor
        /// </summary>
        /// <param name="configuration"></param>
        public ServiceModule(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 請在這個Method裡面註冊類型
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            var sqlConnectionStr = Configuration.GetConnectionString("Sqlite");

            // 將Dapper改成底線模式
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            // 替Dapper加入我們定義的TypeHandler
            Dapper.SqlMapper.AddTypeHandler(new UserNameTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new PasswordTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new ChatTextTypeHandler());

            // 註冊Dao
            builder.RegisterType<SqliteChatUserDao>().As<IChatUserDao>().WithParameter(new TypedParameter(typeof(string), sqlConnectionStr));
            builder.RegisterType<SqliteChatRoomMessageDao>().As<IChatRoomMessageDao>().WithParameter(new TypedParameter(typeof(string), sqlConnectionStr));

            IChatServerSchemaDao chatServerSchemaDao = new SqliteChatServerSchemaDao(sqlConnectionStr);
            chatServerSchemaDao.InitializeSchema();
        }
    }
}
