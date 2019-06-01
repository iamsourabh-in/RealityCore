using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Reality.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }


        //AddJsonFile is automatically called twice when you initialize a new WebHostBuilder with CreateDefaultBuilder. The method is called to load configuration from:

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("customconfig.json", optional: false, reloadOnChange: false);
            })
            .ConfigureLogging((hostingContext, logging) => { logging.AddConsole(); })
            .UseStartup<Startup>();

        public static IWebHostBuilder CreateInlineWebHostBuilder(string[] args) =>
                   WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(configureServices => { })
            .Configure(appBuilder =>
            {
                appBuilder.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
            });


    }
}
