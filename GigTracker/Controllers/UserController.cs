using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GigTracker.Models;
using Microsoft.AspNetCore.Authorization;


namespace GigTracker.Controllers {
	public class UserController : Controller{

		private IUserRepository repository;

		public UserController(IUserRepository repo) {
			repository = repo;
		}

		[HttpGet]
		[Authorize]
		public ViewResult List() => View(repository.Users);
	}
}
