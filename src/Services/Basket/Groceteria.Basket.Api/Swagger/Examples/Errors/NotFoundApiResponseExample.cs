using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Basket.Api.Swagger.Examples.Errors
{
    public class NotFoundApiResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            return new ApiResponse(ErrorCode.NotFound);
        }
    }
}
