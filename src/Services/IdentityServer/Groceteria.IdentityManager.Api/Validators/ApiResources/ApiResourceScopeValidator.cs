using FluentValidation;
using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;

namespace Groceteria.IdentityManager.Api.Validators.ApiResources;

public class ApiResourceScopeValidator : AbstractValidator<ApiResourceScopeDto>
{
    public ApiResourceScopeValidator()
    {
        RuleFor(s => s.Scope)
        .NotEmpty()
        .WithErrorCode(ApiValidationError.ResourceScopeRequired.Code)
        .WithMessage(ApiValidationError.ResourceScopeRequired.Message);
    }
}
