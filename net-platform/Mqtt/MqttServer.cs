
using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using Newtonsoft.Json;

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
                    Task.Run(() =>
                    {
                        transfertMqttRequest(context.ApplicationMessage.Topic, context.ApplicationMessage.Payload);
                    });
                    //context.ApplicationMessage.Payload = Encoding.UTF8.GetBytes("The server injected payload.");
                })
                .WithDefaultEndpointPort(1883);
            
            _mqttServer = new MqttFactory().CreateMqttServer();
            await _mqttServer.StartAsync(optionsBuilder.Build());
            
            Console.WriteLine("MQTT server running");
        }

        public void transfertMqttRequest(string topic, byte[] message)
        {
            byte[] bytesPayload = processMqttMessage(topic, message);
            string finalPayload = Encoding.UTF8.GetString(bytesPayload);
            if(finalPayload != "null") sendMqttMessage(finalPayload, "device/telemetry");
        }

        public async static void sendMqttMessage(string body, string topic)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(body)
                .WithAtMostOnceQoS()
                .Build();

            await _mqttServer.PublishAsync(message);
        }

        public byte[] processMqttMessage(string topic, byte[] message)
        {
            string[] urlMembers = topic.Split("/");
            if (urlMembers.Length == 3 || urlMembers.Length == 4)
            {
                if(urlMembers[0] == "device" && urlMembers[2] == "telemetry")
                {
                    string idDevice = urlMembers[1];
                    string jsonData = Encoding.UTF8.GetString(message);
                    MqttDataSet data = JsonConvert.DeserializeObject<MqttDataSet>(jsonData);
                    data.clientId = idDevice;
                    string finalJson = JsonConvert.SerializeObject(data);
                    
                    return Encoding.UTF8.GetBytes(finalJson);
                }
            }
            return Encoding.UTF8.GetBytes("null");
        }
    }

    public class MqttDataSet
    {
        public string name;
        public string macAddress;
        public string metricDate;
        public string deviceType;
        public string metricValue;
        public string clientId;
    }
}