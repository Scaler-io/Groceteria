using AutoMapper;
using Groceteria.Catalogue.Api.Models.Requests;

namespace Groceteria.Catalogue.Api.Mappers.Brand
{
    public class BrandRequestMappingProfile: Profile
    {
        public BrandRequestMappingProfile()
        {
            CreateMap<Entities.Brand, BrandUpsertRequest>().ReverseMap();
        }
    }
}
