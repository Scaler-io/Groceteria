using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Helpers;
using Serilog;

namespace Groceteria.SalesOrder.Infrastructure.Persistance
{
    public class SalesOrderContextSeed
    {
        public static async Task SeedAsync(SalesOrderContext context, ILogger logger)
        {
            if(!context.Orders.Any())
            {
                context.Orders.AddRange(GetSampleOrders());
                await context.SaveChangesAsync();
            }
        }

        public static IEnumerable<Order> GetSampleOrders()
        {
            var orders = FileReaderHelper<Order>.SeederFileReader("OrderSample", "./Persistence/Seeders");
            return orders;
        }
    }
}
