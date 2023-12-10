using Groceteria.Identity.Shared.Data.Specifications;
using Groceteria.Identity.Shared.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiResources;

public class GetAllApiResourcesWithPagination : BaseSpecification<ApiResourceExtended>
{
    public GetAllApiResourcesWithPagination(int pageIndex = 1, int pageSize = 50)
    {
        AddIncludes("Secrets");
        AddIncludes("Scopes");
        AddIncludes("UserClaims");
        ApplyPaging(pageSize * (pageIndex - 1), pageSize);
    }
}
