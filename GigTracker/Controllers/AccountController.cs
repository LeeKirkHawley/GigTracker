using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Services;
using GigTracker.Entities;

namespace GigTracker.Controllers
{

    public class AccountController : Controller
    {
        IUserRepository _userRepository;
        AccountService _accountService;
        UserService _userService;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(IUserRepository userRepository, 
                                AccountService accountService, 
                                UserService userService,
                                SignInManager<IdentityUser> signInManager) {
            _userRepository = userRepository;
            _accountService = accountService;
            _userService = userService;
            _signInManager = signInManager;
        }

        [HttpGet("Account/Login")]
        public IActionResult Login(string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Account/Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null) {

            IEnumerable<User> users = _userRepository.Get().Result;
            User user = users.Where(u => u.Email == userModel.Email).FirstOrDefault();

            if (user == null) {
                ModelState.AddModelError("", "User not found");
                return View();
            }

            if (_accountService.VerifyPwd(user.Password, userModel.Password) != true) {
                ModelState.AddModelError("", "Login failed.");
                return View();
            }

            this.HttpContext.Session.SetString("UserId", user.Id.ToString());

            //return RedirectToAction("Index", "Home");
            return RedirectToAction("List", "Gig");
        }

        private IActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}