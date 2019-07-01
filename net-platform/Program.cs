using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using net_platform.Calculation;
using net_platform.Mqtt;

namespace net_platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int subMqttComponent = 2;
            BrokerClient client = new BrokerClient(subMqttComponent);
            
            Thread t = new Thread(CreateWebHostBuilder(args).Build().Run);
            t.Start();

            Thread t2 = new Thread(LaunchMqttServer);
            t2.Start();
            
            Thread t3 = new Thread(LaunchCalculEngine);
            t3.Start();
            
            //Thread.Sleep(4000);

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
            /*
            
            client.sendBrokerMessage("device/telemetry", "Nique tout");*/
        }

        public static void LaunchCalculEngine()
        {
            CalculationEngine engine = new CalculationEngine();
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