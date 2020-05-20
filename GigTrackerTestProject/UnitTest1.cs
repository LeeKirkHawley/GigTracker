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
		ApplicationDbContext _context = null;

		[SetUp]
		public void Setup() {
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "Gigtracker")
				.Options;

			_context = new ApplicationDbContext(options);
		}

		[Test]
		public void AddGigTest() {

			_context.Gig.Add(new Gig { Id = 1, VenueName = "Big Venue", Date = DateTime.Now.AddDays(3), ArtistName = "Lou and the Losers"});
			_context.SaveChanges();

			GigRepository gigRepo = new GigRepository(_context);
			List<Gig> gigs = gigRepo.Get().Result.ToList();

			Assert.AreEqual(gigs.FirstOrDefault().Id, 1);
		}

		[Test]
		public void DeleteGigTest() {

			_context.Gig.Add(new Gig { Id = 1, 
										VenueName = "Big Venue", 
										Date = DateTime.Now.AddDays(3), 
										ArtistName = "Lou and the Losers" });
			_context.SaveChanges();

			GigRepository gigRepo = new GigRepository(_context);
			List<Gig> gigs = gigRepo.Get().Result.ToList();
			Gig gig = gigs.FirstOrDefault();

			gigRepo.Delete(gig.Id);
			_context.SaveChanges();

			gigs = gigRepo.Get().Result.ToList();

			Assert.AreEqual(gigs.Count, 0);
		}

		[Test]
		public void UpdateGigTest() {

			_context.Gig.Add(new Gig {
				Id = 1,
				VenueName = "Big Venue",
				Date = DateTime.Now.AddDays(3),
				ArtistName = "Lou and the Losers"
			});
			_context.SaveChanges();

			GigRepository gigRepo = new GigRepository(_context);
			List<Gig> gigs = gigRepo.Get().Result.ToList();
			Gig gig = gigs.FirstOrDefault();

			gig.ArtistName = "Louis and the Losers";

			gigRepo.Update(gig);
			_context.SaveChanges();

			gig = gigRepo.Get().Result.ToList().FirstOrDefault();

			Assert.AreEqual(gig.ArtistName, "Louis and the Losers");
		}


		[Test]
		public void AddUserTest() {

			_context.User.Add(new User { Id = 1, UserName = "lou", FirstName = "Lou" });
			_context.SaveChanges();

			UserRepository userRepo = new UserRepository(_context);
			List<User> users = userRepo.Get().Result.ToList();
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