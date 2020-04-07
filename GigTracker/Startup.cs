using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GigTracker.Models;


namespace GigTracker {
	public class Startup {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services) {
			services.AddTransient<IUserRepository, FakeUserRepository>();
			services.AddTransient<IGigRepository, FakeGigRepository>();
			services.AddMvc();
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

			app.UseStaticFiles();

			// Runs matching. An endpoint is selected and set on the HttpContext if a match is found.
			app.UseRouting();

			// Middleware that run after routing occurs. Usually the following appear here:
			//app.UseAuthentication();
			//app.UseAuthorization();
			//app.UseCors();
			// These middleware can take different actions based on the endpoint.

			// Executes the endpoint that was selected by routing.
			app.UseEndpoints(endpoints =>
			{
				// Mapping of endpoints goes here:
				endpoints.MapGet("/", async context => {
				});

				endpoints.MapControllers();
				endpoints.MapRazorPages();
				//endpoints.MapHub<MyChatHub>();
				//endpoints.MapGrpcService<MyCalculatorService>();
		
			});

			// Middleware here will only run if nothing was matched.

			app.UseStatusCodePages();
			//app.UseMvc(routes => { 

			//});

			app.UseRouting();

		}
	}
}
