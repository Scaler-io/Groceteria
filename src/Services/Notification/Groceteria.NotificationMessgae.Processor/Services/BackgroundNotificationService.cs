using Groceteria.NotificationMessgae.Processor.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor.Services
{
    public class BackgroundNotificationService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public BackgroundNotificationService(IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await emailService.SendMailAsync();
                await Task.Delay(TimeSpan.FromMinutes(Convert.ToInt32(_configuration["ProcessInterval"])), stoppingToken);
            }
        }
    }
}
