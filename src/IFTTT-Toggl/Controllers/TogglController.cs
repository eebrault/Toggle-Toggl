using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Toggl;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IFTTT_Toggl.Controllers
{
    [Route("api/[controller]")]
    public class TogglController : Controller
    {
	    private string TogglApiKey { get; set; }

	    private ILogger<TogglController> Logger { get; set; }

	    public TogglController(ILogger<TogglController> logger)
	    {
		    Logger = logger;
		    TogglApiKey = Startup.Configuration["AppSettings:TogglApiKey"];
	    }


	    // POST api/toggl/start
		[HttpPost("start")]
        public void Start([FromBody]string value)
        {

		}

        [HttpPost]
        public void Stop([FromBody]string value)
        {
        }
    }
}
