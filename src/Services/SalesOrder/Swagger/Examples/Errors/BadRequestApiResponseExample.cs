using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Swagger.Examples.Errors
{
    public class BadRequestApiResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            return new ApiResponse(ErrorCode.BadRequest);
        }
    }
}
