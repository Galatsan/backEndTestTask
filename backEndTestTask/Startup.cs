using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using backEndTestTask.Interfaces;
using backEndTestTask.Interfaces.Services;
using backEndTestTask.Mappings;
using backEndTestTask.Models;
using backEndTestTask.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace backEndTestTask
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var typesWithMappings = new List<Type>()
            {
                typeof(AutomapperProfile)
            };

            var assembliesWithMappings = typesWithMappings
                .Select(t => t.Assembly)
                .Concat(new List<Assembly>() { Assembly.GetExecutingAssembly() });

            services.AddAutoMapper(assembliesWithMappings);
            services.Configure<ImagesThirdPartySetting>(Configuration.GetSection("ImagesThirdParty"));
            services.AddTransient<IClient, Client>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddSingleton<IImagesThirdPartyService, ImagesThirdPartyService>();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
