using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNet.Identity;
using GigTracker.Models;


namespace GigTracker {
	public class Startup {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services) {
			services.AddTransient<IUserRepository, FakeUserRepository>();
			services.AddTransient<IGigRepository, FakeGigRepository>();
			services.AddTransient<IAccountService, AccountService>();
			//services.AddSingleton<AccountService>();

			services.AddAuthentication(	CookieAuthenticationDefaults.AuthenticationScheme)
					.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
						options => {
							options.LoginPath = "/Account/Login";
							options.LogoutPath = "/Account/Logout";
					});

			//			services.AddMvc(options => options.EnableEndpointRouting = false).AddRazorPagesOptions(options => {
			services.AddMvc().AddRazorPagesOptions(options => {
				options.Conventions.AllowAnonymousToPage("/Account/Login");
				options.Conventions.AllowAnonymousToPage("/Home/Index");
				options.Conventions.AuthorizeFolder("/Gigs/List");
				options.Conventions.AuthorizeFolder("/User/List");
			});

			// authentication 
			// Enable cookie authentication
			//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			//		.AddCookie();

			services.AddHttpContextAccessor();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
			//services.AddDistributedMemoryCache();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
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
		}
	}
}
