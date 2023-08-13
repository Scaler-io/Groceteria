namespace Groceteria.IdentityProvider.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            return services;
        }
    }
}
