using FluentValidation;
using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;

namespace Groceteria.IdentityManager.Api.Validators.ApiResources
{
    public class ApiResourceSecretValidator : AbstractValidator<ApiResourceSecretDto>
    {
        public ApiResourceSecretValidator()
        {
            RuleFor(s => s.Value)
            .NotEmpty()
            .WithErrorCode(ApiValidationError.ResourceSecretValueRequired.Code)
            .WithMessage(ApiValidationError.ResourceSecretValueRequired.Message);
        }
    }
}