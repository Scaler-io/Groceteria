using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Helpers;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Groceteria.SalesOrder.Infrastructure.Persistance
{
    public class SalesOrderContextSeed
    {
        public static async Task SeedAsync(SalesOrderContext context, ILogger logger, IWebHostEnvironment environment)
         {           
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(GetSampleOrders(environment, logger));
                await context.SaveChangesAsync();
            }
        }

        public static IEnumerable<Order> GetSampleOrders(IWebHostEnvironment environment, ILogger logger)
        {
            var path = Path.Combine(environment.ContentRootPath, "App_Data/SeedData");
            logger.Information(path);
            var orders = FileReaderHelper<Order>.SeederFileReader("OrderSample", path);
            return orders;
        }
    }
}
