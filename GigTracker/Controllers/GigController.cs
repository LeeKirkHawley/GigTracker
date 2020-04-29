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

			var userId = HttpContext.Session.GetString("UserId");

			User currentUser = null;
			if (userId != null)
				currentUser = _userService.GetById(Convert.ToInt32(userId));

			GigListViewModel model = new GigListViewModel();

			IEnumerable<Gig> gigs = _gigRepository.Get().Result;
			//if (!String.IsNullOrEmpty(artistQuery))
			//	gigs = gigs.Where(g => g.ArtistName == artistQuery);
			var GigRowsToDisplay = HttpContext.Session.GetString("GigRowsToDisplay");


			PagedResult<Gig> result = gigs.GetPaged<Gig>(page, Convert.ToInt32(GigRowsToDisplay));  // page number, page size
			model.Gigs = result;

			//if (currentUser != null) {
			//	model.userId = currentUser.Id;
			//	model.userRole = currentUser.Role;
			//	model.ArtistSearch = artistQuery;
			//};

			//if (userId != null)
			//	this.HttpContext.Session.SetString("UserId", userId.ToString());

			return View(model);


			//string UserId = this.HttpContext.Session.GetString("UserId");
			//User currentUser = _userService.GetById(Convert.ToInt32(UserId));

			//IEnumerable <Gig> gigs = _gigRepository.Get().Result;

			//GigListViewModel model = new GigListViewModel {
			//	Gigs = gigs, 
			//	User = currentUser
			//};

			//return View(model);
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

			return View($"Details/{newGig.Id}");
		}

		[HttpGet("Gig/Details/{id}")]
		public async Task<IActionResult> Details(int id) {

			Gig gig = await _gigRepository.Get(id);

			GigDetailsViewModel detailsModel = new GigDetailsViewModel {
				Gig = gig
			};

			return View("Details", detailsModel);
		}
	}
}
