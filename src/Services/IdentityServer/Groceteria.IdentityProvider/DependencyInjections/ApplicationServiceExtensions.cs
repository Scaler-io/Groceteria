using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Groceteria.IdentityProvider.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<GroceteriaUserContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("UserDb"), sql =>
                {
                    sql.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                });
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
