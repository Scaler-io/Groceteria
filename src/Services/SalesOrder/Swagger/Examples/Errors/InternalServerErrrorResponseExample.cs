using Groceteria.Shared.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger.Examples.Errors
{
    public class InternalServerErrrorResponseExample : IExamplesProvider<ApiExceptionResponse>
    {
        public ApiExceptionResponse GetExamples()
        {
            return new ApiExceptionResponse("Error stack trace");
        }
    }
}
