using Groceteria.Catalogue.Api.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples
{
    public class CreateBrandRequestExample : IExamplesProvider<BrandUpsertRequest>
    {
        public BrandUpsertRequest GetExamples()
        {
            return new BrandUpsertRequest
            {
                Name = "Test brand",
                Description = "Test brand description",
                Image = "https://fake.png"
            };
        }
    }
}
