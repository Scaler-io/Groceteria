using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples
{
    public class NoContentResponseExample : IExamplesProvider<object>
    {
        public object GetExamples()
        {
            return new object();
        }
    }
}
