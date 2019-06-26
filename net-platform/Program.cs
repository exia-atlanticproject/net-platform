using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using net_platform.Mqtt;

namespace net_platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*Thread t = new Thread(CreateWebHostBuilder(args).Build().Run);
            t.Start();*/

            Thread t2 = new Thread(LaunchMqttServer);
            t2.Start();
            
            BrokerClient client = new BrokerClient();
            client.setBrokerClient();

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            /*
            Thread.Sleep(2000);
            client.sendBrokerMessage("device/telemetry", "Nique tout");*/
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