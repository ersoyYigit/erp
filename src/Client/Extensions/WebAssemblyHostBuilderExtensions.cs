using Blazored.LocalStorage;
using ArdaManager.Client.Infrastructure.Authentication;
using ArdaManager.Client.Infrastructure.Managers;
using ArdaManager.Client.Infrastructure.Managers.Preferences;
using ArdaManager.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using ArdaManager.Client.Infrastructure.Managers.ExtendedAttribute;
using ArdaManager.Domain.Entities.ExtendedAttributes;
using ArdaManager.Domain.Entities.Misc;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Radzen;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components.Web;
using ArdaManager.Application.Interfaces.Common;
using Microsoft.Extensions.Configuration;
using ArdaManager.Shared.Constants.Application;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using ArdaManager.Client.Infrastructure.Managers.GlobalVariable;
using MudBlazor.Extensions;
using MudBlazor.Extensions.Options;
using System.Drawing.Drawing2D;
using AutoMapper;

namespace ArdaManager.Client.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        private const string ClientName = "Vapp.API";

        public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            return builder;
        }
        public class NoCorsHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.Headers.Add("Access-Control-Allow-Origin", "*");
                request.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                request.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-Auth-Token, Origin");

                if (request.Method == HttpMethod.Options)
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
                    response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-Auth-Token, Origin");
                    return response;
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {
            //var apiaddress = builder.Configuration["BaseAddress"];

            builder
                .Services
                .AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .AddAuthorizationCore(options =>
                {
                    RegisterPermissionClaims(options);
                })
                .AddBlazoredLocalStorage()
                .AddMudServices(configuration =>
                {
                    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                    configuration.SnackbarConfiguration.HideTransitionDuration = 300;
                    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
                    configuration.SnackbarConfiguration.VisibleStateDuration = 4000;
                    configuration.SnackbarConfiguration.ShowCloseIcon = false;

                })
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddAutoMapperProfiles()
                .AddScoped<ClientPreferenceManager>()
                .AddScoped<VappStateProvider>()
                .AddScoped<AuthenticationStateProvider, VappStateProvider>()
                .AddManagers()
                //.AddGlobals()
                .AddExtendedAttributeManagers()
                .AddTransient<AuthenticationHeaderHandler>()
                //.AddTransient<NoCorsHandler>()
                .AddScoped(sp => sp
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient(ClientName).EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                    /// <summary>
                    /// TODO: Publish
                    /// </summary>

                    var baseAddressPath = builder.Configuration["BaseAddress"];
                    client.BaseAddress = new Uri(baseAddressPath);
                    
                })
                //.AddHttpMessageHandler<NoCorsHandler>()
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();


            ApplicationConstants.Initialize(builder.Configuration);
            builder.Logging.SetMinimumLevel(LogLevel.None);
            builder.Services.AddHttpClientInterceptor();
            builder.Services.AddScoped<Radzen.DialogService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<TooltipService>();
            builder.Services.AddScoped<ContextMenuService>();
            
            
            //builder.Services.AddScoped<ContextMenuService>();
            //builder.Services.AddBlazorContextMenu();

            return builder;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            var managers = typeof(IManager);

            var types = managers
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (managers.IsAssignableFrom(type.Service))
                {
                    if(type.Service.Name.Contains("lobal"))
                        services.AddScoped(type.Service, type.Implementation);
                    else
                        services.AddTransient(type.Service, type.Implementation);
                }
            }

            return services;
        }


        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            var profileBaseType = typeof(Profile);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //var assemblies = AppDomain.CurrentDomain.GetAssemblies().Concat(new[] { Assembly.GetExecutingAssembly() }).ToArray();


            foreach (var assembly in assemblies)
            {
                var profileTypes = assembly
                       .GetTypes()
                       .Where(t => typeof(IProfileConfiguration).IsAssignableFrom(t) && !t.IsAbstract);
/*
                var profileTypes = assembly
                    .GetExportedTypes()
                    .Where(t => profileBaseType.IsAssignableFrom(t) && !t.IsAbstract);
*/
                foreach (var profileType in profileTypes)
                {
                    services.AddSingleton(profileBaseType, profileType);
                }
            }

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(assemblies);
            });

            return services;
        }


        public static IServiceCollection AddExtendedAttributeManagers(this IServiceCollection services)
        {
            

            return services
                .AddTransient(typeof(IExtendedAttributeManager<int, int, Document, DocumentExtendedAttribute>), typeof(ExtendedAttributeManager<int, int, Document, DocumentExtendedAttribute>));
        }

        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                }
            }
        }
    }
}