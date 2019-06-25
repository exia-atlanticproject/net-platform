using System;
using System.Text;
using net_platform.Mqtt;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MqttTest
    {
        private readonly MqttServer _mqttServer;

        public MqttTest()
        {
            _mqttServer = new MqttServer();
        }
        [Test]
        public void TestProcessMqttMessage()
        {
            DeviceData device = new DeviceData();
            string json = JsonConvert.SerializeObject(device);
            byte[] payload = Encoding.UTF8.GetBytes(json);
            
            BrokerData broker = new BrokerData();
            string jsonBroker = JsonConvert.SerializeObject(broker);
            byte[] payloadBroker = Encoding.UTF8.GetBytes(jsonBroker);
            
            string topic = "/device/TheDeviceId";
            
            byte[] result = _mqttServer.processMqttMessage(topic, payload);

            StringAssert.Contains(Encoding.UTF8.GetString(payloadBroker), Encoding.UTF8.GetString(result));
        }
    }

    public class DeviceData
    {
        public string name = "humiditySensor_wealthy-snails";
        public string macAddress = "44:81:C0:0D:6C:E3";
        public string metricDate = "2019-05-25T16:40:13Z";
        public string deviceType = "humiditySensor";
        public string metricValue = "0";
    }

    public class BrokerData
    {
        public string clientId = "TheDeviceId";
        public string name = "humiditySensor_wealthy-snails";
        public string macAddress = "44:81:C0:0D:6C:E3";
        public string metricDate = "2019-05-25T16:40:13Z";
        public string deviceType = "humiditySensor";
        public string metricValue = "0";
    }
}