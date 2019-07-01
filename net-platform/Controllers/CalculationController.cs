using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using net_platform.Calculation;
using net_platform.Mqtt;
using Newtonsoft.Json;

namespace net_platform.Controllers
{
    [Route("api/calcul")]
    [ApiController]
    public class CalculationController : Controller
    {
        [HttpGet("list/{userId}")]
        public ActionResult<string> GetByUser(int userId)
        {
            List<dynamic> dataList = DbConnector.getCalculList(userId);
            string finalJson = "[";
            int compt = 0;
            foreach (var data in dataList)
            {
                if (compt != 0) finalJson += ",";
                finalJson += JsonConvert.SerializeObject(data);
                compt++;
            }

            finalJson += "]";
            return finalJson;
        }
        // GET api/values/5
        [HttpGet("get/{idJob}")]
        public ActionResult<string> Get(int idJob)
        {
            List<double> dataList = DbConnector.getCalculById(idJob);
            string finalJson = "{";
            int compt = 0;
            foreach (var data in dataList)
            {
                if (compt != 0) finalJson += ",";
                finalJson += "\"" + data + "\"";
                compt++;
            }

            finalJson += "}";
            return finalJson;
        }
    }
}