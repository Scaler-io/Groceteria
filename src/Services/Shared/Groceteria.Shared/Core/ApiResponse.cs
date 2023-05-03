using Groceteria.Shared.Constants;
using Groceteria.Shared.Enums;

namespace Groceteria.Shared.Core
{
    public class ApiResponse
    {
        public ApiResponse(ErrorCode code, string message="") 
        {
            Code = code;
            Message = string.IsNullOrEmpty(message) ? GetDefaultMessage(code): message;
        }

        public ErrorCode Code { get; set; }
        public string Message { get; set; }

        protected virtual string GetDefaultMessage(ErrorCode code)
        {
            return code switch
            {   
                ErrorCode.BadRequest => ErrorMessages.BadRequest,
                ErrorCode.NotFound   => ErrorMessages.NotFound,
                ErrorCode.Unauthorized => ErrorMessages.Unauthorized,
                ErrorCode.OperationFailed => ErrorMessages.Operationfailed,
                ErrorCode.InternalServerError => ErrorMessages.InternalServerError,
                ErrorCode.UnprocessableEntity => ErrorMessages.UnprocessableEntity,
                _ => string.Empty
            };
        }
    }
}
