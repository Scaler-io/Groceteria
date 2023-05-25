using AutoMapper;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.Shared.Constants;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using MediatR;
using Serilog;

namespace Groceteria.SalesOrder.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<bool>>
    {
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderCommandHandler(ILogger logger, 
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<Result<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - Delete order {@request}", request.DeleteOrderRequest);

            var orderId = request.DeleteOrderRequest.OrderId;
            var orderToDelete = await _orderRepository.GetByIdAsync(request.DeleteOrderRequest.OrderId);

            if(orderToDelete == null)
            {
                _logger.Here().Error("{@ErrorCodes} No order found with id {@OrderId}", orderId);
                return Result<bool>.Failure(ErrorCode.NotFound, ErrorMessages.NotFound);
            }

            await _orderRepository.DeleteAsync(orderToDelete);

            _logger.Here().WithOrderId(orderId).Information("Order delete successful");
            _logger.Here().MethodExited();

            return Result<bool>.Success(true);
        }
    }
}
