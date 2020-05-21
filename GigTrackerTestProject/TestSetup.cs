using GigTracker.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GigTrackerTestProject {
	public class TestSetup {
		public ApplicationDbContext Setup() {
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "Gigtracker")
				.Options;

			return new ApplicationDbContext(options);
		}
	}
}
