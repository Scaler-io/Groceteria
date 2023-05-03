using Groceteria.Catalogue.Api.Configurations;
using Groceteria.Catalogue.Api.DataAccess;
using Groceteria.Catalogue.Api.DataAccess.Repositories;

namespace Groceteria.Catalogue.Api.DependencyInjections
{
    public static class ApplicationDataLayerExtensions
    {
        public static IServiceCollection AddDataLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            // mongodb
            services.Configure<MongodbSettingOptions>(configuration.GetSection(MongodbSettingOptions.MongodbSettings));
            services.AddTransient<ICatalogueContext, CatalogueContext>();
            services.AddTransient(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            return services;
        }
    }
}
