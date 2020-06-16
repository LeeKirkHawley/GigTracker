using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Entities;
using GigTracker.Services;
using Newtonsoft.Json;
using GigTracker.LinqExtensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GigTracker.Controllers {
	public class GigController : Controller {
		private readonly IGigRepository _gigRepository;
		private readonly UserRepository _userRepository;
		private readonly UserService _userService;

		public GigController(IGigRepository gigRepository, UserService userService, UserRepository userRepository) {
			_gigRepository = gigRepository;
			_userService = userService;
			_userRepository = userRepository;
		}

		[HttpGet("Gig/List/{page?}")]
		public ViewResult List(int page = 1) {

			GigListViewModel model = new GigListViewModel {
				NavbarModel = new NavbarModel()
			};

			int? userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
			if (userId.HasValue == false)
				model.ErrorMsg = "ERROR: no user ID";

			User currentUser = null;
			if (userId != null)
				currentUser = _userService.GetById(Convert.ToInt32(userId));
			else
				model.ErrorMsg = $"ERROR: couldn't find user {userId}";
			model.NavbarModel.CurrentUser = currentUser;
			model.NavbarModel.CurrentUserId = currentUser.Id;

			IEnumerable<Gig> gigs = _gigRepository.Get().Result.Where(g => g.UserId == userId);

			var GigRowsToDisplay = HttpContext.Session.GetString("GigRowsToDisplay");
			if (String.IsNullOrEmpty(GigRowsToDisplay) == true)
				GigRowsToDisplay = "5";  // at the moment this is the only way to set number of rows to show

			PagedResult<Gig> result = gigs.GetPaged<Gig>(page, Convert.ToInt32(GigRowsToDisplay));  // page number, page size
			model.Gigs = result;

			return View(model);
		}

		[HttpGet("Gig/Create")]
		public ViewResult Create() {

			Gig gig = new Gig {
				ArtistName = "Fred",
				VenueName = "Sam's",
				VenueAddress = "322 E. 1st. South",
				VenuePhone = "6027519025",
				Date = DateTime.Now.AddDays(30)
			};

			GigCreateViewModel model = new GigCreateViewModel {
				Gig = gig
			};

			return View(model);
		}

		[HttpPost("Gig/Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(GigCreateViewModel model) {

			if (ModelState.IsValid) { };

			string UserId = this.HttpContext.Session.GetString("UserId");

			model.Gig.UserId = Convert.ToInt32(UserId);

			var newGig = await _gigRepository.Add(model.Gig);

			return RedirectToAction("Details", "Gig", new { id = newGig.Id });
		}

		[HttpGet("Gig/Details/{id}")]
		public ActionResult Details(int id) {

			string strCurrentUserId = HttpContext.Session.GetString("UserId");
			int? currentUserId = null;
			User currentUser = null;

			if (String.IsNullOrEmpty(strCurrentUserId) == false) {
				currentUserId = Convert.ToInt32(strCurrentUserId);
				currentUser = _userRepository.Get(Convert.ToInt32(currentUserId)).Result;
			}

			Gig gig = _gigRepository.Get(id).Result;

			GigDetailsViewModel model = new GigDetailsViewModel {
				NavbarModel = new NavbarModel {
					CurrentUserId = currentUserId,
					CurrentUser = currentUser,
					ArtistSearch = ""
				},
 				Gig = gig
			};

			var v = View(model);
			return v;
		}

		[HttpGet("Gig/Edit/{id}")]
		public ActionResult Edit(int id) {

			string userId = this.HttpContext.Session.GetString("UserId");

			Gig gig = _gigRepository.Get(id).Result;

			if (gig.UserId != Convert.ToInt32(userId)) {
				return Content("ERROR - user cannot edit this Gig.");
			}

			gig.User = _userService.GetById(Convert.ToInt32(gig.UserId));

			GigEditViewModel model = new GigEditViewModel {
				Gig = gig
			};

			var v = View(model);
			return v;
		}

		[HttpPost("Gig/UpdateGig")]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateGig([FromForm] GigEditViewModel model) {

			string userId = this.HttpContext.Session.GetString("UserId");

			if(userId != model.Gig.UserId.ToString()) {
				return Content("ERROR - user cannot edit this Gig.");
			}

			try {
				Gig newGig = _gigRepository.Update(model.Gig).Result;
			}
			catch(Exception ex) {

			}

			return RedirectToAction("List", "Gig");
		}
	}
}
