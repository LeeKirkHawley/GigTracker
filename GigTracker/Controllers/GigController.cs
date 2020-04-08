using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GigTracker.Models;


namespace GigTracker.Controllers {
	public class GigController : Controller {
		private IGigRepository repository;

		public GigController(IGigRepository repo) {
			repository = repo;
		}

		[HttpGet]
		public ViewResult List() => View(repository.Gigs);

	}
}
