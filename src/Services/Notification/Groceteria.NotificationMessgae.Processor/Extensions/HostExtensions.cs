using Groceteria.NotificationMessgae.Processor.DataAccess;
using Groceteria.Shared.Constants;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using Groceteria.Shared.SharedEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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

            if (!dbContext.NotificationHistories.Any())
            {
                var data = new List<EmailField>
                {
                    new EmailField("[username]", "test username"),
                    new EmailField("[total]", "550.00"),
                    new EmailField("[address]", "test address 1"),
                };

                var data1 = new List<EmailField>
                {
                    new EmailField("[username]", "test username 2"),
                    new EmailField("[total]", "770.00"),
                    new EmailField("[address]", "test address 2"),
                };

                var data2 = new List<EmailField>
                {
                    new EmailField("[username]", "test username 3"),
                    new EmailField("[total]", "220.00"),
                    new EmailField("[address]", "test address 3"),
                };

                var notifictions = new List<NotificationHistory>
                {
                    new NotificationHistory
                    {
                        RecipientEmail = "testuser@email.com",
                        TemplateName = "OrderPlacedEmailTemplate",
                        IsPublished = false,
                        Data = JsonConvert.SerializeObject(data),
                        Subject = "Subject line 1"
                    },
                    new NotificationHistory
                    {
                        RecipientEmail = "testuser2@email.com",
                        TemplateName = "OrderPlacedEmailTemplate",
                        IsPublished = false,
                        Data = JsonConvert.SerializeObject(data1),
                        Subject = "Subject line 2"
                    },
                    new NotificationHistory
                    {
                        RecipientEmail = "testuser3@email.com",
                        TemplateName = "OrderPlacedEmailTemplate",
                        IsPublished = false,
                        Data = JsonConvert.SerializeObject(data2),
                        Subject = "Subject line 3"
                    },
                };
                dbContext.AddRange(notifictions);
                await dbContext.SaveChangesAsync();
            }
            logger.Here().Information("Migration completed");
            return host;
        }
    }
}
