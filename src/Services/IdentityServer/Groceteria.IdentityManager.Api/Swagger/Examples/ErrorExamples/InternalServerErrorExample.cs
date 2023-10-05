using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Core;
using Swashbuckle.AspNetCore.Filters;

namespace Groceteria.IdentityManager.Api.Swagger.Examples.ErrorExamples
{
    public class InternalServerErrorExample : IExamplesProvider<ApiExceptionResponse>
    {
        public ApiExceptionResponse GetExamples()
        {
            return new ApiExceptionResponse(ErrorMessages.InternalServerError, "System.DividedByZeroException: Attempted to divide by zero.\r\n   at MyNamespace.MyClass.PerformDivision(Int32 numerator, Int32 denominator) in C:\\Path\\To\\Your\\File.cs:line 20\r\n   at MyNamespace.MyClass.Main() in C:\\Path\\To\\Your\\File.cs:line 10\r\n");
        }
    }
}
