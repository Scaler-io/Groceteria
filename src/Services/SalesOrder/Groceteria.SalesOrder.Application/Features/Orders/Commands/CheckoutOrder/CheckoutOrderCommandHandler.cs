using AutoMapper;
using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Enums;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using MediatR;
using Serilog;

namespace Groceteria.SalesOrder.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, Result<string>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailServiceFactory _emailServiceFactory;

        public CheckoutOrderCommandHandler(ILogger logger,
            IMapper mapper,
            IOrderRepository orderRepository,
            IEmailServiceFactory emailServiceFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _emailServiceFactory = emailServiceFactory;
        }

        public async Task<Result<string>> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - Checkout order {@order}", request.CheckoutOrderRequest);

            var orderEntity = _mapper.Map<Order>(request.CheckoutOrderRequest);
            var placedOrder = await _orderRepository.AddAsync(orderEntity);

            if(placedOrder == null)
            {
                _logger.Here().Error("{@ErrorCode} - Failed to place order - \n {orderEntity}");
                return Result<string>.Failure(ErrorCode.OperationFailed, "Failed to place order");
            }

            var emailService = _emailServiceFactory.GetService(EmailServiceType.OrderPlaced);
            await emailService.SendEmailAsync(orderEntity);

            _logger.Here().WithOrderId(orderEntity.Id).Information("Order placed successfully for {@username}", placedOrder.UserName);
            _logger.Here().MethodExited();
            return Result<string>.Success(placedOrder.Id.ToString());
        }
    }
}
