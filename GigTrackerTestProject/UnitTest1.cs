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
using System.Threading.Tasks;

namespace GigTrackerTestProject {
	public class Tests {

		ApplicationDbContext _context = null;

		[SetUp]
		public void Setup() {
			TestSetup setup = new TestSetup();
			_context = setup.Setup();
		}

		[Test]
		[Parallelizable(ParallelScope.None)]
		public void AddGigTest() {

			GigRepository gigRepo = new GigRepository(_context);

			var newGig = gigRepo.Add(new Gig { /*Id = 1,*/ VenueName = "Big Venue", Date = DateTime.Now.AddDays(3), ArtistName = "Lou and the Losers" }).Result;

			List<Gig> gigs = gigRepo.Get().Result.ToList();

			Gig gig = gigs.FirstOrDefault();
			Assert.AreEqual(gig.Id, newGig.Id);
		}

		[Test]
		[Parallelizable(ParallelScope.None)]
		public async Task DeleteGigTest() {

			GigRepository gigRepo = new GigRepository(_context);

			await gigRepo.Add(new Gig { /*Id = 2,*/ 
										VenueName = "Biggest Venue", 
										Date = DateTime.Now.AddDays(3), 
										ArtistName = "Lou and the Losers" });
			_context.SaveChanges();

			List<Gig> gigs = gigRepo.Get().Result.ToList();
			Gig gig = gigs.Where(g => g.VenueName == "Biggest Venue").FirstOrDefault();

			await gigRepo.Delete(gig.Id);
			_context.SaveChanges();

			gigs = gigRepo.Get().Result.ToList();
			gig = gigRepo.Get().Result.Where(g => g.VenueName == "Biggest Venue").FirstOrDefault();

			Assert.IsNull(gig);
		}

		[Test]
		[Parallelizable(ParallelScope.None)]
		public async Task UpdateGigTest() {

			GigRepository gigRepo = new GigRepository(_context);

			await gigRepo.Add(new Gig {
				//Id = 3,
				VenueName = "Large Venue",
				Date = DateTime.Now.AddDays(3),
				ArtistName = "Lou and the Losers"
			});
			_context.SaveChanges();

			Gig gig = gigRepo.Get().Result.Where(g => g.VenueName == "Large Venue").FirstOrDefault();

			gig.ArtistName = "Louis and the Losers";

			await gigRepo.Update(gig);
			_context.SaveChanges();

			gig = gigRepo.Get().Result.Where(g => g.VenueName == "Large Venue").FirstOrDefault();

			Assert.AreEqual(gig.ArtistName, "Louis and the Losers");
		}


		[Test]
		[Parallelizable(ParallelScope.None)]
		public async Task AddUserTest() {

			UserRepository userRepo = new UserRepository(_context);

			await userRepo.Add(new User { /*Id = 4,*/ UserName = "lou", FirstName = "Lou" });
			_context.SaveChanges();

			User user = userRepo.Get().Result.Where(u => u.UserName == "lou").FirstOrDefault();

			Assert.IsNotNull(user);
		}

		[Test]
		[Parallelizable(ParallelScope.None)]
		public async Task DeleteUserTest() {

			UserRepository userRepo = new UserRepository(_context);

			await userRepo.Add(new User { /*Id = 4,*/ UserName = "lou", FirstName = "Lou" });
			_context.SaveChanges();

			User user = userRepo.Get().Result.Where(u => u.UserName == "lou").FirstOrDefault();

			Assert.IsNotNull(user);

			await userRepo.Delete(user.Id);

			user = userRepo.Get().Result.Where(u => u.UserName == "lou").FirstOrDefault();
			Assert.IsNull(user);
		}

		[Test]
		[Parallelizable(ParallelScope.None)]
		public async Task UpdateUserTest() {

			UserRepository userRepo = new UserRepository(_context);

			await userRepo.Add(new User { /*Id = 4,*/ UserName = "lou", FirstName = "Lou" });
			_context.SaveChanges();

			User user = userRepo.Get().Result.Where(u => u.UserName == "lou").FirstOrDefault();

			user.UserName = "louis";
			await userRepo.Update(user);

			user = userRepo.Get().Result.Where(u => u.UserName == "louis").FirstOrDefault();
			Assert.IsNotNull(user);
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