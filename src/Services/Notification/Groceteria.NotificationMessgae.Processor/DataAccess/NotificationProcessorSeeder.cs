using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using Groceteria.Shared.Helpers;
using Groceteria.Shared.SharedEntities;
using Microsoft.Data.SqlClient;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor.DataAccess
{
    public class NotificationProcessorSeeder
    {
        public static async Task SeedNotificationsAsync(ILogger logger, NotificationProcessorContext context)
        {
            logger.Here().Information("Notification data seeding started");
            try
            {
                if (!context.NotificationHistories.Any())
                {
                    var notifications = FileReaderHelper<NotificationHistory>.SeederFileReader("notifications", "./DataAccess/Seeders");
                    context.NotificationHistories.AddRange(notifications);
                    await context.SaveChangesAsync();
                }
                
            }
            catch(SqlException ex)
            {
                logger.Here().Error("Sql expetion occured - {@message} - {@stack}", ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                logger.Here().Error("{@errorcode} - {@message} - {@stack}", ErrorCode.InternalServerError, ex.Message, ex.StackTrace);
            }
        }
    }
}
