using AutoMapper;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Dtos;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Mappers.ApiClient
{
    public class ApiClientSummaryMapper: Profile
    {
        public ApiClientSummaryMapper()
        {
            
            CreateMap<ApiClientDto, ApiClientSummary>()
                .ForMember(d => d.Created, o => o.MapFrom(s => DateTime.Parse(s.MetaData.CreatedOn)))
                .ForMember(d => d.Updated, o =>
                {
                    o.PreCondition(s => !string.IsNullOrEmpty(s.MetaData.UpdatedOn));   
                    o.MapFrom(s => DateTime.Parse(s.MetaData.UpdatedOn));
                })
                .ForMember(d => d.LastAccessed, o =>
                {
                    o.MapFrom(s => DateTime.Parse(s.MetaData.LastAccesseOn));
                    o.PreCondition(s => !string.IsNullOrEmpty(s.MetaData.LastAccesseOn));
                })
                .ReverseMap();

            CreateMap<Client, ApiClientSummary>()
                .ForMember(d => d.ClientDescription, o => o.MapFrom(s => s.Description));
        }
    }
}
