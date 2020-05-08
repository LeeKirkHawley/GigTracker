using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Entities;
using GigTracker.Services;
using GigTracker.Helpers;



namespace GigTracker.Controllers {
	public class UserController : Controller{

		private readonly IUserRepository _userRepository;
		private readonly UserService _userService;
		private readonly AccountService _accountService;

		public UserController(IUserRepository repo, 
								UserService userService, 
								AccountService accountService) {
			_userRepository = repo;
			_userService = userService;
			_accountService = accountService;
		}

		[HttpGet("User/List")]
		public ActionResult List() {

			User user = null;
			var userId = HttpContext.Session.GetString("UserId");
			if(userId != null)
				user = _userService.GetById(Convert.ToInt32(userId));

			if (user?.Role == Role.Admin) {

				UserListViewModel model = new UserListViewModel {
					Users = _userRepository.Get().Result
				};

				return View("List", model);
			}
			else
				return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public ViewResult Create() {
			UserCreateViewModel model = new UserCreateViewModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserCreateViewModel model) {

			User newUser = await _userRepository.Add(model.User);

			UserDetailsViewModel newModel = new UserDetailsViewModel {
				User = newUser
			};

			return View("Details", newModel);
		}

		[HttpGet("User/Details")]
		public async Task<IActionResult> Details(int Id) {

			User currentUser = await _userRepository.Get(Id);

			UserDetailsViewModel model = new UserDetailsViewModel {
				User = currentUser
			};

			return View(model);
		}

		[HttpGet("User/Profile")]
		public async Task<IActionResult> Profile(int Id) {

			User currentUser = await _userRepository.Get(Id);

			UserDetailsViewModel model = new UserDetailsViewModel {
				User = currentUser
			};

			return View(model);
		}

		[HttpPost("User/UpdateUser")]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateUser([FromForm] UserDetailsViewModel model) {

			string userId = this.HttpContext.Session.GetString("UserId");

			if (userId != model.User.Id.ToString()) {
				return Content("ERROR - user cannot update this Profile.");
			}

			User newUser = _userRepository.Update(model.User).Result;

			return RedirectToAction("Index", "Home");
		}
	}
}
