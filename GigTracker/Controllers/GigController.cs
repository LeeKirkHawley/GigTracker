using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GigTracker.Models;
using GigTracker.Data;

namespace GigTracker.Controllers {
	public class GigController : Controller {
		private IGigRepository _gigRepository;

		public GigController(IGigRepository gigRepository) {
			_gigRepository = gigRepository;
		}

		[HttpGet]
		//[Authorize]
		public ViewResult List() {
			IEnumerable<Gig> gigs = _gigRepository.Get();
			return View(gigs);
		}
	}
}
