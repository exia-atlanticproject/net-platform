using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using net_platform.Mqtt;
using Newtonsoft.Json.Linq;

namespace net_platform.Calculation
{
    public class CalculationEngine : IBrokerAction
    {
        public static int _nbRequest = 1;
        private static string _target;
        public static int timeStart;
        public CalculationEngine()
        {
            _target = Guid.NewGuid().ToString();
            string topic = "calculation";
            BrokerClient.addNewListener(this, topic);
        }
        public int testFunction(int num, int num2)
        {
            return num + num2;
        }

        private void parseRequest(string request)
        {
            dynamic jsonRequest = JObject.Parse(request);
            jsonRequest = jsonRequest.payload;
            List<List<double>> computedDataSet = dataSetByGranularity(jsonRequest);
            List<double> finalResults = executeRequestedCalcul(computedDataSet, jsonRequest.calculType.ToString());
            createDbCalcul(finalResults, jsonRequest);
        }

        private void createDbCalcul(List<double> results, dynamic payload)
        {
            string name = payload.nameJob.ToString();
            string start = payload.startJob.ToString();
            string end = payload.endJob.ToString();
            string granularity = payload.granularity.ToString();
            string idUser = payload.userId.ToString();
            string calculType = payload.calculType.ToString();
            if (!DbConnector.verifyUser(idUser))
            {
                DbConnector.insertUser(idUser);
            }
            int idJob= DbConnector.createJob(name, start, end, idUser, granularity, calculType);

            foreach (double result in results)
            {
                DbConnector.addResult(idJob, result);
            }
        }
        private List<double> executeRequestedCalcul(List<List<double>> computedDataSet, string type)
        {
            List<double> finalResults = new List<double>();
            foreach (List<double> dataList in computedDataSet)
            {
                double computeResult = 0;
                switch (type)
                {
                    case "average":
                        computeResult = computeAverage(dataList);
                        break;
                    case "median":
                        computeResult = computeMedian(dataList);
                        break;
                    case "max":
                        computeResult = computeMax(dataList);
                        break;
                    case "min":
                        computeResult = computeMin(dataList);
                        break;
                    case "deviation":
                        computeResult = computeDeviation(dataList);
                        break;
                }
                finalResults.Add(computeResult);
            }

            return finalResults;
        }

        public double computeAverage(List<double> dataList)
        {
            int iData = 0;
            double output = 0;
            foreach (double data in dataList)
            {
                iData++;
                output += data;
            }

            return output / iData;
        }

        public double computeMedian(List<double> dataList)
        {
            int dataLength = dataList.Count;
            dataLength = dataLength / 2;
            
            return dataList[dataLength];
        }

        public double computeMax(List<double> dataList)
        {
            double output = 0;
            foreach (double data in dataList)
            {
                if (data > output) output = data;
            }

            return output;
        }

        public double computeMin(List<double> dataList)
        {
            double output = 9999999999999999999;
            foreach (double data in dataList)
            {
                if (data < output) output = data;
            }

            return output;
        }

        public double computeDeviation(List<double> dataList)
        {
            double moyenne = computeAverage(dataList);
            double somme =0.0;
            for (int i=0; i<dataList.Count; i++) 
            {
                double delta = dataList[i] - moyenne;
                somme += delta*delta;
            }
            return Math.Sqrt(somme/(dataList.Count-1));
        }
        public List<List<double>> dataSetByGranularity(dynamic payload)
        {
            int timeFrame = getTimeFrame(payload.granularity.ToString());
            Int32 start = getTimestamp(payload.startJob.ToString());
            Int32 end = getTimestamp(payload.endJob.ToString());

            double tabSize = (end - start) / timeFrame;
            tabSize = Math.Ceiling(tabSize);
            int size = (int) tabSize;
            size += 1;

            List<List<double>> valuesTab = new List<List<double>>();

            for (int i = 0; i < size; i++)
            {
                valuesTab.Add(new List<double>());
            }
            
            foreach (var set in payload.dataSet)
            {
                Int32 timestamp = getTimestamp(set.metricDate.ToString());
                if (timestamp >= start && timestamp <= end)
                {
                    Int32 currentDiff = timestamp - start;
                    double tabIndex = currentDiff / timeFrame;
                    tabIndex = Math.Floor(tabIndex);
                    int i = (int) tabIndex;
                    double value = Double.Parse(set.metricValue.ToString());
                    valuesTab[i].Add(value);
                }
            }

            return valuesTab;
        }

        public Int32 getTimestamp(string dateTime)
        {
            DateTime setDate = Convert.ToDateTime(dateTime);
            return (Int32)setDate.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public int getTimeFrame(string granularity)
        {
            switch (granularity)
            {
                case "year":
                    return 31536000;
                    break;
                case "month":
                    return 31536000 / 12;
                    break;
                case "day":
                    return 31536000 / 12 / 30;
                    break;
                case "hour":
                    return 31536000 / 12 / 30 / 24;
                    break;
                case "minute":
                    return (int)31536000 / 12 / 30 / 24 / 60;
                    break;
                case "second":
                    return 1;
                    break;
                default:
                    return 1;
                    break;
            }
        }
        public async void notify(string brokerResponse)
        {
            Task.Run(async () =>
            {
                //DbConnector.insertUser(123);
                parseRequest(brokerResponse);
                //await Task.Delay(2000);
                //Console.WriteLine("Engine calculation n°" + _nbRequest);
                //_nbRequest++;
            });
        }
    }
}