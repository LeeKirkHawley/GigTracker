using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GigTracker.Controllers {
	public class HomeController : Controller {
		// GET: /<controller>/
		[Route("")]
		[Route("Home")]
		[Route("Home/Index")]
		[Authorize]
		public IActionResult Index() {
			return View("Index");
		}
	}
}
