using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.Shared.Core;
using MediatR;

namespace Groceteria.SalesOrder.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand: IRequest<Result<bool>>
    {
        public DeleteOrderRequest DeleteOrderRequest { get; set; }
    }
}
