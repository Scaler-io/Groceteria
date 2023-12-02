using Groceteria.Identity.Shared.Data.Specifications;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiScopes
{
    public class GetApiScopeWithId: BaseSpecification<ApiScope>
    {
        public GetApiScopeWithId(int id):base(x => x.Id == id) 
        {
            AddIncludes("UserClaims");
        }
    }
}
