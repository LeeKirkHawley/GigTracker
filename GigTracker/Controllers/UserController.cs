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

		[HttpGet("User/List/{page?}")]
		public ActionResult List(int page = 1) {

			UserListViewModel model = new UserListViewModel();

			int? userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
			if (userId.HasValue == false) {
				model.ErrorMsg = "ERROR: no user ID";
				return Content(model.ErrorMsg);
			}

			User currentUser = null;
			if (userId != null)
				currentUser = _userService.GetById(Convert.ToInt32(userId));
			else {
				model.ErrorMsg = $"ERROR: couldn't find user {userId}";
				return Content(model.ErrorMsg);
			}

			if(currentUser.Role != Role.Admin) {
				model.ErrorMsg = $"ERROR: user is not admin {userId}";
				return Content(model.ErrorMsg);
			}

			model.CurrentUser = currentUser;

			IEnumerable<User> users = _userRepository.Get().Result;

			var GigRowsToDisplay = HttpContext.Session.GetString("GigRowsToDisplay");
			if (String.IsNullOrEmpty(GigRowsToDisplay) == true)
				GigRowsToDisplay = "5";  // at the moment this is the only way to set number of rows to show

			PagedResult<User> result = users.GetPaged<User>(page, Convert.ToInt32(GigRowsToDisplay));  // page number, page size
			model.Users = result;

			return View(model);
		}

		[HttpGet("User/Create")]
		public ViewResult Create() {
			UserCreateViewModel model = new UserCreateViewModel();
			return View(model);
		}

		[HttpPost("User/Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(UserCreateViewModel model) {

			User newUser = await _userRepository.Add(model.User);

			UserDetailsViewModel newModel = new UserDetailsViewModel {
				User = newUser
			};

			//return View("Details", newModel);
			return RedirectToAction("List", "User");
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

			string currentUserId = HttpContext.Session.GetString("UserId");

			// gotta use NoTracking because we're already tracking the same user object, coming in in model
			User currentUser = _userRepository.GetNoTracking(Convert.ToInt32(currentUserId));

			if (currentUserId != model.User.Id.ToString() && currentUser.Role != Role.Admin) {
				return Content("ERROR - user cannot update this Profile.");
			}

			User newUser = _userRepository.Update(model.User).Result;

			return RedirectToAction("Index", "Home");
		}
	}
}
