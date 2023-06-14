using AutoMapper;
using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Models.Email;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.SalesOrder.Domain.Enums;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Serilog;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Groceteria.SalesOrder.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, Result<string>>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailQueue _emailQueue;

        public CheckoutOrderCommandHandler(ILogger logger,
            IMapper mapper,
            IOrderRepository orderRepository,
            IEmailQueue emailQueue)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _emailQueue = emailQueue;
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

            // scehdule for sending order placed email
            //var emailQueuTask = new EmailQueueTask
            //{
            //    EmailServiceType = EmailServiceType.OrderPlaced,
            //    OrderEntity = orderEntity
            //};
            //_emailQueue.Queue.Enqueue(emailQueuTask);

            _logger.Here().WithOrderId(orderEntity.Id).Information("Order placed successfully for {@username}", placedOrder.UserName);
            _logger.Here().MethodExited();
            return Result<string>.Success(placedOrder.Id.ToString());
        }
    }
}
