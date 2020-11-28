using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Configuration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {            
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var switchMappings = new Dictionary<string, string>()
            {
                { "-k1", "key1" },
                { "-k2", "key2" },
                { "--alt3", "key3" },
                { "--alt4", "key4" },
                { "--alt5", "key5" },
                { "--alt6", "key6" },
            };
            
            var dict = new Dictionary<string, string>
            {
                {"MyKey", "Dictionary MyKey Value"},
                {"Position:Title", "Dictionary_Title"},
                {"Position:Name", "Dictionary_Name" },
                {"Logging:LogLevel:Default", "Warning"}
            };
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    // config.Sources.Clear();
                    
                    // config.AddIniFile("MyIniConfig.ini", optional: true, reloadOnChange: true);
                    config.AddJsonFile("MyIniConfig1.json", optional: true, reloadOnChange: true);
                    config.AddCommandLine(args, switchMappings);

                    string path = Path.Combine(Directory.GetCurrentDirectory(), "ConfigFiles");
                    config.AddKeyPerFile(path, true);

                    //config.AddInMemoryCollection(dict);
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}