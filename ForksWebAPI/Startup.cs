using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ForksWebAPI.BackgroundTask;
using ForksWebAPI.DATA;
using ForksWebAPI.Models;
using LiveForks.Admin.Providers.Positiv;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace ForksWebAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ForksDbContext>(options =>
                  options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddHostedService<ConsumeParserService>();
            services.AddScoped<IValidAuthentication, ValidAuthentication>();
            //services.AddScoped<IParserPositiveService, ParserPositiveService>();
            services.AddScoped<IServiceForksClient, PositivClient>();
            //services.AddSingleton<IServiceForksClient, PositivClient>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseDatabaseErrorPage();


            app.UseMvc();
        }
    }
}
