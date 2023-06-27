using FluentValidation;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.SalesOrder.Application.Validators.Rules;

namespace Groceteria.SalesOrder.Application.Validators.UpdateOrder
{
    public class UpdateOrderValidation: AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderValidation()
        {
            RuleFor(o => o.OrderId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();

            RuleFor(o => o.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();

            RuleFor(o => o.OrderedItems)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();

            RuleFor(o => o.TotalPrice)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();

            RuleFor(o => o.BillingAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required()
                .SetValidator(new BillingAddressValidationRules());

            RuleFor(o => o.PaymentDetails)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required()
                .SetValidator(new PaymentDetailsValidationRules());
        }
    }
}
