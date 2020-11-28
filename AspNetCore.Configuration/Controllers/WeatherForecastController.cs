using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Configuration.Interfaces;
using AspNetCore.Configuration.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNetCore.Configuration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfigurationRoot _configurationRoot;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _configurationRoot = configuration as IConfigurationRoot;
            var env = hostEnvironment.EnvironmentName;
            var setting = configuration["a"];
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            string providersString = string.Empty;
            var allProviders = this._configurationRoot.Providers;
            foreach (var provider in allProviders)
            {
                providersString += provider.ToString() + "\n";
            }
            
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpGet("options")]
        public string TestIOptions([FromServices]IOptions<PositionOptions> options)
        {
            return PrintInfo(options.Value);
        }
        
        [HttpGet("optionssnapshot")]
        public string TestIOptionsSnapshot([FromServices]IOptionsSnapshot<PositionOptions> options)
        {
            return PrintInfo(options.Value);
        }
        
        [HttpGet("optionssnapshot2")]
        public async Task<string> TestIOptionsSnapshot2([FromServices]IOptionsSnapshot<PositionOptions> options)
        {
            var rest = PrintInfo(options.Value);
            await Task.Delay(10000);
            return rest + "\t" + PrintInfo(options.Value);
        }
        
        [HttpGet("optionsmonitor")]
        public string TestIOptionsMonitor([FromServices]IOptionsMonitor<PositionOptions> options)
        {
            return PrintInfo(options.CurrentValue);
        }
        
        [HttpGet("optionsmonitor2")]
        public async Task<string> TestIOptionsMonitor2([FromServices]IOptionsMonitor<PositionOptions> options)
        {
            var rest = PrintInfo(options.CurrentValue);
            await Task.Delay(10000);
            return rest + "\t" + PrintInfo(options.CurrentValue);
        }
        
        [HttpGet("namedoptions")]
        public string TestNamedIOptionsSnapshot([FromServices]IOptionsMonitor<TopItemSettings> options)
        {
            var option = options.Get(TopItemSettings.Month);
            return $"Name:{option.Name}\tModel:{option.Model}";
        }
        
        [HttpGet("validation")]
        public string ValidateOptions([FromServices]IOptions<ValidateItem> options)
        {
            try
            {
                var setting = options.Value;
                return "OK";
            }
            catch (OptionsValidationException e)
            {
                return $"Error:{e.Message}";
            }
        }

        [HttpGet("work")]
        public async Task<PositionOptions> DoWork([FromServices]IWork work)
        {
            var name = await work.DoSomeWork();
            return new PositionOptions() {Name = name, Title = "HttpGet"};
        }

        private string PrintInfo(PositionOptions options)
        {
            return $"Name:{options.Name}\tTitle:{options.Title}";
        }
    }
}