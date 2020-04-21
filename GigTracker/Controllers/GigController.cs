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

namespace GigTracker.Controllers {
	public class GigController : Controller {
		private readonly IGigRepository _gigRepository;
		private readonly UserService _userService;

		public GigController(IGigRepository gigRepository, UserService userService) {
			_gigRepository = gigRepository;
			_userService = userService;
		}

		[HttpGet]
		public ViewResult List() {

			string UserId = this.HttpContext.Session.GetString("UserId");
			User currentUser = _userService.GetById(Convert.ToInt32(UserId));

			IEnumerable <Gig> gigs = _gigRepository.Get().Result;

			GigListViewModel model = new GigListViewModel {
				Gigs = gigs, 
				User = currentUser
			};

			return View(model);
		}

		[HttpGet]
		public ViewResult Create() {
			GigCreateViewModel model = new GigCreateViewModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(GigCreateViewModel model) {

			string UserId = this.HttpContext.Session.GetString("UserId");

			model.Gig.UserId = Convert.ToInt32(UserId);

			var newGig = await _gigRepository.Add(model.Gig);

			GigDetailsViewModel detailsModel = new GigDetailsViewModel {
				Gig = newGig
			};

			return View("Details", detailsModel);
		}

		[HttpGet]
		public async Task<IActionResult> Details(int id) {

			Gig gig = await _gigRepository.Get(id);

			GigDetailsViewModel detailsModel = new GigDetailsViewModel {
				Gig = gig
			};

			return View("Details", detailsModel);
		}
	}
}
