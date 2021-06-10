using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CashBook Auth server";
            Console.WriteLine($@"Process Id: {Process.GetCurrentProcess().Id}");
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
               WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:6051")
                .UseStartup<Startup>();
    }
}
