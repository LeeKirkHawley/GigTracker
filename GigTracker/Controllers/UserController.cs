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
using Microsoft.Extensions.Logging;


namespace GigTracker.Controllers {
	public class UserController : Controller{

		private readonly UserRepository _userRepository;
		private readonly UserService _userService;
		private readonly AccountService _accountService;
		ILogger<UserController> _logger;

		public UserController(UserRepository repo, 
								UserService userService, 
								AccountService accountService,
								ILogger<UserController> logger) {
			_userRepository = repo;
			_userService = userService;
			_accountService = accountService;
			_logger = logger;
		}

		[HttpGet("User/List/{page?}")]
		public ActionResult List(int page = 1) {

			UserListViewModel model = new UserListViewModel();

			User currentUser = null;
			try {
				currentUser = _userService.GetCurrentUser(HttpContext);
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, "Couldn't get current user.");
            }

			if (currentUser == null) {
				model.ErrorMsg = $"ERROR: couldn't get current user";
				return Content(model.ErrorMsg);
			}

			if(currentUser.Role != Role.Admin) {
				model.ErrorMsg = $"ERROR: user is not admin";
				return Content(model.ErrorMsg);
			}

			model.CurrentUser = currentUser;

			IEnumerable<User> users = null;
			try {
				users = _userRepository.Get().Result;
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, "Couldn't get users.");
            }

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

			User newUser = null;
			try {
				newUser = await _userRepository.Add(model.User);
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, "Couldn't add new user.");
            }

			UserDetailsViewModel newModel = new UserDetailsViewModel {
				User = newUser
			};

			User currentUser = _userService.GetCurrentUser(HttpContext);

			if(currentUser?.Role == Role.Admin)
				return RedirectToAction("List", "User");
	
			return RedirectToAction("Index", "Home");
		}

		[HttpGet("User/Details")]
		public async Task<IActionResult> Details(int Id) {

			User currentUser = null;
			try {
				currentUser = await _userRepository.Get(Id);
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, $"Couldn't get user {Id}");
            }

			UserDetailsViewModel model = new UserDetailsViewModel {
				User = currentUser
			};

			return View(model);
		}

		[HttpGet("User/Profile")]
		public async Task<IActionResult> Profile(int Id) {

			User currentUser = null;
			try {
				currentUser = await _userRepository.Get(Id);
			}
			catch (Exception ex) {
				_logger.LogDebug(ex, $"Couldn't get user {Id}");
			}

			UserDetailsViewModel model = new UserDetailsViewModel {
				User = currentUser
			};

			return View(model);
		}

		[HttpPost("User/UpdateUser")]
		[ValidateAntiForgeryToken]
		public ActionResult UpdateUser([FromForm] UserDetailsViewModel model) {

			string currentUserId = HttpContext.Session.GetString("UserId");

			User currentUser = null;
			try {
				// gotta use NoTracking because we're already tracking the same user object, coming in in model
				currentUser = _userRepository.GetNoTracking(Convert.ToInt32(currentUserId));
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, $"Couldn't get user {currentUserId}");
            }

			if (currentUserId != model.User.Id.ToString() && currentUser.Role != Role.Admin) {
				return Content("ERROR - user cannot update this Profile.");
			}

			try {
				User newUser = _userRepository.Update(model.User).Result;
			}
			catch(Exception ex) {
				_logger.LogDebug(ex, $"Couldn't update user {currentUserId}");
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
