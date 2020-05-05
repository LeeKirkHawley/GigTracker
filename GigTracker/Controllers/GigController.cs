using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Entities;
using GigTracker.Services;
using Newtonsoft.Json;
using GigTracker.LinqExtensions;

namespace GigTracker.Controllers {
	public class GigController : Controller {
		private readonly IGigRepository _gigRepository;
		private readonly UserService _userService;

		public GigController(IGigRepository gigRepository, UserService userService) {
			_gigRepository = gigRepository;
			_userService = userService;
		}

		[HttpGet("Gig/List/{page?}")]
		public ViewResult List(int page = 1) {

			GigListViewModel model = new GigListViewModel();

			int? userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
			if (userId.HasValue == false)
				model.ErrorMsg = "ERROR: no user ID";

			User currentUser = null;
			if (userId != null)
				currentUser = _userService.GetById(Convert.ToInt32(userId));
			else
				model.ErrorMsg = $"ERROR: couldn't find user {userId}";
			model.CurrentUser = currentUser;

			IEnumerable<Gig> gigs = _gigRepository.Get().Result.Where(g => g.UserId == userId);

			var GigRowsToDisplay = HttpContext.Session.GetString("GigRowsToDisplay");
			if (String.IsNullOrEmpty(GigRowsToDisplay) == true)
				GigRowsToDisplay = "5";  // at the moment this is the only way to set number of rows to show


			PagedResult<Gig> result = gigs.GetPaged<Gig>(page, Convert.ToInt32(GigRowsToDisplay));  // page number, page size
			model.Gigs = result;

			return View(model);
		}


		//[HttpGet("Gig/UserGigs")]
		//public IEnumerable<Gig> UserGigs() {

		//	string UserId = this.HttpContext.Session.GetString("UserId");
		//	User currentUser = _userService.GetById(Convert.ToInt32(UserId));

		//	IEnumerable<Gig> gigs = _gigRepository.Get().Result;

		//	GigListViewModel model = new GigListViewModel {
		//		Gigs = gigs,
		//		User = currentUser
		//	};

		//	return gigs;
		//}

		[HttpGet("Gig/Create")]
		public ViewResult Create() {
			GigCreateViewModel model = new GigCreateViewModel();
			return View(model);
		}

		[HttpPost("Gig/Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(GigCreateViewModel model) {

			string UserId = this.HttpContext.Session.GetString("UserId");

			model.Gig.UserId = Convert.ToInt32(UserId);

			var newGig = await _gigRepository.Add(model.Gig);

			return View($"~/Gig/Details/{newGig.Id}");
		}

		[HttpGet("~/Gig/Details/{id}")]
		public async Task<IActionResult> Details(int id) {

			Gig gig = await _gigRepository.Get(id);

			GigDetailsViewModel detailsModel = new GigDetailsViewModel {
				Gig = gig
			};

			return View("Details", detailsModel);
		}
	}
}
