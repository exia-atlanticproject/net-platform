using System.Collections.Generic;
using System.Threading;
using net_platform.Calculation;
using net_platform.Mqtt;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CalculationTest
    {
        private CalculationEngine _engine;
        
        public CalculationTest()
        {
            int subMqttComponent = 2;
            new BrokerClient(subMqttComponent);
            _engine = new CalculationEngine();
        }

        [Test]
        public void TestFullJobResults()
        {
            string message = "{\"payload\":{\"userId\": 256,\"nameJob\": \"First job!\",\"startJob\": \"2019-01-01 00:00:00\",\"endJob\": \"2019-06-30 00:00:00\",\"granularity\": \"month\",\"calculType\": \"average\",\"dataSet\": [{\"metricDate\": \"2019-01-21 00:00:00\",\"metricValue\": \"125.23658\"},{\"metricDate\": \"2019-02-21 00:00:00\",\"metricValue\": \"2365.2874563\"},{\"metricDate\": \"2019-03-21 00:00:00\",\"metricValue\": \"12.68451321\"},{\"metricDate\": \"2019-04-21 00:00:00\",\"metricValue\": \"2365.2874563\"},{\"metricDate\": \"2019-05-21 00:00:00\",\"metricValue\": \"2365.2874563\"},{\"metricDate\": \"2019-06-21 00:00:00\",\"metricValue\": \"2365.2874563\"}]}}";
            _engine.notify(message);
            Thread.Sleep(2000);
            int lastId = DbConnector._lastId;
            List<double> testList = DbConnector.getCalculById(lastId);
            
            List<double> finalList = new List<double>();
            finalList.Add(125.23658);
            finalList.Add(2365.2874563);
            finalList.Add(12.68451321);
            finalList.Add(2365.2874563);
            finalList.Add(2365.2874563);
            finalList.Add(2365.2874563);
            
            Assert.That(testList, Is.EquivalentTo(finalList));
        }

        [Test]
        public void TestAverage()
        {
            List<double> dataList = new List<double>();
            dataList.Add(222.3441);
            dataList.Add(2094.2113);
            dataList.Add(132424.232424);
            dataList.Add(13420249.23);
            dataList.Add(2324);
            dataList.Add(121.2);
            dataList.Add(240924.2);

            int count = 0;
            double final = 0;
            foreach (double data in dataList)
            {
                count++;
                final += data;
            }
            final = final / count;

            double testValue = _engine.computeAverage(dataList);

            Assert.AreEqual(final, testValue);
        }
        [Test]
        public void TestMedian()
        {
            List<double> dataList = new List<double>();
            dataList.Add(222.3441);
            dataList.Add(2094.2113);
            dataList.Add(132424.232424);
            dataList.Add(13420249.23);
            dataList.Add(2324);
            dataList.Add(121.2);
            dataList.Add(240924.2);

            int dataLength = dataList.Count;
            dataLength = dataLength / 2;

            double testValue = _engine.computeMedian(dataList);

            Assert.AreEqual(dataList[dataLength], testValue);
        }
        [Test]
        public void TestMax()
        {
            List<double> dataList = new List<double>();
            dataList.Add(222.3441);
            dataList.Add(2094.2113);
            dataList.Add(132424.232424);
            dataList.Add(13420249.23);
            dataList.Add(2324);
            dataList.Add(121.2);
            dataList.Add(240924.2);

            double max = 0;
            foreach (var data in dataList)
            {
                if (data > max) max = data;
            }

            double testValue = _engine.computeMax(dataList);

            Assert.AreEqual(max, testValue);
        }
        [Test]
        public void TestMin()
        {
            List<double> dataList = new List<double>();
            dataList.Add(222.3441);
            dataList.Add(2094.2113);
            dataList.Add(132424.232424);
            dataList.Add(13420249.23);
            dataList.Add(2324);
            dataList.Add(121.2);
            dataList.Add(240924.2);

            double min = 9999999999999;
            foreach (var data in dataList)
            {
                if (data < min) min = data;
            }

            double testValue = _engine.computeMin(dataList);

            Assert.AreEqual(min, testValue);
        }

        [Test]
        public void TestGranularityProcess()
        {
            string message = "{\"userId\": 256,\"nameJob\": \"First job!\",\"startJob\": \"2019-01-01 00:00:00\",\"endJob\": \"2019-06-30 00:00:00\",\"granularity\": \"month\",\"calculType\": \"average\",\"dataSet\": [{\"metricDate\": \"2019-01-21 00:00:00\",\"metricValue\": \"125.23658\"},{\"metricDate\": \"2019-02-21 00:00:00\",\"metricValue\": \"2365.2874563\"},{\"metricDate\": \"2019-03-21 00:00:00\",\"metricValue\": \"12.68451321\"},{\"metricDate\": \"2019-04-21 00:00:00\",\"metricValue\": \"2365.2874563\"},{\"metricDate\": \"2019-05-21 00:00:00\",\"metricValue\": \"2365.2874563\"},{\"metricDate\": \"2019-06-21 00:00:00\",\"metricValue\": \"2365.2874563\"}]}";
            dynamic jsonMessage = JObject.Parse(message);
            List<List<double>> testList = _engine.dataSetByGranularity(jsonMessage);
            
            List<List<double>> finalList = new List<List<double>>();
            
            List<double> first = new List<double>();
            first.Add(125.23658);
            finalList.Add(first);
            
            List<double> scnd = new List<double>();
            scnd.Add(2365.2874563);
            finalList.Add(scnd);

            List<double> third = new List<double>();
            third.Add(12.68451321);
            finalList.Add(third);
            
            List<double> fourth = new List<double>();
            fourth.Add(2365.2874563);
            finalList.Add(fourth);
            
            List<double> fifth = new List<double>();
            fifth.Add(2365.2874563);
            finalList.Add(fifth);
            
            List<double> sixth = new List<double>();
            sixth.Add(2365.2874563);
            finalList.Add(sixth);
            
            Assert.That(finalList, Is.EquivalentTo(testList));
        }
    }
}