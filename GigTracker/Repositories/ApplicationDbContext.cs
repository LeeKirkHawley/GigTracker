using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GigTracker.Models;
using GigTracker.Entities;

namespace GigTracker.Repositories {
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>{

		public ApplicationDbContext() { }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.EnableSensitiveDataLogging();
		}

		public virtual DbSet<User> User { get; set; }

		public virtual DbSet<Gig> Gig { get; set; }
	}
}
