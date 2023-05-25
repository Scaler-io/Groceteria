using FluentValidation;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.SalesOrder.Application.Validators.CheckoutOrder.Rules;

namespace Groceteria.SalesOrder.Application.Validators.CheckoutOrder
{
    public class CheckoutOrderValidator: AbstractValidator<CheckoutOrderRequest>
    {
        public CheckoutOrderValidator()
        {
            RuleFor(p => p.Username)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();

            RuleFor(p => p.TotalPrice)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            
            RuleFor(p => p.OrderedItems)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(p => p.BillingAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required()
                .SetValidator(new BillingAddressValidationRules());
            RuleFor(p => p.PaymentDetails)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required()
                .SetValidator(new PaymentDetailsValidationRules());
        }
    }
}
