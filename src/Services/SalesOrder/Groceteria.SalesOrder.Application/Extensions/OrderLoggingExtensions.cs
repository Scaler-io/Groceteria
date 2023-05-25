using Serilog;

namespace Groceteria.SalesOrder.Application.Extensions
{
    public static class OrderLoggingExtensions
    {
        public static ILogger WithOrderId(this ILogger logger, object orderId)
        {
            logger.ForContext("OrderId", orderId.ToString());
            return logger;
        }
    }
}
