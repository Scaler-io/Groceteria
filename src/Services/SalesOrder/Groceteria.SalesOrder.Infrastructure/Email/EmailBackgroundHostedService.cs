using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class EmailBackgroundHostedService : BackgroundService, IEmailBackgroundHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SemaphoreSlim _emailSemaphore = new SemaphoreSlim(1);

        public EmailBackgroundHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task SendMailAsync(object arg, EmailServiceType type)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                await _emailSemaphore.WaitAsync();
                var emailServiceFactory = scope.ServiceProvider.GetRequiredService<IEmailServiceFactory>();
                try
                {
                    var emailService = emailServiceFactory.GetService(type);
                    await emailService.SendEmailAsync(arg);
                }
                finally
                {
                    _emailSemaphore.Release();
                }
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
