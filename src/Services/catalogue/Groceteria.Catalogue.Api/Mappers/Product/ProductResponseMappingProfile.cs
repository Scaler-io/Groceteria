using AutoMapper;
using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Responses;

namespace Groceteria.Catalogue.Api.Mappers.Product
{
    public class ProductResponseMappingProfile: Profile
    {
        public ProductResponseMappingProfile()
        {
            CreateMap<Entities.Product, ProductResponse>()
                    .ForMember(p => p.Category, o => o.MapFrom(s => s.Category.Name))
                    .ForMember(p => p.Brand, o => o.MapFrom(s => s.Brand.Name))
                    .ForMember(p => p.MetaData, o => o.MapFrom(s => new MetaData
                    {
                        Id = s.Id,
                        CreatedAt = s.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss"),
                        UpdatedAt = s.UpdatedAt.ToString("dd/MM/yyyy hh:mm:ss")
                    }));
        }
    }
}
