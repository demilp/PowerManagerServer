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

namespace PowerManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BuildWebHost(args).Run();

            var host = WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appSettings.json");
            })
            .UseStartup<Startup>()
            .UseUrls("http://0.0.0.0:63158/")
            .Build();

            host.Run();

        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);
            return WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .Build();
        }
    }
    public class Response {
        public bool success { get; set; }
        public object data { get; set; }
        public string error { get; set;}
    }
}
