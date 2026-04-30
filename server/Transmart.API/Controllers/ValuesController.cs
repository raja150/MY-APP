using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TranSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        { 
            _logger.LogInformation("Writing to log file with INFORMATION severity level.");
        
            _logger.LogDebug("Writing to log file with DEBUG severity level."); 
        
            _logger.LogWarning("Writing to log file with WARNING severity level.");
        
            _logger.LogError("Writing to log file with ERROR severity level.");
        
            _logger.LogCritical("Writing to log file with CRITICAL severity level.");

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
		[HttpGet("QA")]
		public IActionResult QA()
		{
			return Ok();
		}

    }
}
