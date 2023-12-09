using Groceteria.Identity.Shared.Data.Specifications;
using Groceteria.Identity.Shared.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiScopes
{
    public class GetApiScopeWithId: BaseSpecification<ApiScopeExtended>
    {
        public GetApiScopeWithId(int id):base(x => x.Id == id) 
        {
            AddIncludes("UserClaims");
        }
    }
}
