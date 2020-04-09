﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GigTracker.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace GigTracker.Controllers
{

    public class AccountController : Controller
    {
        IUserRepository _userRepository;
        IAccountService _accountService;

        public AccountController(IUserRepository userRepository, IAccountService accountService) {
            _userRepository = userRepository;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null) {
            //return View();
            //return RedirectToLocal(returnUrl);

            var user = _userRepository.Users.Where(u => u.Email == userModel.Email).FirstOrDefault();

            if (user == null) {
                ModelState.AddModelError("", "User not found");
                return View();
            }

            if(_accountService.VerifyPwd(user.Password, userModel.Password) != true) {
                ModelState.AddModelError("", "Login failed.");
                return View();
            }


            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            //identity.AddClaim(new Claim(ClaimTypes.Name, user.Ssn));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));

            //foreach (var role in user.Roles) {
            //    identity.AddClaim(new Claim(ClaimTypes.Role, role.Role));
            //}
            identity.AddClaim(new Claim(ClaimTypes.Authentication, "true"));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}