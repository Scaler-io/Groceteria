using Groceteria.NotificationMessgae.Processor.DataAccess;
using Groceteria.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor.Extensions
{
    public static class HostExtensions
    {
        public static async Task<IHost> MigrateDbAsync(this IHost host)
        {
            var dbContext = host.Services.GetRequiredService<NotificationProcessorContext>();
            var logger = host.Services.GetRequiredService<ILogger>();
            if(dbContext == null)
            {
                logger.Here().Error("No db context has been found");
                return host;
            }

            logger.Here().Information("Migration started");
            await dbContext.Database.MigrateAsync();
            logger.Here().Information("Migration completed");

            await NotificationProcessorSeeder.SeedNotificationsAsync(logger, dbContext);

            return host;
        }
    }
}
