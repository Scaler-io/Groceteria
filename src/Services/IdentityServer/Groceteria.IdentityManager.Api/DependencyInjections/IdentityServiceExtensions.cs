using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Entities;
using Microsoft.AspNetCore.Identity;

namespace Groceteria.IdentityManager.Api.DependencyInjections
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<GroceteriaUserContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
