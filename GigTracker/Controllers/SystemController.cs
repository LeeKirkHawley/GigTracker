using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using GigTracker.Services;
using GigTracker.Entities;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.LinqExtensions;
using Microsoft.AspNetCore.Diagnostics;


namespace GigTracker.Controllers
{
	[ApiController]
	public class SystemController : ControllerBase
    {
		[Route("Error")]
		public IActionResult Error() {
			//			return View();
			// Retrieve error information in case of internal errors
			ErrorViewModel model = new ErrorViewModel();

			var error = HttpContext
					  .Features
					  .Get<IExceptionHandlerFeature>();

			if (error != null) {
				var exception = error.Error;
				model.DisplayMsg = exception.Message;
			}

			//return View(model);
			return RedirectToAction("Index", "Error");
		}

	}
}