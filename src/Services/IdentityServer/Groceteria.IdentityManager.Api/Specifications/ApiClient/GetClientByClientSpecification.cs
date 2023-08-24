using Groceteria.Identity.Shared.Data.Specifications;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiClient
{
    public class GetClientByClientSpecification : BaseSpecification<Client>
    {
        public GetClientByClientSpecification(string clientId)
            : base(x => x.ClientId == clientId)
        {
            AddIncludes("AllowedGrantTypes");
            AddIncludes("ClientSecrets");
            AddIncludes("RedirectUris");
            AddIncludes("PostLogoutRedirectUris");
            AddIncludes("AllowedScopes");
        }
    }
}
