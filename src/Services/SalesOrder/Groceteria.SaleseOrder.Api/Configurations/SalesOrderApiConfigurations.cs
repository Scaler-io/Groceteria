using Groceteria.Shared.Core;
using Microsoft.AspNetCore.Mvc;

namespace Groceteria.SaleseOrder.Api.Configurations
{
    public class SalesOrderApiConfigurations
    {
        public static Func<ActionContext, IActionResult> HandleFrameworkValidationFailure()
        {
            return context =>
            {
                var errors = context.ModelState
                                    .Where(error => error.Value.Errors.Count > 0).ToList();
                var validationError = new ApiValidationResponse
                {
                    Errors = new List<FieldLevelError>()
                };
                foreach (var error in errors)
                {
                    var fieldLevelError = new FieldLevelError()
                    {
                        Code = "Invalid",
                        Field = error.Key,
                        Message = error.Value.Errors?.First().ErrorMessage,
                    };

                    validationError.Errors.Add(fieldLevelError);
                }
                return new UnprocessableEntityObjectResult(validationError);
            };
        }
    }
}
