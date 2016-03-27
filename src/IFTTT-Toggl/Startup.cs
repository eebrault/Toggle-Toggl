using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IFTTT_Toggl
{
	// ReSharper disable once UnusedMember.Global
    public class Startup
    {
	    // ReSharper disable once UnusedParameter.Local
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
				.AddJsonFile("config.json")
				.AddJsonFile($"config.{env.EnvironmentName}.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public static IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
	    // ReSharper disable once UnusedMember.Global
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

			// Add logging
	        services.AddLogging();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	    // ReSharper disable once UnusedMember.Global
	    // ReSharper disable once UnusedParameter.Global
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseMvc();
        }

        // Entry point for the application.
	    // ReSharper disable once UnusedMember.Global
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
