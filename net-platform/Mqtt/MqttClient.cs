using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace net_platform.Mqtt
{
    public class MqttClient
    {
        private static IMqttClient _mqttClient;
        public MqttClient() {}

        public async void setMqttClient()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId("c#ClientTest1")
                .WithTcpServer("test.mosquitto.org", 1883)
                .WithTls()
                .Build();
            await mqttClient.ConnectAsync(options, token);

            _mqttClient = mqttClient;
        }
    }
}