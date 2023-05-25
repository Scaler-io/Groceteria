using FluentValidation;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Requests;

namespace Groceteria.SalesOrder.Application.Validators.CheckoutOrder.Rules
{
    public class PaymentDetailsValidationRules: AbstractValidator<PaymentDetailsRequest>
    {
        public PaymentDetailsValidationRules()
        {
            RuleFor(pd => pd.PaymentMethod)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(pd => pd.CardName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(pd => pd.CardNumber)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(pd => pd.CVV)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(pd => pd.Expiration)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
        }
    }
}
