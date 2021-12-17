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
            // ���oAppSettings�å[�J�ç@��singleton�[�Jservice
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>(c => c.BindNonPublicProperties = true);
            services.AddSingleton(appSettings);

            // �[�JCORS
            services.AddCors();

            // �NMQTTChatService����@��singleton�[�Jservice
            services.AddSingleton<MQTTChatService>();

            // �[�J MQTT Service
            services
                .AddHostedMqttServerWithServices(aspNetMqttServerOptionsBuilder =>
                {
                    var mqttService = aspNetMqttServerOptionsBuilder.ServiceProvider.GetRequiredService<MQTTChatService>();
                    // �Q��MQTTChatService�hBuild IMqttServerOptions
                    mqttService.BuildMqttServerOptions(aspNetMqttServerOptionsBuilder);
                })
                .AddMqttConnectionHandler()
                .AddConnections();

            services.AddControllers();
        }

        /// <summary>
        /// �o�OAutofac���U���p���a��A���U�W�h�g�bServiceModule��
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

            // �]�wCORS
            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // �]�wEndPoints
            app.UseEndpoints(endpoints =>
            {
                // Setup EndPoints for Controller actions
                endpoints.MapControllers();
                // �NMQTT���s�u������"/mqtt"���|
                endpoints.MapConnectionHandler<MqttConnectionHandler>(
                    "/mqtt",
                    httpConnectionDispatcherOptions =>
                        httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                            protocolList => protocolList.FirstOrDefault() ?? string.Empty);
            });

            // ��MQTTChatService�h�]�wIMqttServer
            app.UseMqttServer(server =>
                app.ApplicationServices.GetRequiredService<MQTTChatService>().ConfigureMqttServer(server));
        }
    }
}
