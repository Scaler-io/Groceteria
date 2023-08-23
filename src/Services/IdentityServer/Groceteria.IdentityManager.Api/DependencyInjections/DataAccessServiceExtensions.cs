using Groceteria.Identity.Shared.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.IdentityManager.Api.DependencyInjections
{
    public static class DataAccessServiceExtensions
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<GroceteriaUserContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("UserDb"));
            });

            services.AddConfigurationDbContext(options =>
            {
                options.ConfigureDbContext = c =>
                {
                    c.UseSqlServer(configuration.GetConnectionString("OAuthDb"));
                };
            });
            services.AddOperationalDbContext(options =>
            {
                options.ConfigureDbContext = c =>
                {
                    c.UseSqlServer(configuration.GetConnectionString("OAuthDb"));
                };
            });
            services.AddDbContext<ConfigurationDbContext>();
            services.AddDbContext<PersistedGrantDbContext>();

            return services;
        }
    }
}
