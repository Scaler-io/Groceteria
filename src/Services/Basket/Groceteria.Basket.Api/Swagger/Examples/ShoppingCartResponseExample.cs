using Groceteria.Basket.Api.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Basket.Api.Swagger.Examples
{
    public class ShoppingCartResponseExample : IExamplesProvider<ShoppingCartResponse>
    {
        public ShoppingCartResponse GetExamples()
        {
            return new ShoppingCartResponse("test user")
            {
                BasketTotal = 130,
                Items = new List<ShoppingCartItemResponse>
                {
                    new ShoppingCartItemResponse
                    {
                        Name = "Test item name",
                        Brand = "Test brand",
                        Category = "Test category",
                        Color = "Test color",
                        Description = "Description",
                        Image = "http://fake-image.jpg",
                        Price = 130,
                        ProductId = Guid.NewGuid().ToString(),
                        Quantity = 1,
                        SKU = "Test SKU",
                        Summary = "Test summary"
                    }
                },
                MetaData = new Models.Core.Metadata
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTimeOffset.Now.Ticks.ToString(),
                    UpdatedAt = DateTimeOffset.Now.Ticks.ToString()
                }
            };
        }
    }
}
