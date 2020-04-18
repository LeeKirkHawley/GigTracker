using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GigTracker.Services;
using GigTracker.Entities;
using GigTracker.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GigTracker.Controllers {
	public class HomeController : Controller {
		// GET: /<controller>/
		//[Route("")]
		//[Route("/Home")]
		//[Route("/Home/Index")]
		//[Authorize]  // anybody should be able to access

		UserService _userService;

		public HomeController(UserService userSevice) {
			_userService = userSevice;
		}

		public IActionResult Index() {
			var userId = TempData["UserId"];

			User currentUser = null;
			if(userId != null)
				currentUser = _userService.GetById(Convert.ToInt32(userId));

			HomeIndexViewModel model = new HomeIndexViewModel {
				user = currentUser
			};

			return View("Index", model);
		}
	}
}
