using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using GigTracker.Models;

namespace GigTracker.Data {
	public class ApplicationDbContext : DbContext{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<User> User { get; set; }

		public DbSet<Gig> Gig { get; set; }
	}
}
