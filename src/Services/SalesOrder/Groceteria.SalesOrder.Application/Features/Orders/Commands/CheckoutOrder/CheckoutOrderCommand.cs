using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.Shared.Core;
using MediatR;

namespace Groceteria.SalesOrder.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand: IRequest<Result<string>>
    {
        public CheckoutOrderRequest CheckoutOrderRequest { get; set; }
    }
}
