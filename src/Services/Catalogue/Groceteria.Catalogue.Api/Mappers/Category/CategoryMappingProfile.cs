using AutoMapper;
using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Responses;

namespace Groceteria.Catalogue.Api.Mappers.Category
{
    public class CategoryMappingProfile: Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<Entities.Category, CategoryResponse>()
                    .ForMember(c => c.ImageLink, o => o.MapFrom(s => s.Image))
                    .ForMember(c => c.MetaData, o => o.MapFrom(s => new MetaData
                    {
                        Id = s.Id,
                        CreatedAt = s.CreatedAt.ToString("dd/MM/yyyy hh:mm:ss"),
                        UpdatedAt = s.UpdatedAt.ToString("dd/MM/yyyy hh:mm:ss")
                    }));
        }
    }
}
