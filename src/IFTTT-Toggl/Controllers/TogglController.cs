using System;
using System.Linq;
using System.Net;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Toggl;
using Toggl.Services;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace IFTTT_Toggl.Controllers
{
    [Route("api/[controller]")]
    public class TogglController : Controller
    {
	    private string TogglApiKey { get; }

	    private ILogger<TogglController> Logger { get; }

	    private TimeEntryService TimeEntryService { get; }

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
        public JsonResult Start([FromBody]string value)
		{
			try
			{
				// Start a new time entry
				var runningTimeentry = TimeEntryService.Start(new TimeEntry()
				{
					Description = "IFTTT",
					CreatedWith = "IFTTT Channel",
					WorkspaceId = DefaultWorkspacedId
				});

				return Json(runningTimeentry);
			}
			catch (Exception ex)
			{
				Logger.LogError("Failed to start new time entry", ex);
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new {ex.Message });
			}
		}

        [HttpPost]
        public void Stop([FromBody]string value)
        {
        }
    }
}
