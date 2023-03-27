using AutoMapper;
using Groceteria.Catalogue.Api.Models.Requests;

namespace Groceteria.Catalogue.Api.Mappers.Product
{
    public class ProductRequestMappingProfile: Profile
    {
        public ProductRequestMappingProfile()
        {
            CreateMap<Entities.Product, ProductUpsertRequest>().ReverseMap();
        }
    }
}
