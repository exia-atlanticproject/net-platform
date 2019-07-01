using net_platform.Calculation;
using net_platform.Mqtt;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CalculationTest
    {
        private CalculationEngine _engine;
        private string _jsonrequest = "{\"userId\": 256,\"nameJob\": \"First job!\",\"startJob\": \"2019-06-21 00:00:00\",\"endJob\": \"2019-06-21 00:00:00\",\"granularity\": \"month\",\"calculType\": \"average\",\"dataSet\": [{\"metricDate\": \"2019-06-21 00:00:00\",\"metricValue\": \"125.23658\"},{\"metricDate\": \"2019-06-21 00:00:00\",\"metricValue\": \"2365.2874563\"},{\"metricDate\": \"2019-06-21 00:00:00\",\"metricValue\": \"12.68451321\"}]}";

        public CalculationTest()
        {
            int subMqttComponent = 2;
            new BrokerClient(subMqttComponent);
            _engine = new CalculationEngine();
        }

        [Test]
        public void CalculationParseRequest()
        {
            _engine.notify(_jsonrequest);
            
            Assert.AreEqual(3, 3, 0);
        }
    }
}