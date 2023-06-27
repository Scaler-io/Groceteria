using FluentValidation;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Requests;

namespace Groceteria.SalesOrder.Application.Validators.Rules
{
    public class OrderItemValidationRules: AbstractValidator<OrderItemRequest>
    {
        public OrderItemValidationRules()
        {
            RuleFor(i => i.Name)
                .Required();
            RuleFor(i => i.ProductId)
                .Required();
            RuleFor(i => i.Quantity)
                .Required();
            RuleFor(i => i.Price)
                .Required();
            RuleFor(i => i.Brand)
                .Required();
            RuleFor(i => i.Category)
                .Required();
            RuleFor(i => i.Description)
                .Required();
        }
    }
}
