
using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using net_platform.Calculation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace net_platform.Mqtt
{
    public class MqttServer : IBrokerAction
    {
        private static IMqttServer _mqttServer;

        public MqttServer()
        {
            string topic = "command";
            BrokerClient.addNewListener(this, topic);
        }

        public async void setupServer()
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithApplicationMessageInterceptor(context =>
                {
                    Task.Run(() =>
                    {
                        if (context.ApplicationMessage.Topic.Contains("device") &&
                            context.ApplicationMessage.Topic.Contains("telemetry"))
                        {
                            dynamic jsonRequest = JObject.Parse(Encoding.UTF8.GetString(context.ApplicationMessage.Payload));
                            processMqttMessage(jsonRequest, context.ClientId);
                        }
                    });
                })
                .WithDefaultEndpointPort(1883);
            
            _mqttServer = new MqttFactory().CreateMqttServer();
            await _mqttServer.StartAsync(optionsBuilder.Build());
            
            Console.WriteLine("MQTT server running");
        }

        public static void processMqttMessage(dynamic payload, string clientId)
        {
            MqttDataSet dataSet = new MqttDataSet();
            dataSet.deviceId = clientId;
            dataSet.metricDate = payload.metricDate.ToString();
            dataSet.metricValue = payload.metricValue.ToString();
            dataSet.deviceType = payload.deviceType.ToString();
            dataSet.macAddress= payload.macAddress.ToString();
            dataSet.deviceName= payload.name.ToString();
            string jsonDataSet = JsonConvert.SerializeObject(dataSet);
            jsonDataSet = "{\"action\": \"telemetry\",\"source\": \"\",\"callback\": \"\",\"payload\": " + jsonDataSet + "}";
            Console.WriteLine(jsonDataSet);
            BrokerClient.sendMqttMessage(jsonDataSet, "Data-Controller");
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
                    data.deviceId = idDevice;
                    string finalJson = JsonConvert.SerializeObject(data);
                    
                    return Encoding.UTF8.GetBytes(finalJson);
                }
            }
            return Encoding.UTF8.GetBytes("null");
        }
        
        public void notify(string brokerResponse)
        {
            dynamic jsonRequest = JObject.Parse(brokerResponse);
            forwardCommand(jsonRequest.payload.idDevice.ToString(), (int) jsonRequest.payload.idCommand);
        }

        private void forwardCommand(string deviceId, int command)
        {
            string strCommand = "{\"command\": " + command + "}";
            string topic = "device/command/" + deviceId;
            Console.WriteLine("Publishing on " + topic + " with payload " + strCommand);
            sendMqttMessage(strCommand, topic);
        }
    }

    public class MqttDataSet
    {
        public string deviceId;
        public string metricDate;
        public string metricValue;
        public string deviceType;
        public string macAddress;
        public string deviceName;
    }
}