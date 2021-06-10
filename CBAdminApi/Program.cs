using System;
using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CBAdminApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CashBook Admin Api";
            Console.WriteLine($@"Process Id: {Process.GetCurrentProcess().Id}");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("log.txt", LogEventLevel.Debug, rollingInterval: RollingInterval.Day)
                .CreateLogger();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:6052")
                .UseSerilog()
                .UseStartup<Startup>();
    }
}

