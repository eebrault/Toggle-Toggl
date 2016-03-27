using System;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Toggl;
using Toggl.Extensions;
using Toggl.QueryObjects;
using Toggl.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IFTTT_Toggl.Controllers
{
    [Route("api/[controller]")]
    public class TogglController : Controller
    {
	    private string TogglApiKey { get; set; }

	    private ILogger<TogglController> Logger { get; set; }

	    private TimeEntryService TimeEntryService { get; set; }

	    public TogglController(ILogger<TogglController> logger)
		{
			Logger = logger;
			TogglApiKey = Startup.Configuration["AppSettings:TogglApiKey"];
			TimeEntryService = new TimeEntryService(TogglApiKey);

			SetDefaultWorkspace();
		}

		private void SetDefaultWorkspace()
		{
			var workspaceService = new WorkspaceService(TogglApiKey);
			var defaultWorkspace = workspaceService.List().FirstOrDefault();
			if (defaultWorkspace?.Id != null) DefaultWorkspacedId = (int) defaultWorkspace.Id;
		}

		private int DefaultWorkspacedId { get; set; }

	    // POST api/toggl/start
		[HttpPost("start")]
        public void Start([FromBody]string value)
		{
			// Start a new time entry
			TimeEntryService.Start(new TimeEntry()
			{
				Description = "IFTTT",
				CreatedWith = "IFTTT Channel",
				WorkspaceId = DefaultWorkspacedId
			});
		}

        [HttpPost]
        public void Stop([FromBody]string value)
        {
        }
    }
}
