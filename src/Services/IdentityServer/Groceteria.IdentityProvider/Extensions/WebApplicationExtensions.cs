using AutoMapper;
using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityProvider.Configurations.Client;
using Groceteria.IdentityProvider.DataAccess;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.IdentityProvider.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task MigrateAsycn(this WebApplication app, IConfiguration configuration)
        {
            if (app.Environment.IsDevelopment())
            {
                using (var scope = app.Services.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                    await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
                        .Database
                        .MigrateAsync();

                    using (var context = scope.ServiceProvider.GetRequiredService<GroceteriaOauthDbContext>())
                    {
                        try
                        {
                            await context.Database.MigrateAsync();
                            if (!context.ApiClients.Any())
                            {
                                var clientSettings = configuration.GetSection("DefaultApiClients").Get<DefaultApiClients>();
                                foreach (var client in IdentityConfig.Clients(clientSettings))
                                {
                                    var clientEntity = client.ToEntity();
                                    var apiClient = mapper.Map<ApiClient>(clientEntity);
                                    context.ApiClients.Add(apiClient);
                                }
                            }
                            if (!context.IdentityResources.Any())
                            {
                                foreach (var resource in IdentityConfig.IdentityResources)
                                    context.IdentityResources.Add(resource.ToEntity());
                            }
                            if (!context.ApiScopesExtended.Any())
                            {
                                foreach (var apiScope in IdentityConfig.ApiScopes)
                                {
                                    var scopeEntity = apiScope.ToEntity();
                                    var apiScopeExtended = mapper.Map<ApiScopeExtended>(scopeEntity);
                                    context.ApiScopesExtended.Add(apiScopeExtended);
                                }
                            }
                            if (!context.ApiResourcesExtended.Any())
                            {
                                foreach (var apiResource in IdentityConfig.ApiResources)
                                {
                                    var apiResourceEntity = apiResource.ToEntity();
                                    var apiResourceExtended = mapper.Map<ApiResourceExtended>(apiResourceEntity);
                                    context.ApiResourcesExtended.Add(apiResourceExtended);
                                }
                            }
                            await context.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            logger.Error("error migrating database {@stack}", e.StackTrace);
                            throw;
                        }
                    }

                    using (var context = scope.ServiceProvider.GetRequiredService<GroceteriaUserContext>())
                    {
                        try
                        {
                            await context.Database.MigrateAsync();
                            if (!context.Users.Any())
                            {
                                await UserContextSeed.SeedDefaultAdminAndRoleAsync(logger, userManager, roleManager);
                            }

                        }
                        catch (Exception e)
                        {
                            logger.Here().Error("Failed to seed default user and role. {@stackTrace}", e.StackTrace);
                        }
                    }
                }
            }
        }
    }
}
