using Groceteria.SalesOrder.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Groceteria.SalesOrder.Infrastructure.DependencyInjections
{
    public static class InfrastructureLayerExtensions
    {
        public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<SalesOrderContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString"));
                option.EnableDetailedErrors();
            });
            return services;
        }
    }
}
