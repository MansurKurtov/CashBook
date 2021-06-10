using System;
using System.Diagnostics;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace OcelotApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CashBook Ocelot Api";
            Console.WriteLine($@"Process Id: {Process.GetCurrentProcess().Id}");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:6050")
                .UseStartup<Startup>();
    }
}
