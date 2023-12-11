using Groceteria.Identity.Shared.Data.Specifications;
using Groceteria.Identity.Shared.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiScopes
{
    public class GetAllScopesWithPagination : BaseSpecification<ApiScopeExtended>
    {
        public GetAllScopesWithPagination(int pageIndex = 1, int pageSize = 50)
        {
            AddIncludes("UserClaims");
            ApplyPaging(pageSize * (pageIndex - 1), pageSize);
        }
    }
}
