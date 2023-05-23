using Groceteria.SalesOrder.Application.Models.Dtos;
using MediatR;

namespace Groceteria.SalesOrder.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrderListQuery: IRequest<IEnumerable<OrderDto>>
    {
        public string Username { get; set; }

        public GetOrderListQuery(string username)
        {
            Username = username;
        }
    }
}
