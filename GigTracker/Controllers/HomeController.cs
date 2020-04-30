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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GigTracker.Controllers {
	public class HomeController : Controller {
		UserService _userService;
		IGigRepository _gigRepository;

		public HomeController(UserService userSevice, IGigRepository gigRepository) {
			_userService = userSevice;
			_gigRepository = gigRepository;
		}

		//[HttpGet("")]
		[HttpGet("{suggest, page?}")]
		public IActionResult Index(string artistQuery, int page = 1) {

			HomeIndexViewModel model = new HomeIndexViewModel();

			var userId = HttpContext.Session.GetString("UserId");

			User currentUser = null;
			if(userId != null)
				currentUser = _userService.GetById(Convert.ToInt32(userId));

			IEnumerable<Gig> gigs = _gigRepository.Get().Result;
			if (!String.IsNullOrEmpty(artistQuery))
				gigs = gigs.Where(g => g.ArtistName == artistQuery);

			var GigRowsToDisplay = HttpContext.Session.GetString("GigRowsToDisplay");
			if (String.IsNullOrEmpty(GigRowsToDisplay) == true)
				GigRowsToDisplay = "5";  // at the moment this is the only way to set number of rows to show

			PagedResult<Gig> result = gigs.GetPaged<Gig>(page, Convert.ToInt32(GigRowsToDisplay));  // page number, page size
			model.Gigs = result;


			if (currentUser != null) {
				model.userId = currentUser.Id;
				model.userRole = currentUser.Role;
				model.ArtistSearch = artistQuery;
			}

			// open error dlg
			//model.ErrorMsg = "ERROR:";

			if (userId != null)
				this.HttpContext.Session.SetString("UserId", userId.ToString());

			return View("Index", model);
		}

		public ActionResult ModalPopUp() {
			return View();
		}
	}
}
