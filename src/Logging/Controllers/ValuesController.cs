using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger _logger;

        public ValuesController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ValuesController>();
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            using (_logger.BeginScope("Beispielscope"))
            {
                _logger.LogTrace("This is for development purposes only.");
                _logger.LogDebug("This is for debugging only.");
                _logger.LogInformation("This is an information.");
                _logger.LogWarning("This is a warning.");
                _logger.LogError("This is an error.");
                _logger.LogCritical("This is critical.");

                var result = new { Key = "value" };

                _logger.LogInformation("Returning {@result}.", result);
                return Ok(result);
            }
        }
    }
}
