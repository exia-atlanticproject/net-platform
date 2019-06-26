using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace net_platform.Mqtt
{
    public class BrokerClient
    {
        private static IMqttClient _mqttClient;
        public async void setBrokerClient()
        {
            var factory = new MqttFactory();
            var _mqttClient = factory.CreateMqttClient();
            
            _mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("my/topic").Build());

                Console.WriteLine("### SUBSCRIBED ###");
            });

            var options = new MqttClientOptionsBuilder()
                .WithClientId("Client1")
                .WithTcpServer("10.154.128.153", 1883)
                .WithCleanSession();
            
            try
            {
                await _mqttClient.ConnectAsync(options.Build());
                Console.WriteLine("Client created!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        
        /*public static MqttClient _mqttClient;
        public static string[] _topics;
        public static IBrokerAction[] _topicsObjects;

        public void setBrokerClient()
        {
            MqttClient _mqttClient = new MqttClient("10.154.128.153");
            //byte code = client.Connect(Guid.NewGuid().ToString());
            _mqttClient.Connect(Guid.NewGuid().ToString());
            _mqttClient.MqttMsgSubscribed += client_MqttMsgSubscribed;
            _mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            ushort msgId = _mqttClient.Subscribe(new string[] { "device/telemetry"}, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Received = " + Encoding.UTF8.GetString(e.Message) + " on topic " + e.Topic);
        }
        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Subscribed for id = " + e.MessageId);
        }
        public void sendBrokerMessage(string topic, string message)
        {
            _mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }*/
    }
}