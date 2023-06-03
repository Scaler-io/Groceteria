using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Infrastructure.Email;
using Groceteria.SalesOrder.Infrastructure.Email.Factory;
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

            services.AddScoped(typeof(IBaseRepository<>), typeof(IBaseRepository<>));
            services.AddScoped<IOrderRepository, IOrderRepository>();
            services.AddScoped<IEmailBackgroundHostedService, EmailBackgroundHostedService>();
            services.AddScoped<IEmailServiceFactory, EmailServiceFactory>();
            services.AddScoped<IEmailService, OrderPlacedEmailService>();
            services.AddHostedService<EmailBackgroundHostedService>();
            return services;
        }
    }
}
