using Groceteria.Shared.Enums;

namespace Groceteria.Shared.Core
{
    public class ApiExceptionResponse : ApiResponse
    {
        public string StackTrace { get; set; }
        public ApiExceptionResponse(string stackTrace="", string message = "") 
            : base(ErrorCode.InternalServerError)
        {
            StackTrace = stackTrace;
        }
    }
}
