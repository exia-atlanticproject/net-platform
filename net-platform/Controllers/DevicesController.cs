using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace net_platform.Controllers
{
    [Route("api/device/")]
    [ApiController]
    public class DevicesController : Controller
    {
        [HttpPost("{deviceId}/telemetry")]
        public string Post([FromBody] string data)
        {
            return "ok";
        }
        
        [HttpPost]
        public string PostDevice([FromBody] string data)
        {
            return "ok";
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1", "value2"};
        }
    }
}