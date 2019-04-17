using LocationReporter.Events;
using LocationReporter.Models;
using LocationReporter.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LocationReporter
{
    public class Startup
    {
        public IConfiguration _configuration { get; }

        public Startup(
            IConfiguration configuration,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            _configuration = configuration;

            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();

            services.Configure<AMQPOptions>(_configuration.GetSection("amqp"));
            services.Configure<TeamServiceOptions>(_configuration.GetSection("teamservice"));

            services.AddSingleton(typeof(IEventEmitter), typeof(AMQPEventEmitter));
            services.AddSingleton(typeof(ICommandEventConverter), typeof(CommandEventConverter));
            services.AddSingleton(typeof(ITeamServiceClient), typeof(HttpTeamServiceClient));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            ITeamServiceClient teamServiceClient,
            IEventEmitter eventEmitter)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts(); 

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
