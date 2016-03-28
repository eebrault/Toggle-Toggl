using System;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using IFTTT_Toggl.ViewModels;
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
		private TimeEntryService _timeEntryService;

		private ILogger<TogglController> Logger { get; }

		private string TogglApiKey { get; set; }

		private int DefaultWorkspacedId { get; set; }

		private TimeEntryService TimeEntryService
		{
			get
			{
				if (_timeEntryService == null)
				{
					_timeEntryService = new TimeEntryService(TogglApiKey);
				}

				return _timeEntryService;
			}
		}

		public TogglController(ILogger<TogglController> logger)
		{
			Logger = logger;
		}

		private void SetDefaultWorkspace()
		{
			var workspaceService = new WorkspaceService(TogglApiKey);
			var defaultWorkspace = workspaceService.List().FirstOrDefault();
			if (defaultWorkspace?.Id != null) DefaultWorkspacedId = (int)defaultWorkspace.Id;
		}

		private void InitParams(ApiKeyViewModel vm)
		{
			TogglApiKey = vm.TogglApiKey;
			SetDefaultWorkspace();
		}

		// POST api/toggl/start
		[HttpPost("start")]
		public JsonResult Start([FromBody]ApiKeyViewModel vm)
		{
			try
			{
				InitParams(vm);

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
				return Json(new { ex.Message });
			}
		}

		[HttpPost("stop")]
		public JsonResult Stop([FromBody]ApiKeyViewModel vm)
		{
			try
			{
				InitParams(vm);
				// Get running time entry
				var runningTimeEntry = TimeEntryService.Current();
				if (runningTimeEntry.Id != null)
				{
					var stoppedTimeEntry = TimeEntryService.Stop(runningTimeEntry);
					return Json(stoppedTimeEntry);
				}

				Logger.LogError("Unable to find a running time entry.");
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new {Message = "Unable to find a running time entry."});
			}
			catch (Exception ex)
			{
				Logger.LogError("Failed to stop time entry", ex);
				Response.StatusCode = (int)HttpStatusCode.BadRequest;
				return Json(new { ex.Message });
			}
		}
	}
}
