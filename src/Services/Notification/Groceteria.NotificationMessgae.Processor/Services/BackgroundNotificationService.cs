using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor.Services
{
    public class BackgroundNotificationService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public BackgroundNotificationService(IConfiguration configuration,
            ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {               
                await Task.Delay(TimeSpan.FromMinutes(Convert.ToInt32(_configuration["ProcessInterval"])), stoppingToken);
            }
        }
    }
}
