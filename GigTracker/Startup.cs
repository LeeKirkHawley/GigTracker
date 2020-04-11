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
using GigTracker.Models;
using GigTracker.Data;
using GigTracker.Services;
using GigTracker.Helpers;

namespace GigTracker {
	public class Startup {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }
		

		public void ConfigureServices(IServiceCollection services) {

			services.AddControllersWithViews();

			services.AddTransient<IUserRepository, UserRepository>();
			services.AddTransient<IGigRepository, GigRepository>();
			services.AddScoped<ApplicationDbContext>();
			services.AddTransient<UserService>();
			services.AddTransient<IAccountService, AccountService>();
			services.AddTransient<AccountService>();
			
			var appSettingsSection = Configuration.GetSection("AppSettings");

			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(x => {
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x => {
				 x.RequireHttpsMetadata = false;
				 x.SaveToken = true;
				 x.TokenValidationParameters = new TokenValidationParameters {
					 ValidateIssuerSigningKey = true,
					 IssuerSigningKey = new SymmetricSecurityKey(key),
					 ValidateIssuer = false,
					 ValidateAudience = false
				 };
			 })
			.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
			 	options => {
			 		options.LoginPath = "/Account/Login";
			 		options.LogoutPath = "/Account/Logout";
			 });

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

		}
	}
}