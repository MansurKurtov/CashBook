using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ReferenceApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:6054")
                .UseStartup<Startup>();
    }
}
