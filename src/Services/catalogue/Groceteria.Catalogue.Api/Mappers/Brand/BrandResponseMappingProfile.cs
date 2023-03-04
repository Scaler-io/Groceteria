using AutoMapper;
using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Responses;

namespace Groceteria.Catalogue.Api.Mappers.Brand
{
    public class BrandResponseMappingProfile: Profile
    {
        public BrandResponseMappingProfile()
        {
            CreateMap<Entities.Brand, BrandResponse>()
                .ForMember(b => b.ImageLink, o => o.MapFrom(s => s.Image))
                .ForMember(b => b.MetaData, o => o.MapFrom(s => new MetaData
                {
                    Id = s.Id,
                    CreatedAt = s.CreatedAt.ToString("dd/MM/yyy hh:mm:ss"),
                    UpdatedAt = s.UpdatedAt.ToString("dd/MM/yyy hh:mm:ss")
                }));
        }
    }
}
