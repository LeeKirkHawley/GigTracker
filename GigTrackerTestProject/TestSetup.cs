using GigTracker.Entities;
using GigTracker.Helpers;
using GigTracker.Repositories;
using GigTracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GigTrackerTestProject {
	class TestSetup {

		public readonly AppSettings settings = new AppSettings();

		public TestSetup() {
			settings.Secret = "";
		}

		public IOptions<AppSettings> Settings() {
			IOptions<AppSettings> appSettingsOptions = Options.Create(settings);
			return appSettingsOptions;
		}

		public ApplicationDbContext Setup() {
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "Gigtracker")
				.Options;

			var context = new ApplicationDbContext(options) {
				//context.Movies.Add(new Movie { Id = 1, Title = "Movie 1", YearOfRelease = 2018, Genre = "Action" });
				//context.Movies.Add(new Movie { Id = 2, Title = "Movie 2", YearOfRelease = 2018, Genre = "Action" });
				//context.Movies.Add(nnew Movie { Id = 3, Title = "Movie 3", YearOfRelease = 2019, Genre = "Action"});
				//context.SaveChanges();
			};

			return context;
		}
	}
}
