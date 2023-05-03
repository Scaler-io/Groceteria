using AutoMapper;
using Groceteria.Catalogue.Api.Models.Requests;

namespace Groceteria.Catalogue.Api.Mappers.Category
{
    public class CategoryRequestMappingProfile: Profile
    {
        public CategoryRequestMappingProfile()
        {
            CreateMap<Entities.Category, CategoryUpsertRequest>().ReverseMap();
        }
    }
}
