using FluentValidation;
using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;

namespace Groceteria.IdentityManager.Api.Validators.ApiScopes
{
    public class ApiScopeValidator: AbstractValidator<ApiScopeDto>
    {
        public ApiScopeValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithErrorCode(ApiValidationError.ScopeNameNotExist.Code)
                .WithMessage(ApiValidationError.ScopeNameNotExist.Message);

            RuleFor(x => x.DisplayName).NotEmpty()
                .WithErrorCode(ApiValidationError.ScopeDisplayNameNotExist.Code)
                .WithMessage(ApiValidationError.ScopeDisplayNameNotExist.Message);

            RuleFor(x => x.Description).NotEmpty()
                .WithErrorCode(ApiValidationError.ScopeDescriptionNotExist.Code)
                .WithMessage(ApiValidationError.ScopeDescriptionNotExist.Message);
        }
    }
}
