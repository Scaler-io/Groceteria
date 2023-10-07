using Groceteria.Identity.Shared.Data.Specifications;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiClient
{
    public class GetAllClientsWithPagination : BaseSpecification<Client> 
    {
        public GetAllClientsWithPagination(int pageIndex = 1, int pageSize = 50)
        {
            ApplyPaging(pageSize * (pageIndex - 1), pageSize);
            AddIncludes("AllowedGrantTypes");
            AddIncludes("ClientSecrets");
            AddIncludes("RedirectUris");
            AddIncludes("PostLogoutRedirectUris");
            AddIncludes("AllowedScopes");
        }
    }
}
