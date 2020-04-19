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
			//var userId = TempData["UserId"];

			var userId = HttpContext.Session.GetString("UserId");

			User currentUser = null;
			if(userId != null)
				currentUser = _userService.GetById(Convert.ToInt32(userId));

			HomeIndexViewModel model = new HomeIndexViewModel();
			if(currentUser != null)
			{
				model.userId = currentUser.Id;
				model.userRole = currentUser.Role;
			};

			if(userId != null)
				this.HttpContext.Session.SetString("UserId", userId.ToString());

			return View("Index", model);
		}
	}
}
