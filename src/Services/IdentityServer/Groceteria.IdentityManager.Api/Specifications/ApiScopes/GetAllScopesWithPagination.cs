using Groceteria.Identity.Shared.Data.Specifications;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiScopes
{
    public class GetAllScopesWithPagination: BaseSpecification<ApiScope>
    {
        public GetAllScopesWithPagination(int pageIndex = 1, int pageSize = 50)
        {
            AddIncludes("UserClaims");
            ApplyPaging(pageSize * (pageIndex - 1), pageSize);
        }
    }
}
