using Groceteria.IdentityProvider.Configurations.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            .AddConfigurationStore(options =>
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
            .AddDeveloperSigningCredential();

            return services;
        }
    }
}
