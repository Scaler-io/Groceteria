using Groceteria.SalesOrder.Application.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Groceteria.SalesOrder.Application.DependencyInjections
{
    public static class BusinessLayerExtensions
    {
        public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettingsOption>(configuration.GetSection(EmailSettingsOption.EmailSettings));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
