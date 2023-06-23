using Groceteria.Infrastructure.Logger;
using Groceteria.NotificationMessgae.Processor.Configurations;
using Groceteria.NotificationMessgae.Processor.Services;
using Groceteria.NotificationMessgae.Processor.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var logIndexPattern = $"Groceteria.Notification.Processor-Dev";
            var logger = LoggerConfig.Configure(configuration, logIndexPattern);
            services.AddSingleton(Log.Logger)
                    .AddSingleton(x => logger);

            services.AddScoped<IEmailService, EmailService>();
            services.Configure<EmailSettingsOption>(configuration.GetSection(EmailSettingsOption.EmailSettings));
            return services;
        }
    }
}
