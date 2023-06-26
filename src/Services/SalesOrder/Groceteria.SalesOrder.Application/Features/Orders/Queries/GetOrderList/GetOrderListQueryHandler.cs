using Groceteria.SalesOrder.Application.Models.Dtos;
using MediatR;
using Serilog;
using Groceteria.Shared.Extensions;
using AutoMapper;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Constants;

namespace Groceteria.SalesOrder.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, Result<Pagination<OrderDto>>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public GetOrderListQueryHandler(ILogger logger, IMapper mapper, IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<Result<Pagination<OrderDto>>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - get order list for {@username}", request.Username);

            var orderList = await _orderRepository.GetOrdersByUserName(request.Username, request.Query);
            var orderCount = await _orderRepository.GetCount(o => o.UserName == request.Username);

            if (orderList == null || orderList.Count() == 0)
            {
                _logger.Here().Error("{@ErrorCode} - No order was found.", ErrorCode.NotFound);
                return Result<Pagination<OrderDto>>.Failure(ErrorCode.NotFound, ErrorMessages.NotFound);
            }

            var result = _mapper.Map<IReadOnlyList<OrderDto>>(orderList);

            _logger.Here().Information("Order list fetch successfull");
            _logger.Here().MethodExited();
            return Result<Pagination<OrderDto>>.Success(new Pagination<OrderDto>(request.Query.PageIndex, request.Query.PageSize, orderCount, result));
        }
    }
}
