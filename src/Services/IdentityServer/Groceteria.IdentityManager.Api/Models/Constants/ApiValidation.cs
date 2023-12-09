namespace Groceteria.IdentityManager.Api.Models.Constants
{
    public static class ApiValidationError
    {
        public static ApiValidation ClientIdNotExist = new ApiValidation { Code = "1001", Message = "Api client id is required" };
        public static ApiValidation ClientNameNotExist = new ApiValidation { Code = "1002", Message = "Api client name is required" };
        public static ApiValidation ScopeNameNotExist = new ApiValidation { Code = "2001", Message = "Api scope name is required" };
        public static ApiValidation ScopeDisplayNameNotExist = new ApiValidation { Code = "2002", Message = "Api scope display name is required" };
        public static ApiValidation ScopeDescriptionNotExist = new ApiValidation { Code = "2003", Message = "Api scope description is required" };
    }

    public class ApiValidation
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
