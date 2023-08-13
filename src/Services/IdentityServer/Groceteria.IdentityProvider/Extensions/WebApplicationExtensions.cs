using Groceteria.IdentityProvider.Configurations.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.IdentityProvider.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task MigrateAsycn(this WebApplication app, IConfiguration configuration)
        {
            using (var scope = app.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
                    .Database
                    .MigrateAsync();

                using (var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                        if (!context.Clients.Any())
                        {
                            var clientSettings = configuration.GetSection("DefaultApiClients").Get<DefaultApiClients>();
                            foreach (var client in IdentityConfig.Clients(clientSettings))
                                context.Clients.Add(client.ToEntity());
                        }
                        if (!context.IdentityResources.Any())
                        {
                            foreach (var resource in IdentityConfig.IdentityResources)
                                context.IdentityResources.Add(resource.ToEntity());
                        }
                        if (!context.ApiScopes.Any())
                        {
                            foreach (var apiScope in IdentityConfig.ApiScopes)
                                context.ApiScopes.Add(apiScope.ToEntity());
                        }
                        if (!context.ApiResources.Any())
                        {
                            foreach (var apiResource in IdentityConfig.ApiResources)
                                context.ApiResources.Add(apiResource.ToEntity());
                        }
                        await context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        logger.Error("error migrating database {@stack}", e.StackTrace);
                        throw;
                    }
                }
            }
        }
    }
}
