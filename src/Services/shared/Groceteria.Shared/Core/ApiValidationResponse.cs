using Groceteria.Shared.Enums;

namespace Groceteria.Shared.Core
{
    public class ApiValidationResponse : ApiResponse
    {
        public ApiValidationResponse(string message = "") 
            : base(ErrorCode.BadRequest)
        {
            Message = message ??  GetDefaultMessage(ErrorCode.BadRequest);
        }

        public List<FieldLevelError> Errors { get; set; }
    }
}
