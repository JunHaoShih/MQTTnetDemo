using System.Linq;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MQTTBrokerAspDotNetWebSocket.Models.Settings;
using MQTTBrokerAspDotNetWebSocket.MQTT;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.Extensions;
using Newtonsoft.Json;

namespace MQTTBrokerAspDotNetWebSocket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // 取得AppSettings並加入並作為singleton加入service
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>(c => c.BindNonPublicProperties = true);
            services.AddSingleton(appSettings);

            // 加入CORS
            services.AddCors();

            // 將MQTTChatService物件作為singleton加入service
            services.AddSingleton<MQTTChatService>();

            // 加入 MQTT Service
            services
                .AddHostedMqttServerWithServices(aspNetMqttServerOptionsBuilder =>
                {
                    var mqttService = aspNetMqttServerOptionsBuilder.ServiceProvider.GetRequiredService<MQTTChatService>();
                    // 利用MQTTChatService去Build IMqttServerOptions
                    mqttService.BuildMqttServerOptions(aspNetMqttServerOptionsBuilder);
                })
                .AddMqttConnectionHandler()
                .AddConnections();

            services.AddControllers();
        }

        /// <summary>
        /// 這是Autofac註冊關聯的地方，註冊規則寫在ServiceModule內
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your own things directly with Autofac here. Don't
            // call builder.Populate(), that happens in AutofacServiceProviderFactory
            // for you.
            builder.RegisterModule(new ServiceModule(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 設定CORS
            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // 設定EndPoints
            app.UseEndpoints(endpoints =>
            {
                // Setup EndPoints for Controller actions
                endpoints.MapControllers();
                // 將MQTT的連線對應到"/mqtt"路徑
                endpoints.MapConnectionHandler<MqttConnectionHandler>(
                    "/mqtt",
                    httpConnectionDispatcherOptions =>
                        httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                            protocolList => protocolList.FirstOrDefault() ?? string.Empty);
            });

            // 用MQTTChatService去設定IMqttServer
            app.UseMqttServer(server =>
                app.ApplicationServices.GetRequiredService<MQTTChatService>().ConfigureMqttServer(server));
        }
    }
}
