using NUnit.Framework;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Entities;
using Microsoft.EntityFrameworkCore;
using GigTracker.Services;
using GigTracker.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;

namespace GigTrackerTestProject {
	public class Tests {
		[SetUp]
		public void Setup() {
		}

		[Test]
		public void Test1() {
			TestSetup setup = new TestSetup();

			ApplicationDbContext context = setup.Setup();

			context.Gig.Add(new Gig { Id = 1, VenueName = "Big Venue", Date = DateTime.Now.AddDays(3), ArtistName = "Lou and the Losers"});
			context.SaveChanges();

			GigRepository gigRepo = new GigRepository(context);
			List<Gig> gigs = gigRepo.Get().Result.ToList();
		}

		//		[Test]
		//		public void Test1() {
		//			var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
		//			var result = await _userManager.CreateAsync(user, model.Password);
		//			if (result.Succeeded) {
		//				_logger.LogInformation("User created a new account with password.");
		//​
		//            // Add a user to the default role, or any role you prefer here
		//            await _userManager.AddToRoleAsync(user, "Member");
		//​
		//            await _signInManager.SignInAsync(user, isPersistent: false);
		//				_logger.LogInformation("User created a new account with password.");

		//				Assert.Pass();
		//		}
	}
}