using FluentValidation;
using Groceteria.IdentityManager.Api.Models.Dtos;

namespace Groceteria.IdentityManager.Api.Validators.ApiClients
{
    public class ApiClientValidator : AbstractValidator<ApiClientDto>
    {
        public ApiClientValidator()
        {
            RuleFor(c => c.ClientId)
                .NotEmpty()
                .WithMessage("Client id is required");
            RuleFor(c => c.ClientName)
                .NotEmpty()
                .WithMessage("Client name is required");
            RuleFor(c => c.ClientDescription)
                .NotEmpty()
                .WithMessage("Client description is required");
        }
    }
}
