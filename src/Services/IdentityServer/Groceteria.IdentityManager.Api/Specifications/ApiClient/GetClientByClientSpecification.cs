using Groceteria.Identity.Shared.Data.Specifications;

namespace Groceteria.IdentityManager.Api.Specifications.ApiClient
{
    public class GetClientByClientSpecification : BaseSpecification<Identity.Shared.Entities.ApiClient>
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
