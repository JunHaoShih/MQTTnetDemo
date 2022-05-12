using Autofac;
using MQTTChatClient.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQTTChatClient
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            /*var view = new MainForm();
            _ = new MainPresenter(view);
            Application.Run(view);*/
            Application.Run(CompositionRoot().Resolve<MainForm>());
        }

        private static IContainer CompositionRoot()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainForm>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<MqttService>().As<IMqttService>().InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
