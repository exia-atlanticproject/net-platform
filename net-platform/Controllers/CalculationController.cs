using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace net_platform.Controllers
{
    [Route("api/calcul")]
    [ApiController]
    public class CalculationController : Controller
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] {"value1", "value2"};
        }
    }
}