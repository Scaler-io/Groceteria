using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.Shared.Core;
using MediatR;

namespace Groceteria.SalesOrder.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand: IRequest<Result<Unit>>
    {
        public UpdateOrderRequest UpdateOrderRequest { get; set; }
    }
}
