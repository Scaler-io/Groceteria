using AutoMapper;
using Groceteria.Infrastructure.EventBus.Message.Events.BasketEvents;
using Groceteria.Infrastructure.EventBus.Message.Models.BasketCheckout;
using Groceteria.SalesOrder.Application.Models.Requests;

namespace Groceteria.SalesOrder.Application.Mappers
{
    public class BasketCheckoutConsumerMappingProfile: Profile
    {
        public BasketCheckoutConsumerMappingProfile()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderRequest>();
            CreateMap<OrderItem, OrderItemRequest>();
            CreateMap<BillingAddress, BillingDetailsRequest>();
            CreateMap<PaymentDetails, PaymentDetailsRequest>();
        }
    }
}
