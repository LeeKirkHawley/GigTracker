using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.IO;

namespace GigTracker {
	public class Program {
		public static void Main(string[] args) {
			var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;
            }

            host.Run();
		}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder => 
                 {
                    webBuilder.UseStartup<Startup>();
                     webBuilder.UseKestrel(options => 
                     {
                         options.Listen(IPAddress.Loopback, 5001);
                         options.Listen(IPAddress.Loopback, 5002, listenOptions => 
                         {
                             listenOptions.UseHttps(".\\test-certificate.pfx", "testpassword");
                         }
                     );}
                );
          });
    }
}
