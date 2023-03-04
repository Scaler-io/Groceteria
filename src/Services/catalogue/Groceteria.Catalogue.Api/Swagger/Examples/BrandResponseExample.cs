using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples
{
    public class BrandResponseExample : IExamplesProvider<BrandResponse>
    {
        public BrandResponse GetExamples()
        {
            return new BrandResponse
            {
                Name = "Test brand",
                Description = "Test brand description",
                ImageLink = "Test image link",
                MetaData = new MetaData
                {
                    Id = "FAKE_ID",
                    CreatedAt = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss"),
                    UpdatedAt = DateTime.UtcNow.ToString("dd/MM/yyyy hh:mm:ss")
                }
            };
        }
    }
}
