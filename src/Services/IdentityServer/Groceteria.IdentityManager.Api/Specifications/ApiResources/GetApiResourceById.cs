using Groceteria.Identity.Shared.Data.Specifications;
using Groceteria.Identity.Shared.Entities;

namespace Groceteria.IdentityManager.Api.Specifications.ApiResources;

public class GetApiResourceById : BaseSpecification<ApiResourceExtended>
{
    public GetApiResourceById(string resourceId)
        : base(x => x.Id == int.Parse(resourceId))
    {
        AddIncludes("Secrets");
        AddIncludes("Scopes");
        AddIncludes("UserClaims");
    }
}
