using System.Collections.Generic;
using Configuration.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Configuration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ExampleOptions _snapshot;
        private readonly ExampleOptions _options;

        public ValuesController(IOptions<ExampleOptions> options, IOptionsSnapshot<ExampleOptions> snapshot)
        {
            _snapshot = snapshot.Value;
            _options = options.Value;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok(_options);
        }

        [HttpGet("snapshot")]
        public ActionResult<IEnumerable<string>> GetSnapshot()
        {
            return Ok(_snapshot);
        }
    }
}
