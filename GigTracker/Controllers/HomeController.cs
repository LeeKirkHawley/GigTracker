using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using GigTracker.Services;
using GigTracker.Entities;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.LinqExtensions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using NLog;
using NLog.Web;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GigTracker.Controllers {
	public class HomeController : Controller {
		UserService _userService;
		IGigRepository _gigRepository;
		ILogger<HomeController> _logger;

		public HomeController(UserService userSevice, IGigRepository gigRepository, ILogger<HomeController> logger) {
			_userService = userSevice;
			_gigRepository = gigRepository;
			_logger = logger;
		}

		//[HttpGet("")]
		[HttpGet("{suggest, page?}")]
		public IActionResult Index(string artistQuery, int page = 1, bool newQuery = false) {

			_logger.LogInformation("entering HomeController.Index");

			if(newQuery == true) {
				if (artistQuery == null || artistQuery == "")
					this.HttpContext.Session.Remove("ArtistSearch");
				else
					this.HttpContext.Session.SetString("ArtistSearch", artistQuery);
			}

			// for some reason I don't understand, page is being set to its most recent value
			// when called from the javascript search
			// so hack up a fix
			if (newQuery == true)
				page = 1;

			HomeIndexViewModel model = new HomeIndexViewModel {
				NavbarModel = new NavbarModel()
			};

			var userId = HttpContext.Session.GetString("UserId");

			User currentUser = null;
			try	{
				if (userId != null)
				{
					currentUser = _userService.GetById(Convert.ToInt32(userId));
					model.NavbarModel.CurrentUser = currentUser;
					model.NavbarModel.CurrentUserId = currentUser.Id;
				}
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, "Couldn't get user.");
            }

			IEnumerable<Gig> gigs = null;
			try {
				gigs = _gigRepository.Get().Result;
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, "Couldn't get gigs from database.");
            }

			// filter for artist?
			artistQuery = HttpContext.Session.GetString("ArtistSearch");
			if (!String.IsNullOrEmpty(artistQuery))
				gigs = gigs.Where(g => g.ArtistName.ToLower().Contains(artistQuery.ToLower())); // case insensitive

			var GigRowsToDisplay = HttpContext.Session.GetString("GigRowsToDisplay");
			if (String.IsNullOrEmpty(GigRowsToDisplay) == true)
				GigRowsToDisplay = "5";  // at the moment this is the only way to set number of rows to show

			PagedResult<Gig> result = gigs.GetPaged<Gig>(page, Convert.ToInt32(GigRowsToDisplay));  // page number, page size
			model.Gigs = result;


			if (currentUser != null) {
				model.NavbarModel.CurrentUserId = currentUser.Id;
				model.NavbarModel.CurrentUser = currentUser;
			}
			model.NavbarModel.ArtistSearch = artistQuery;

			if (userId != null)
				this.HttpContext.Session.SetString("UserId", userId.ToString());


			return View("Index", model);
		}

		public ActionResult ModalPopUp() {
			return View();
		}

		[HttpGet("Home/Error")]
		public IActionResult Error() {
			//Retrieve error information in case of internal errors
		   ErrorViewModel model = new ErrorViewModel();

			var error = HttpContext
					  .Features
					  .Get<IExceptionHandlerFeature>();

			if (error != null) {
				var exception = error.Error;
				model.DisplayMsg = exception.Message;
			}

			return View(model);
		}
	}
}
