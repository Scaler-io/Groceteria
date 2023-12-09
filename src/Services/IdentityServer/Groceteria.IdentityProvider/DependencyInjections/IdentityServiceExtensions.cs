using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityProvider.Configurations.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Groceteria.IdentityProvider.DependencyInjections
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultApiClients = configuration.GetSection("DefaultApiClients").Get<DefaultApiClients>();

            services.AddIdentityServer(options =>
            {
                options.EmitStaticAudienceClaim = true;
            })
            .AddConfigurationStore<GroceteriaOauthDbContext>(options =>
            {
                options.ConfigureDbContext = c =>
                c.UseSqlServer(
                    configuration.GetConnectionString("OAuthDb"),
                    sql => sql.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().ToString())
                );
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = c =>
                c.UseSqlServer(
                    configuration.GetConnectionString("OAuthDb"),
                    sql => sql.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().ToString())
                );
            })
            .AddAspNetIdentity<AppUser>()
            .AddDeveloperSigningCredential();
            return services;
        }
    }
}
