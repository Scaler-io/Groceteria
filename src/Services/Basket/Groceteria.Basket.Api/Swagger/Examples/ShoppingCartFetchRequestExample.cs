using Groceteria.Basket.Api.Models.Requests;
using Groceteria.Shared.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Basket.Api.Swagger.Examples
{
    public class ShoppingCartFetchRequestExample : IExamplesProvider<ShoppingCartFetchRequest>
    {
        public ShoppingCartFetchRequest GetExamples()
        {
            return new ShoppingCartFetchRequest
            {
                Username = "test user"
            };
        }
    }
}
