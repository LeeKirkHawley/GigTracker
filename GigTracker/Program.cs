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
using System.Reflection;
using NLog.Web;
using NLog;


namespace GigTracker {
	public class Program {

        static Logger logger = null;

        public static void Main(string[] args) {
            logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Debug("Entering Main()");


            string argsString = "";
            foreach(string str in args) {
                argsString += str;
                argsString += " ";
            }

            IHostBuilder hostBuilder = null;
            IHost host = null;

            try {
                logger.Debug($"Calling CreateHostBuilder() - {argsString}");
                hostBuilder = CreateHostBuilder(args);
                logger.Debug($"Calling Build() - {argsString}");
                host = hostBuilder.Build();
                logger.Debug("host built");
            }
            catch(Exception ex) {
                logger.Debug(ex);
            }

            //using (var scope = host.Services.CreateScope()) {
            //    var services = scope.ServiceProvider;
            //}

            logger.Debug("Calling Host.Run()");
            host.Run();

            logger.Debug("Leaving Main()");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            logger.Debug("Entering CreateHostBuilder()");

            IHostBuilder hostBuilder = null;

            try {
                hostBuilder = Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder => {
                       webBuilder.UseStartup<Startup>();
                   })
                   .ConfigureLogging(logging => {
                   //logging.ClearProviders();
                   //logging.SetMinimumLevel(LogLevel.Information);
               })
                  .UseNLog();
            }
            catch(Exception ex) {
                logger.Debug(ex);
            }

            logger.Debug("Leaving CreateHostBuilder()");
            return hostBuilder;
        }
    }
}
