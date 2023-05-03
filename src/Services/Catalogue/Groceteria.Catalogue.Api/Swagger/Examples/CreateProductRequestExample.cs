using Groceteria.Catalogue.Api.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples
{
    public class CreateProductRequestExample : IExamplesProvider<ProductUpsertRequest>
    {
        public ProductUpsertRequest GetExamples()
        {
            return new ProductUpsertRequest
            {
                Id = Guid.NewGuid().ToString(),
                BrandId = Guid.NewGuid().ToString(),
                CategoryId = Guid.NewGuid().ToString(),
                Name = "Test Product",
                Color = "Red",
                Description = "Description",
                Image = "https://fakedomain.co",
                Price = 0,
                SKU = "GRC/TEST-SKU"
            };
        }
    }
}
