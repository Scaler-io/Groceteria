using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples.Error
{
    public class BadRequestApiResponseExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            return new ApiResponse(ErrorCode.BadRequest);
        }
    }
}
