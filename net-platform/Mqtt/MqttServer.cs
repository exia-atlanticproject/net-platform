
using System;
using System.Text;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace net_platform.Mqtt
{
    public class MqttServer
    {
        private static IMqttServer _mqttServer;
        public MqttServer() {}

        public async void setupServer()
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithApplicationMessageInterceptor(context =>
                {
                    processMqttMessage(context.ApplicationMessage.Topic, context.ApplicationMessage.Payload);
                    //context.ApplicationMessage.Payload = Encoding.UTF8.GetBytes("The server injected payload.");
                })
                .WithDefaultEndpointPort(1883);
            
            _mqttServer = new MqttFactory().CreateMqttServer();
            await _mqttServer.StartAsync(optionsBuilder.Build());
            Console.WriteLine("MQTT server running");
        }

        public byte[] processMqttMessage(string topic, byte[] message)
        {
            string[] urlMembers = topic.Split("/");
            if (urlMembers.Length == 1)
            {
                if(urlMembers[0] == "device")
                {
                    
                }
            }
        }
    }
}