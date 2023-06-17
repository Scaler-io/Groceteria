using Groceteria.NotificationMessgae.Processor.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Groceteria.NotificationMessgae.Processor.DependencyInjections
{
    public static class DataServiceExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotificationProcessorContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("NotificationProcessor"));
            });
            return services;
        }
    }
}
