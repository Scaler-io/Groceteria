using Groceteria.IdentityManager.Api.Models.Enums;

namespace Groceteria.IdentityManager.Api.Models.Core
{
    public class ApiExceptionResponse: ApiResponse
    {
        public string StackTrace { get; set; }
        public ApiExceptionResponse(string errorMessages = "", string stackTrace = "")
            :base(ErrorCodes.InternalServerError)
        {
            ErrorMessage = errorMessages ?? GetDefaultMessage(Code);
            StackTrace = stackTrace;
        }
    }
}
