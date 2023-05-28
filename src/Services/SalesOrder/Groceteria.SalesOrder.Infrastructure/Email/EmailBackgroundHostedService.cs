using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class EmailBackgroundHostedService : IHostedService
    {
        private readonly IEmailService _emailService;

        public EmailBackgroundHostedService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => RunEmailSendingLoop(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task RunEmailSendingLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Perform email sending task here
                //await _emailService.SendEmailAsync();

                // Delay before processing the next email (optional)
                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }
    }
}
