using FluentValidation;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Requests;

namespace Groceteria.SalesOrder.Application.Validators.CheckoutOrder.Rules
{
    public class BillingAddressValidationRules: AbstractValidator<BillingDetailsRequest>
    {
        public BillingAddressValidationRules()
        {
            RuleFor(ba => ba.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.EmailAddress)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.Mobile)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.AlternateMobile)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.AddressLine)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.City)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.State)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
            RuleFor(ba => ba.ZipCode)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Required();
        }
    }
}
