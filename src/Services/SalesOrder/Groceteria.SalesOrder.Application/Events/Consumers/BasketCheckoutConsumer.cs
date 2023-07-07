using AutoMapper;
using Groceteria.Infrastructure.EventBus.Message.Events.BasketEvents;
using Groceteria.SalesOrder.Application.Features.Orders.Commands.CheckoutOrder;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.Shared.Extensions;
using MassTransit;
using MediatR;
using Serilog;

namespace Groceteria.SalesOrder.Application.Events.Consumers
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BasketCheckoutConsumer(ILogger logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            _logger.Here().Information("Message processing {@messageId}", context.MessageId);
            _logger.Here().Information("{@message}", context.Message);

            var checkoutRequest = _mapper.Map<CheckoutOrderRequest>(context.Message);

            await _mediator.Send(new CheckoutOrderCommand { CheckoutOrderRequest = checkoutRequest});
        }
    }
}
