using Groceteria.SalesOrder.Application.Models.Dtos;
using MediatR;
using Serilog;

namespace Groceteria.SalesOrder.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, IEnumerable<OrderDto>>
    {
        private readonly ILogger _logger;

        public GetOrderListQueryHandler(ILogger logger)
        {
            _logger = logger;
        }

        public Task<IEnumerable<OrderDto>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
