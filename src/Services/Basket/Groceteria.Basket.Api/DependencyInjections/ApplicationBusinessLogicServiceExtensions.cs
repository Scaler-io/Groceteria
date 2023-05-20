using Groceteria.Basket.Api.Services.Grpc;
using Groceteria.Basket.Api.Services.Interfaces.Grpc;
using Groceteria.Basket.Api.Services.Interfaces.v2;
using Groceteria.Basket.Api.Services.v2;

namespace Groceteria.Basket.Api.DependencyInjections
{
    public static class ApplicationBusinessLogicServiceExtensions
    {
        public static IServiceCollection AddBusinessLogiceServices(this IServiceCollection services)
        {
            services.AddScoped<IProductSearchService, ProductSearchService>();
            services.AddScoped<IBasketWorkflowService, BasketWorkflowService>();
            services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();
            return services;
        }
    }
}
