using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GigTracker.Models;
using GigTracker.Data;
using GigTracker.Entities;
using GigTracker.Services;
using GigTracker.Helpers;


namespace GigTracker.Controllers {
	public class UserController : Controller{

		private readonly IUserRepository _userRepository;
		private readonly UserService _userService;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly AccountService _accountService;

		public UserController(IUserRepository repo, 
								UserService userService, 
								UserManager<IdentityUser> userManager,
								AccountService accountService) {
			_userRepository = repo;
			_userService = userService;
			_userManager = userManager;
			_accountService = accountService;
		}

		[HttpGet]
		public ViewResult List() {
			//if (_accountService._currentUser.Role == "Administrator") {
			//	var user = _userManager.GetUserAsync(HttpContext.User);
			//	return View(_userRepository.Get());
			//}
			//else
				return View("Index", "Home");
		}

		//[AllowAnonymous]
		//[HttpPost("authenticate")]
		//public IActionResult Authenticate([FromBody]AuthenticateModel model) {
		//	var user = _userService.Authenticate(model.Username, model.Password);

		//	if (user == null)
		//		return BadRequest(new { message = "Username or password is incorrect" });

		//	return Ok(user);
		//}

		[HttpGet("{id}")]
		public IActionResult GetById(int id) {

			//var user = _users.FirstOrDefault(x => x.Id == id);
			User user = _userService.GetById(id);
			user =  user.WithoutPassword();


			//// only allow admins to access other user records
			//var currentUserId = int.Parse(User.Identity.Name);
			//if (id != currentUserId && !User.IsInRole(Role.Admin))
			//	return Forbid();

			//var user = _userRepository.Get(id);

			if (user == null)
				return NotFound();

			return Ok(user);
		}
	}
}
