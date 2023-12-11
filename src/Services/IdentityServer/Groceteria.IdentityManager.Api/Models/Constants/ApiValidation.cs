namespace Groceteria.IdentityManager.Api.Models.Constants
{
    public static class ApiValidationError
    {
        public static ApiValidation ClientIdNotExist = new ApiValidation { Code = "1001", Message = "Api client id is required" };
        public static ApiValidation ClientNameNotExist = new ApiValidation { Code = "1002", Message = "Api client name is required" };
        public static ApiValidation ScopeNameNotExist = new ApiValidation { Code = "2001", Message = "Api scope name is required" };
        public static ApiValidation ScopeDisplayNameNotExist = new ApiValidation { Code = "2002", Message = "Api scope display name is required" };
        public static ApiValidation ScopeDescriptionNotExist = new ApiValidation { Code = "2003", Message = "Api scope description is required" };
        public static ApiValidation ResourceNameRequired = new ApiValidation { Code = "3001", Message = "Api resource name is required" };
        public static ApiValidation ResourceDisplayNameRequired = new ApiValidation { Code = "3002", Message = "Api resource display name is required" };
        public static ApiValidation ResourceDescriptionRequired = new ApiValidation { Code = "3003", Message = "Api resource description is required" };
        public static ApiValidation ResourceSecretValueRequired = new ApiValidation { Code = "3004", Message = "Api resource secret value is required" };
        public static ApiValidation ResourceScopeRequired = new ApiValidation { Code = "3005", Message = "Atleast one scope value is required" };
    }

    public class ApiValidation
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
