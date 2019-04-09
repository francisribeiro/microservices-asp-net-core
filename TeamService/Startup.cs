using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TeamService.LocationClient;
using TeamService.Persistence;

namespace TeamService
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public static string[] Args { get; set; } = new string[] { };
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true)
                 .AddEnvironmentVariables()
                 .AddCommandLine(Args);

            _configuration = builder.Build();

            _loggerFactory = loggerFactory;
            loggerFactory.AddConsole(LogLevel.Information);
            loggerFactory.AddDebug();

            _logger = _loggerFactory.CreateLogger("Startup");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddScoped<ITeamRepository, MemoryTeamRepository>();

            var locationUrl = _configuration.GetSection("location:url").Value;
            _logger.LogInformation("Using {0} for location service URL.", locationUrl);
            services.AddSingleton<ILocationClient>(new HttpLocationClient(locationUrl));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
