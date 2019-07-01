using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;

namespace net_platform.Mqtt
{
    public class BrokerClient
    {
        private static IMqttClient _mqttClient;
        private static IBrokerAction[] _componentListener;
        private static string[] _topicListened;
        private static int _comptListener = 0;
        private static int _totalListener;

        public BrokerClient(int totalListener)
        {
            _componentListener = new IBrokerAction[totalListener];
            _topicListened = new string[totalListener];
            _totalListener = totalListener;
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
        }
        public static async void setBrokerClient()
        {
            Console.WriteLine("Setting up client");
            
            _mqttClient.UseConnectedHandler(async e =>
            {
                int comptTest = 0;
                foreach (string topic in _topicListened)
                {
                    // Subscribe to a topic
                    Console.WriteLine("New subscriber");
                    Console.WriteLine(topic);
                    await _mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(topic).Build());
                    comptTest++;
                }
            });
            
            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                int comptTest = 0;
                foreach (string topic in _topicListened)
                {
                    if (topic == e.ApplicationMessage.Topic)
                    {
                        _componentListener[comptTest].notify(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                    }
                    comptTest++;
                }
            });

            var options = new MqttClientOptionsBuilder()
                .WithClientId(Guid.NewGuid().ToString())
                //.WithTcpServer("localhost", 1883)
                .WithTcpServer("10.154.128.62", 1883)
                .WithCredentials("service", "safepw")
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

        public static void addNewListener(IBrokerAction component, string topic)
        {
            _componentListener[_comptListener] = component;
            _topicListened[_comptListener] = topic;

            _comptListener ++;
            if(_comptListener == _totalListener) setBrokerClient();
        }

        public static async void sendMqttMessage(string message, string topic)
        {
            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(message)
                .WithAtMostOnceQoS()
                .Build();

            await _mqttClient.PublishAsync(mqttMessage);
        }
    }
}