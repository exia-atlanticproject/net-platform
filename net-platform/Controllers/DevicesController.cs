using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using net_platform.Mqtt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace net_platform.Controllers
{
    [Route("api/device/")]
    [ApiController]
    public class DevicesController : Controller
    {
        [HttpPost("{deviceId}/telemetry")]
        public string Post([FromBody] dynamic data, string deviceId)
        {
            data.name = "";
            data.macAddress = "";
            MqttServer.processMqttMessage(data, deviceId);
            return "success";
        }
        
        [HttpPost]
        public string PostDevice([FromBody] dynamic data)
        {
            string id = Guid.NewGuid().ToString();
            string name;
            if (data.name.ToString().Length == 0)
            {
                name = data.deviceType.ToString() + " - " + id;
            }
            else
            {
                name = data.name.ToString();
            }

            var finalData = new
            {
                action="registerDevice",
                source="",
                callback="",
                payload= new
                {
                    deviceId=id,
                    deviceName=name,
                    deviceType=data.deviceType.ToString()
                }
            };
            string jsonDataSet = JsonConvert.SerializeObject(finalData);
            string returnDataSet = JsonConvert.SerializeObject(finalData.payload);
            BrokerClient.sendMqttMessage(jsonDataSet, "Data-Controller");
            
            return returnDataSet;
        }
    }
}