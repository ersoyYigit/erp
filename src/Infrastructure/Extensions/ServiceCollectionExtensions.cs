using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ArdaManager.Application.Interfaces.Repositories;
using ArdaManager.Application.Interfaces.Services.Storage;
using ArdaManager.Application.Interfaces.Services.Storage.Provider;
using ArdaManager.Application.Interfaces.Serialization.Serializers;
using ArdaManager.Application.Serialization.JsonConverters;
using ArdaManager.Infrastructure.Repositories;
using ArdaManager.Infrastructure.Services.Storage;
using ArdaManager.Application.Serialization.Options;
using ArdaManager.Infrastructure.Services.Storage.Provider;
using ArdaManager.Application.Serialization.Serializers;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Infrastructure.Services;

namespace ArdaManager.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
                .AddTransient<IProductRepository, ProductRepository>()
                .AddTransient<ICityRepository, CityRepository>()
                .AddTransient<ICategoryRepository, CategoryRepository>()
                .AddTransient<ICompanyRepository, CompanyRepository>()
                .AddTransient<IVappRepository, VappRepository>()
                .AddTransient<IPatternRepository, PatternRepository>()
                .AddTransient<IDocumentRepository, DocumentRepository>()
                .AddTransient<IDocumentTypeRepository, DocumentTypeRepository>()
                .AddTransient<IRecipeItemRepository, RecipeItemRepository>()
                .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        }

        public static IServiceCollection AddExtendedAttributesUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IExtendedAttributeUnitOfWork<,,>), typeof(ExtendedAttributeUnitOfWork<,,>));
        }

        public static IServiceCollection AddServerStorage(this IServiceCollection services)
            => AddServerStorage(services, null);

        public static IServiceCollection AddRecuringJobs(this IServiceCollection services)
        {
            return services
                .AddScoped<IExchangeService, ExchangeService>();
        }


        public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<SystemTextJsonOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, ServerStorageProvider>()
                .AddScoped<IServerStorageService, ServerStorageService>()
                .AddScoped<ISyncServerStorageService, ServerStorageService>()
                .Configure<SystemTextJsonOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    if (!configureOptions.JsonSerializerOptions.Converters.Any(c => c.GetType() == typeof(TimespanJsonConverter)))
                        configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}