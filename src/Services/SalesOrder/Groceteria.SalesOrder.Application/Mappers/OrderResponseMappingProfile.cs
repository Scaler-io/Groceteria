using AutoMapper;
using Groceteria.SalesOrder.Application.Models.Dtos;
using Groceteria.SalesOrder.Domain.Entities;

namespace Groceteria.SalesOrder.Application.Mappers
{
    public class OrderResponseMappingProfile: Profile
    {
        public OrderResponseMappingProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetadataDto
                {
                    CreatedAt = s.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss t"),
                    LastModifiedAt = s.LastModifiedAt.ToString("dd/MM/yyyy hh:mm:ss t"),
                    CreatedBy = s.CreatedBy,
                    LastModifiedBy = s.LastModifiedBy
                }));
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<BillingAddress, BillingAddressDto>();
        }
    }
}
