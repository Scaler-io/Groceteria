namespace Groceteria.ApiGateway.Models.Core
{
    public class ErrorResponse
    {
        public ErrorResponse(int code, string errorMessage, string stackTrace = null)
        {
            Code = code;
            ErrorMessage = errorMessage;
            StackTrace = stackTrace;
        }

        public int Code { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
