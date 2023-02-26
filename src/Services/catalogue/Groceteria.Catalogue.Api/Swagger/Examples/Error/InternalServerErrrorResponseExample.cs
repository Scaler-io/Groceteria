using Groceteria.Shared.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples.Error
{
    public class InternalServerErrrorResponseExample : IExamplesProvider<ApiExceptionResponse>
    {
        public ApiExceptionResponse GetExamples()
        {
            return new ApiExceptionResponse("Error stack trace");
        }
    }
}
