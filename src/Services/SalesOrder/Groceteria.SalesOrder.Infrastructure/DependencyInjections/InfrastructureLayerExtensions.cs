using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Infrastructure.Email;
using Groceteria.SalesOrder.Infrastructure.Email.Factory;
using Groceteria.SalesOrder.Infrastructure.Persistance;
using Groceteria.SalesOrder.Infrastructure.Repositories;
using Groceteria.SalesOrder.Infrastructure.Repositories.Orders;
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

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEmailBackgroundHostedService, EmailBackgroundHostedService>();
            services.AddScoped<IEmailServiceFactory, EmailServiceFactory>();
            services.AddScoped<IEmailService, OrderPlacedEmailService>();
            services.AddHostedService<EmailBackgroundHostedService>();

            services.Configure<EmailSettingsOption>(configuration.GetSection(EmailSettingsOption.EmailSettings));
            return services;
        }
    }
}
