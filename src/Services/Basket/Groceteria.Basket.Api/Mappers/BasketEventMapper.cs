using AutoMapper;
using Groceteria.Basket.Api.Models.Requests.BasketCheckout;
using Groceteria.Infrastructure.EventBus.Message.Events.BasketEvents;
using Groceteria.Infrastructure.EventBus.Message.Models.BasketCheckout;

namespace Groceteria.Basket.Api.Mappers
{
    public class BasketEventMapper: Profile
    {
        public BasketEventMapper()
        {
            CreateMap<BasketCheckoutRequest, BasketCheckoutEvent>();
            CreateMap<OrderItemRequest, OrderItem>();
            CreateMap<BillingAddressRequest, BillingAddress>();
            CreateMap<PaymentDetailsRequest, PaymentDetails>();
        }
    }
}
