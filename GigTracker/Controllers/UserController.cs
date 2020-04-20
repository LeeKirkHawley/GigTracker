﻿using System;
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

		[HttpGet]
		public ActionResult List() {

			User user = null;
			var userId = HttpContext.Session.GetString("UserId");
			if(userId != null)
				user = _userService.GetById(Convert.ToInt32(userId));

			if(user?.Role == Role.Admin)
				return View(_userRepository.Get());
			else
				return RedirectToAction("Index", "Home");
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id) {

			User user = _userService.GetById(id);
			user =  user.WithoutPassword();

			if (user == null)
				return NotFound();

			return Ok(user);
		}
	}
}
