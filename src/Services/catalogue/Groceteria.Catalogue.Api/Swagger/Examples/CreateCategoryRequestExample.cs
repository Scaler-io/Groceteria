using Groceteria.Catalogue.Api.Models.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples
{
    public class CreateCategoryRequestExample : IExamplesProvider<CategoryUpsertRequest>
    {
        public CategoryUpsertRequest GetExamples()
        {
            return new CategoryUpsertRequest
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test Category",
                Description = "description",
                Image = "https://fakeimagedomain.co",
            };
        }
    }
}
