using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.IdentityManager.Api.Swagger.Examples.ErrorExamples
{
    public class BadRequestErrorExample : IExamplesProvider<ApiValidationResponse>
    {
        public ApiValidationResponse GetExamples()
        {
            return new ApiValidationResponse(ErrorMessages.BadRequest);
        }
    }
}
