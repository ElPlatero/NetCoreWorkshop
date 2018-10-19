using System.IO;
using System.Reflection;
using Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration
{
    public class Startup
    {
        public Startup(IConfiguration configuration) //<- injiziert durch den defaultwebhostbuilder (siehe source)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var hostingEnvironment = services.BuildServiceProvider().GetService<IHostingEnvironment>();

            #region using configurationbuilder
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (hostingEnvironment.IsDevelopment())
            {
                var appAssembly = Assembly.Load(new AssemblyName(hostingEnvironment.ApplicationName));
                if (appAssembly != null) { builder.AddUserSecrets(appAssembly, optional: true); }
            }
            builder.AddEnvironmentVariables();

            IConfiguration configuration = builder.Build();
            #endregion

            #region IOptions-Pattern
            // https://docs.microsoft.com/de-de/aspnet/core/fundamentals/configuration/options?view=aspnetcore-2.1
            services.Configure<ExampleOptions>(configuration); //flach im Json
            services.Configure<ExampleOptions>(options => { options.Name = "configured via delegate"; });
            services.Configure<ExampleOptions>(configuration.GetSection("exampleOptions"));
            #endregion



            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
