using Groceteria.SalesOrder.Application.Models.Dtos;
using Groceteria.Shared.Core;
using MediatR;

namespace Groceteria.SalesOrder.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrderListQuery: IRequest<Result<Pagination<OrderDto>>>
    {
        public string Username { get; set; }
        public RequestQuery Query { get; set; }

        public GetOrderListQuery(string username, RequestQuery query)
        {
            Username = username;
            Query = query;
        }
    }
}
