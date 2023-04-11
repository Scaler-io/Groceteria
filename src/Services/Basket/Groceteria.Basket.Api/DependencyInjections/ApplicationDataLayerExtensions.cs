using Groceteria.Basket.Api.DataAccess;
using Groceteria.Basket.Api.DataAccess.Interfaces;

namespace Groceteria.Basket.Api.DependencyInjections
{
    public static class ApplicationDataLayerExtensions
    {
        public static IServiceCollection AdddataLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, BasketRepository>();
            return services;
        }
    }
}
