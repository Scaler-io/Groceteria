using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples
{
    public class ProductResponseExample : IExamplesProvider<ProductResponse>
    {
        public ProductResponse GetExamples()
        {
            return new ProductResponse
            {
                Name = "Test Product",
                Summary = "Test summary",
                Description = "Test description",
                Brand = "Test brand",
                Category= "Test category",
                Color = "Test color",
                Price = 100,
                SKU = "Test SKU",
                MetaData = new MetaData
                {
                    Id = "FAKE_ID",
                    CreatedAt = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss"),
                    UpdatedAt = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss"),
                }
            };
        }
    }
}
