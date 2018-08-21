using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace VersionManagement
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional:true) // The build of web host and the build of the app(Startup.cs) are seperate. So different config files can be used.
                .Build())
            .UseKestrel()
            .UseStartup<Startup>()
            .UseSetting("detailedErrors", "true")
            .CaptureStartupErrors(true);
    }
}
