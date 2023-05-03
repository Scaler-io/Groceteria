using Groceteria.Discount.Grpc.DataAccess.Repositories;

namespace Groceteria.Discount.Grpc.DependencyInjections
{
    public static class DataLayerServiceExtensions
    {
        public static IServiceCollection AddDataLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            return services;
        }
    }
}
