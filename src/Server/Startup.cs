using ArdaManager.Application.Extensions;
using ArdaManager.Infrastructure.Extensions;
using ArdaManager.Server.Extensions;
using ArdaManager.Server.Middlewares;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.IO;
using ArdaManager.Server.Filters;
using ArdaManager.Server.Managers.Preferences;
using Microsoft.Extensions.Localization;
using Serilog;
using ArdaManager.Shared.Constants.Application;
using static ArdaManager.Shared.Constants.Application.ApplicationConstants;
using Microsoft.AspNetCore.DataProtection;
using ArdaManager.Application.Interfaces.Services;
using System;
using Hangfire.SqlServer;
using System.Configuration;

namespace ArdaManager.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: "VappSpec",
                    builder =>
                    {
                        builder.AllowAnyOrigin()//WithOrigins("https://app.ardaglassware.com","https:localhost:5001", "https://api.ardaglassware.com:44398")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        ;//.SetIsOriginAllowed((host) => true);
                    });
            });
            
            services.AddSignalR();
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddCurrentUserService();
            services.AddSerialization();
            services.AddDatabase(_configuration);
            services.AddServerStorage(); //TODO - should implement ServerStorageProvider to work correctly!
            services.AddScoped<ServerPreferenceManager>();
            services.AddServerLocalization();
            services.AddIdentity();
            services.AddJwtAuthentication(services.GetApplicationSettings(_configuration));
            services.AddApplicationLayer();
            services.AddApplicationServices();
            services.AddRepositories();
            services.AddRecuringJobs();
            services.AddExtendedAttributesUnitOfWork();
            services.AddSharedInfrastructure(_configuration);
            services.RegisterSwagger();
            //DI Container
            services.AddInfrastructureMappings();
            //DI Container
            services.AddHangfire(x => x.UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection")));
            /*
            services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection"),
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    UsePageLocksOnDequeue = true,
                    DisableGlobalLocks = true
                }));
            */
            services.AddDataProtection().SetApplicationName("ArdaManager");

            // Bu satýrý ekleyin: Hangfire için kapsamlý hizmet saðlayýcýlarý destekler.
            services.AddHangfireServer(options => options.ServerName = _configuration.GetConnectionString("HangfireServerName"));
            //services.AddHangfireServer();
            services.AddControllers().AddValidators();
            services.AddExtendedAttributesValidators();
            services.AddExtendedAttributesHandlers();
            services.AddRazorPages();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
            services.AddLazyCache();
            
            ImageExtensions.Initialize(_configuration);
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 44398; // The port number of your HTTPS endpoint
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseCors("VappSpec");

            

            app.UseExceptionHandling(env);
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            //app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            /*
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Files")),
                RequestPath = new PathString("/Files")
            });
            */
            app.UseRequestLocalizationByCulture();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(policy =>
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                DashboardTitle = "Vapp Jobs",
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var myService = serviceScope.ServiceProvider.GetService<IExchangeService>();

                // Belirlediðiniz saatte çalýþacak iþi ekleyin (örneðin her gün saat 16:00)
                RecurringJob.AddOrUpdate("exchangerates-job", () => myService.GetExchangeRatesByDate(null), "0 20 * * 1-5");
            }

            app.UseEndpoints();
            app.ConfigureSwagger();
            app.Initialize(_configuration);



            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            
        }
    }
}