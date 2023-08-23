using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.IdentityManager.Api.Swagger.Examples.ErrorExamples
{
    public class NotFoundErrorExample : IExamplesProvider<ApiResponse>
    {
        public ApiResponse GetExamples()
        {
            return new ApiResponse(ErrorCodes.NotFound, ErrorMessages.NotFound);
        }
    }
}
