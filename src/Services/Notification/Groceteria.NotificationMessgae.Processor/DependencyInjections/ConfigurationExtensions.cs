using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Groceteria.NotificationMessgae.Processor.DependencyInjections
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigurationSetting(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();

            services.AddSingleton<IConfiguration>(configuration);
            return services;
        }
    }
}
