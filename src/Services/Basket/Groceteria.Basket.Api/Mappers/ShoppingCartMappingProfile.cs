using AutoMapper;
using Groceteria.Basket.Api.Entities;
using Groceteria.Basket.Api.Models.Core;
using Groceteria.Basket.Api.Models.Responses;

namespace Groceteria.Basket.Api.Mappers
{
    public class ShoppingCartMappingProfile: Profile
    {
        public ShoppingCartMappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartResponse>()
                .ForMember(s => s.Username, o => o.MapFrom(d => d.Username))
                .ForMember(s => s.BasketTotal, o => o.MapFrom(d => d.TotalPrice))
                .ForMember(s => s.Items, o => o.MapFrom(d => d.Items))
                .ForMember(s => s.MetaData, o => o.MapFrom(d => new Metadata
                {
                    Id = d.Id.ToString(),
                    CreatedAt = d.CreatedAt.ToString("dd/MM/yyyy"),
                    UpdatedAt = d.UpdatedAt.ToString("dd/MM/yyyy")
                }));
            CreateMap<ShoppingCartItem, ShoppingCartItemResponse>()
                .ForMember(s => s.ProductId, o => o.MapFrom(d => d.ProductId));
        }
    }
}