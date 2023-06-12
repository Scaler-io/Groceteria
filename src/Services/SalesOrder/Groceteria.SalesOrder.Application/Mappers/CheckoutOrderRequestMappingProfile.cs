using AutoMapper;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.SalesOrder.Domain.Entities;

namespace Groceteria.SalesOrder.Application.Mappers
{
    public class CheckoutOrderRequestMappingProfile: Profile
    {
        public CheckoutOrderRequestMappingProfile()
        {
            CreateMap<CheckoutOrderRequest, Order>();
            CreateMap<OrderItemRequest, OrderItem>();
            CreateMap<BillingDetailsRequest, BillingAddress>();
            CreateMap<PaymentDetailsRequest, PaymentDetails>();
        }
    }
}
