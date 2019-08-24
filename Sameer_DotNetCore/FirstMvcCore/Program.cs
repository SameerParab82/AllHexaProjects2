using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FirstMvcCore
{
    public class Program
    {
        static Dictionary<string, string> appSetting = new Dictionary<string, string>()
        {
            {"Name","Sameer Parab" },
            {"Email","SameerParab82@gmail.com" }
        };
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
                config.AddEventSourceLogger();
                config.SetMinimumLevel(LogLevel.Warning);
            })
            //.ConfigureKestrel(Options =>
            //{
            //    Options.
            //});
           /* .ConfigureAppConfiguration(option =>
            {
                option.SetBasePath(Directory.GetCurrentDirectory())
                                    // This are by default configured by default builder
                                    //.AddJsonFile("appsetting.json", optional: true)  // order 1
                                    //.AddEnvironmentVariables()  // order 2
                                    //.AddCommandLine(args);  // order 3
                                    .AddInMemoryCollection(appSetting)  // order 4
                                    .AddXmlFile("mysetting.xml", optional: true)
                                    .AddIniFile("mysetting.ini", optional: true)
                                    .AddJsonFile("mysetting.json", optional: true)
                                    .AddKeyPerFile("FileDir", optional: true);

            })*/
            .UseStartup<Startup>();
    }
}
