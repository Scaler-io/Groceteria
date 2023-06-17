using Groceteria.NotificationMessgae.Processor.DataAccess;
using Groceteria.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groceteria.NotificationMessgae.Processor.Extensions
{
    public static class HostExtensions
    {
        public static async Task<IHost> MigrateDbAsync(this IHost host)
        {
            var dbContext = host.Services.GetRequiredService<NotificationProcessorContext>();
            var logger = host.Services.GetRequiredService<ILogger>();
            logger.Here().Information("Migration started");
            if(dbContext == null)
            {
                logger.Here().Error("No db context has been found");
                return host;
            }

            await dbContext.Database.MigrateAsync();
            logger.Here().Information("Migration completed");
            return host;
        }
    }
}
