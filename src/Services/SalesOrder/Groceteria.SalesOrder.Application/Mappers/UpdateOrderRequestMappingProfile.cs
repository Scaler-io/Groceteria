using AutoMapper;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.SalesOrder.Domain.Entities;

namespace Groceteria.SalesOrder.Application.Mappers
{
    public class UpdateOrderRequestMappingProfile: Profile
    {
        public UpdateOrderRequestMappingProfile()
        {
            CreateMap<UpdateOrderRequest, Order>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.OrderId));

            CreateMap<BillingDetailsRequest, BillingAddress>();
            CreateMap<PaymentDetailsRequest, PaymentDetails>();
        }
    }
}
