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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using GigTracker.Models;
using GigTracker.Repositories;
using GigTracker.Services;
using GigTracker.Helpers;
using GigTracker.Entities;

namespace GigTracker {
	public class Startup {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services) {

			services.AddControllersWithViews();

			// creating AccountService as a singleton so I can manage CurrentUser
			// this means that AccountService shouldn't or can't consume any oher object that is not a singleton
			// https://codingblast.com/asp-net-core-dependency-injection-cannot-consume-scoped-service/
			services.AddTransient<AccountService>();			

			services.AddTransient<UserService>();
			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IGigRepository, GigRepository>();
			services.AddScoped<ApplicationDbContext>();
			services.AddTransient<UserRepository>();
			services.AddTransient<AccountService>();
			//services.AddSingleton<IClaimsTransformation, LocationClaimsProvider>();
			
			var appSettingsSection = Configuration.GetSection("AppSettings");

			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			//	.AddCookie();
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(o => o.LoginPath = new PathString("/Account/Login"));


			//services.AddAuthentication(x => {
			//	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			//	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			//})
			//.AddJwtBearer(x => {
			//	 x.RequireHttpsMetadata = false;
			//	 x.SaveToken = true;
			//	 x.TokenValidationParameters = new TokenValidationParameters {
			//		 ValidateIssuerSigningKey = true,
			//		 IssuerSigningKey = new SymmetricSecurityKey(key),
			//		 ValidateIssuer = false,
			//		 ValidateAudience = false
			//	 };
			// })
			//.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
			// 	options => {
			// 		options.LoginPath = "/Account/Login";
			// 		options.LogoutPath = "/Account/Logout";
			// });

			services.AddMvc().AddRazorPagesOptions(options => {
				options.Conventions.AllowAnonymousToPage("/Account/Login");
				options.Conventions.AllowAnonymousToPage("/Home/Index");
				options.Conventions.AllowAnonymousToPage("/Home");
				options.Conventions.AllowAnonymousToPage("/");
				options.Conventions.AuthorizeFolder("/Gigs/List");
				options.Conventions.AuthorizeFolder("/User/List");
			});

			//services.AddHttpContextAccessor();

			IServiceCollection s = services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


			//services.AddDefaultIdentity<IdentityUser>()
			// 	.AddRoles<IdentityRole>();
			//apparently can't use this with custom login and such
			//.AddEntityFrameworkStores<ApplicationDbContext>();


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				//app.UseDatabaseErrorPage();
			}
			else {
				app.UseExceptionHandler("/Error");

			}

			app.UseStatusCodePages();
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseRouting();
			app.UseAuthorization(); // must go between UseRouting() and UseEndoints()
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");

				endpoints.MapRazorPages();
			});
			//app.UseMvcWithDefaultRoute();
			//app.UseMvc(routes => {
			//	routes.MapRoute(
			//		name: "default",
			//		template: "{controller=Home}/{action=Index}/{id?}");
			//});

			try {
				using (var serviceScope = app.ApplicationServices.CreateScope()) {
					var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
					IAccountService accountService = serviceScope.ServiceProvider.GetService<IAccountService>();

					//ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
					//AccountService accountService = services.GetRequiredService<AccountService>();

					SeedData.EnsurePopulated(app, context, accountService);
				}

			}
			catch (Exception ex) {
				//var logger = services.GetRequiredService<ILogger<Program>>();
				//logger.LogError(ex, "An error occurred seeding the DB.");
			}

			//CreateRoles(services);

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

			////Here you could create a super user who will maintain the web app
			//var poweruser = new ApplicationUser {

			//	UserName = Configuration["AppSettings:UserName"],
			//	Email = Configuration["AppSettings:UserEmail"],
			//};
			////Ensure you have these values in your appsettings.json file
			//string userPWD = Configuration["AppSettings:UserPassword"];
			//var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

			//if (_user == null) {
			//	var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
			//	if (createPowerUser.Succeeded) {
			//		//here we tie the new user to the role
			//		await UserManager.AddToRoleAsync(poweruser, "Admin");

			//	}
			//}
		}
	}
}
