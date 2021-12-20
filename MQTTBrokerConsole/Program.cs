using Autofac;
using MQTTBrokerConsole.MQTT;
using MQTTDataAccessLib.DAOs;
using MQTTDataAccessLib.DAOs.Implements;
using MQTTDataAccessLib.DAOs.TypeHandlers;
using System;

namespace MQTTBrokerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            /*MQTTChatHandler mQTTHandler = new MQTTChatHandler();
            mQTTHandler.Start();
            Console.WriteLine("Hello World!");*/
            CompositionRoot().Resolve<MQTTApplication>().Run();
        }

        private static IContainer CompositionRoot()
        {
            var sqlConnectionStr = "data source=MqttChatServer.sqlite";

            // 將Dapper改成底線模式
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            // 替Dapper加入我們定義的TypeHandler
            Dapper.SqlMapper.AddTypeHandler(new UserNameTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new PasswordTypeHandler());
            Dapper.SqlMapper.AddTypeHandler(new ChatTextTypeHandler());

            var builder = new ContainerBuilder();
            builder.RegisterType<MQTTApplication>();
            builder.RegisterType<MQTTChatService>();
            //builder.RegisterType<SqliteChatServerSchemaDao>().As<IChatServerSchemaDao>().WithParameter(new TypedParameter(typeof(string), sqlConnectionStr));
            // 註冊Dao
            builder.RegisterType<SqliteChatUserDao>().As<IChatUserDao>().WithParameter(new TypedParameter(typeof(string), sqlConnectionStr));
            builder.RegisterType<SqliteChatRoomMessageDao>().As<IChatRoomMessageDao>().WithParameter(new TypedParameter(typeof(string), sqlConnectionStr));

            // initialize
            IChatServerSchemaDao chatServerSchemaDao = new SqliteChatServerSchemaDao(sqlConnectionStr);
            chatServerSchemaDao.InitializeSchema();

            return builder.Build();
        }
    }
}
