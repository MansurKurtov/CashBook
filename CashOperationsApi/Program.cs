using System;
using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CashOperationsApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CashBook Operations Api";
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
                .UseUrls("http://*:6053")
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
