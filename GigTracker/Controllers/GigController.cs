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

		[HttpPost]
		public async Task<IActionResult> AllGigs(string filter) {

			IEnumerable<Gig> gigs = _gigRepository.Get().Result;
			var filtered = gigs;
			//.Select(g => new { g.ArtistName, g.Date, g.VenueName })
			//.Where(g => g.VenueName == "Ripps").ToList();

			////List<GigGridViewModel> gigViewList = new List<GigGridViewModel>();
			//GigGridViewModel model = new GigGridViewModel();
			//model.data = new List<Data>();

			//foreach (Gig gig in filtered) {
			//	Data data = new Data();

			//	data.ArtistName = gig.ArtistName;
			//	data.Date = gig.Date.ToString();
			//	data.VenueName = gig.VenueName;

			//	model.data.Add(data);
			//}

			var json =  new JsonResult(new JqueryDataTablesResult<Gig> {
				Data = gigs,
//				RecordsFiltered = results.TotalSize,
				RecordsTotal = gigs.Count()
			});

			//var json = JsonConvert.SerializeObject(filtered);
			//var json = JsonConvert.SerializeObject(model);

			return json;
		}

		[HttpGet]
		public IEnumerable<Gig> UserGigs() {

			string UserId = this.HttpContext.Session.GetString("UserId");
			User currentUser = _userService.GetById(Convert.ToInt32(UserId));

			IEnumerable<Gig> gigs = _gigRepository.Get().Result;

			GigListViewModel model = new GigListViewModel {
				Gigs = gigs,
				User = currentUser
			};

			return gigs;
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
