using AutoMapper;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Constants;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using MediatR;
using Serilog;

namespace Groceteria.SalesOrder.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Result<Unit>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderCommandHandler(ILogger logger, 
            IMapper mapper, 
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<Result<Unit>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - Updte order {@request}", request.UpdateOrderRequest);

            var orderId = request.UpdateOrderRequest.OrderId;
            var orderToUpdate = await _orderRepository.GetByIdAsync(orderId);

            if (orderToUpdate == null)
            {
                _logger.Here().Error("{@ErrorCodes} No order found with id {@orderId}", ErrorCode.NotFound, orderId);
                return Result<Unit>.Failure(ErrorCode.NotFound, ErrorMessages.NotFound);
            }

            _mapper.Map(request.UpdateOrderRequest, orderToUpdate, typeof(UpdateOrderRequest), typeof(Order));

            await _orderRepository.UpdateAsync(orderToUpdate);

            _logger.Here().WithOrderId(orderId).Information("Order updated successfully");
            _logger.Here().MethodExited();

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
