using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples
{
    public class CreateBrandRequestExample : IExamplesProvider<bool>
    {
        public bool GetExamples()
        {
            return true;
        }
    }
}
