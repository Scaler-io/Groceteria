using FluentValidation;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Requests;

namespace Groceteria.SalesOrder.Application.Validators.DeleteOrder
{
    public class DeleteOrderValidation: AbstractValidator<DeleteOrderRequest>
    {
        public DeleteOrderValidation()
        {
            RuleFor(o => o.OrderId)
                .Required();
        }
    }
}
