using Groceteria.IdentityManager.Api.Models.Enums;

namespace Groceteria.IdentityManager.Api.Models.Core
{
    public class ApiValidationResponse: ApiResponse
    {
        public ApiValidationResponse(string errorMessages = "")
            :base(ErrorCodes.BadRequest)
        {
            ErrorMessage = errorMessages;
        }

        public List<FieldLevelError> Errors { get; set; }
    }
}
