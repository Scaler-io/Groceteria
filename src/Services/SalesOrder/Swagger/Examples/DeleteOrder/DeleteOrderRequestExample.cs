using Groceteria.SalesOrder.Application.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger.Examples.DeleteOrder
{
    public class DeleteOrderRequestExample : IExamplesProvider<DeleteOrderRequest>
    {
        public DeleteOrderRequest GetExamples()
        {
            return new DeleteOrderRequest
            {
                OrderId = Guid.NewGuid().ToString(),
            };
        }
    }
}
