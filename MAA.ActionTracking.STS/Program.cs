using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;

namespace MAA.ActionTracking.STS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            //Seed default tenant
            MAA.ActionTracking.WebHost.Infrastructures.Helpers.DbHelper.SeedData.EnsureSeedData(host.Services);

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
          Microsoft.AspNetCore.WebHost.CreateDefaultBuilder(args)
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  logging.AddConsole();
                  logging.AddDebug();
                  logging.AddSerilog(new LoggerConfiguration()
                      .MinimumLevel.Debug()
                      .WriteTo.RollingFile("Logs/identity-{Date}.log")
                      .Filter.ByIncludingOnly(Matching.FromSource("IdentityServer4"))
                      .CreateLogger());
              })            
            .ConfigureLogging((hostingContext, logging) =>
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddSerilog(new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.RollingFile("Logs/host-{Date}.log")
                    .Filter.ByIncludingOnly(Matching.FromSource("MAA.ActionTracking.STS"))
                    .CreateLogger());
            })
            .UseStartup<Startup>();
    }
}
