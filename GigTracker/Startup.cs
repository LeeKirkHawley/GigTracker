using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Services;
using GigTracker.Helpers;
using GigTracker.Entities;
using NLog.Web;
using NLog;


namespace GigTracker {
	public class Startup {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services) {

			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			logger.Debug("Entering ConfigureServices()");


			//services.AddHttpsRedirection(options => {
			//	options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
			//	options.HttpsPort = 5002;
			//});

			services.AddMemoryCache();

			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromSeconds(10);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			services.AddControllersWithViews();

			services.AddSession(so =>
			{
				so.IdleTimeout = TimeSpan.FromSeconds(60);
			});
			services.AddMvc();

			try {
				IServiceCollection s = services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
			}
			catch(Exception ex) {
				logger.Debug(ex, "AddDbContext exception"); ;
			}

			services.AddTransient<UserRepository>();
			//services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IGigRepository, GigRepository>();
			services.AddTransient<AccountService>();
			services.AddTransient<UserService>();

			services.AddScoped<ApplicationDbContext>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			var appSettingsSection = Configuration.GetSection("AppSettings");

			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);


			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, 
								IWebHostEnvironment env, 
								IServiceProvider services) {

			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			logger.Debug("Entering Configure()");

			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseExceptionHandler("/Home/Error");
			}

			//app.UseStatusCodePages();
			app.UseDefaultFiles();
			app.UseStaticFiles(); // For the wwwroot folder
			//app.UseHttpsRedirection();  // force redirect to https
			app.UseFileServer();
			app.UseSession();
			app.UseRouting();
			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});

			try {
				using (var serviceScope = app.ApplicationServices.CreateScope()) {
					var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
					IAccountService accountService = serviceScope.ServiceProvider.GetService<AccountService>();

					logger.Debug("Calling EnsurePopulated()");
					SeedData.EnsurePopulated(app, context, accountService);
				}
			}
			catch (Exception ex) {
				logger.Debug(ex, "An error occurred seeding the DB.");
			}
        }

		private async Task CreateRoles(IServiceProvider serviceProvider) {
			//initializing custom roles 
			var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
			string[] roleNames = { "Admin", "Manager", "Member" };
			IdentityResult roleResult;

			foreach (var roleName in roleNames) {
				bool roleExist = await RoleManager.RoleExistsAsync(roleName);
				if (!roleExist) {
					//create the roles and seed them to the database: Question 1
					roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
				}
			}
		}
	}
}
