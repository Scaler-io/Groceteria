using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Domain.Enums;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class EmailBackgroundHostedService : BackgroundService, IEmailBackgroundHostedService
    {
        private readonly IEmailServiceFactory _emailServiceFactory;
        private readonly ILogger _logger;
        private readonly SemaphoreSlim _emailSemaphore = new SemaphoreSlim(1);

        public EmailBackgroundHostedService(ILogger logger, 
            IEmailServiceFactory emailServiceFactory)
        {
            _logger = logger;
            _emailServiceFactory = emailServiceFactory;
        }

        public async Task SendMailAsync(object arg, EmailServiceType type)
        {
            await _emailSemaphore.WaitAsync();
            try
            {
                var emailService = _emailServiceFactory.GetService(type); 
                await emailService.SendEmailAsync(arg);
            }
            finally
            {
                _emailSemaphore.Release();
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
