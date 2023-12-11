using FluentValidation;
using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;

namespace Groceteria.IdentityManager.Api.Validators.ApiResources
{
    public class ApiResorceValidator : AbstractValidator<ApiResourceDto>
    {
        public ApiResorceValidator()
        {
            RuleFor(r => r.Name)
            .NotEmpty()
            .WithErrorCode(ApiValidationError.ResourceNameRequired.Code)
            .WithMessage(ApiValidationError.ResourceNameRequired.Message);

            RuleFor(r => r.DisplayName)
            .NotEmpty()
            .WithErrorCode(ApiValidationError.ResourceDisplayNameRequired.Code)
            .WithMessage(ApiValidationError.ResourceDisplayNameRequired.Message);

            RuleFor(r => r.Description)
            .NotEmpty()
            .WithErrorCode(ApiValidationError.ResourceDescriptionRequired.Code)
            .WithMessage(ApiValidationError.ResourceDescriptionRequired.Message);

            RuleFor(r => r.Secrets)
            .ForEach(item =>
            {
                item.SetValidator(new ApiResourceSecretValidator())
                .When(i => i.Any());
            });

            RuleFor(r => r.Scopes)
            .ForEach(item =>
            {
                item.SetValidator(new ApiResourceScopeValidator())
                .When(i => i.Any());
            });
        }
    }
}