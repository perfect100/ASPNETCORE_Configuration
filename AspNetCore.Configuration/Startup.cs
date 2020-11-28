using System;
using AspNetCore.Configuration.Interfaces;
using AspNetCore.Configuration.Models;
using AspNetCore.Configuration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCore.Configuration
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Check
            if (!this.Configuration.GetSection("NotExistSection").Exists())
            {
                Console.WriteLine("Not Exist Section.");
            }
            
            // Get Item
            string notExsit = Configuration["NotExistSection"];
            string name = Configuration["Position:Name"];
            //int age = Configuration["Position:Age"];
            int age = int.Parse(Configuration["Position:Age"]);
            string envValue = Configuration["EnvValue"];
            string envEmail = Configuration["Person:Email"];
            string fileValue = Configuration["Value:From:File"];
            
            // Get Object: Bind + Get
            // Bind
            PositionOptions options = new PositionOptions();
            this.Configuration.Bind(PositionOptions.Position, options);
            // Get
            var getOption = this.Configuration.GetSection(PositionOptions.Position).Get<PositionOptions>();
            var getValueWithDefault = this.Configuration.GetValue<int>("abc", 100);
            
            // Injection
            services.Configure<PositionOptions>(Configuration.GetSection(PositionOptions.Position));

            // Named options
            services.Configure<TopItemSettings>(TopItemSettings.Month, Configuration.GetSection("TopItem:Month"));
            services.Configure<TopItemSettings>(TopItemSettings.Year, Configuration.GetSection("TopItem:Year"));
            
            // validation
            services.AddOptions<ValidateItem>()
                .Bind(Configuration.GetSection("ValidateItem"))
                .ValidateDataAnnotations();

            // // post configuration
            // services.PostConfigure<ValidateItem>(options =>
            // {
            //     options.Age = 40;
            // });

            services.AddScoped<IWork, MyWork>();

            services.AddHttpClient("MyobServer");
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}