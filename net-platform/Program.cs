using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using net_platform.Mqtt;

namespace net_platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread t = new Thread(CreateWebHostBuilder(args).Build().Run);
            t.Start();
            
            Thread t2 = new Thread(LaunchMqttServer);
            t2.Start();
            
            /*MqttClient client = new MqttClient();
            client.setMqttClient();*/
        }

        public static void LaunchMqttServer()
        {
            MqttServer server = new MqttServer();
            server.setupServer();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}