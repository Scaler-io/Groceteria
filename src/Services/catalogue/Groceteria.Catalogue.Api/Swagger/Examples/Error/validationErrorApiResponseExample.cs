using Groceteria.Shared.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.Catalogue.Api.Swagger.Examples.Error
{
    public class validationErrorApiResponseExample : IExamplesProvider<ApiValidationResponse>
    {
        public ApiValidationResponse GetExamples()
        {
            return new ApiValidationResponse
            {
                Errors = new List<FieldLevelError>
                {
                    new FieldLevelError
                    {
                        Code = "Invalid",
                        Field = "Field 1",
                        Message = "Field 1 value is invalid"
                    }
                }
            };
        }
    }
}
