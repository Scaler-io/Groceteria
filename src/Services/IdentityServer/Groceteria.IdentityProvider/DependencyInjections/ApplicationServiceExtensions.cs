using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityProvider.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.IdentityProvider.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<GroceteriaUserContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("UserDb"));
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<GroceteriaUserContext>()
            .AddDefaultTokenProviders();

            services.AddControllersWithViews();

            return services;
        }
    }
}
