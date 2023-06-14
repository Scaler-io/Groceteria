using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.Shared.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailQueue _emailQueue;
        private readonly ILogger _logger;

        public EmailBackgroundService(IEmailQueue emailQueue, ILogger logger)
        {
            _emailQueue = emailQueue;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_emailQueue.Queue.Any())
                {
                    _logger.Information("queue details with queue {@count}", _emailQueue.Queue.Count);
                }

                await Task.Delay(1000);
                _logger.Information("queue details without queue");
            }
        }
    }
}
