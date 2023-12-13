using Groceteria.Identity.Shared.Data.Specifications;

namespace Groceteria.IdentityManager.Api.Specifications.IdentityResource;

public class GetIdentityResourceById :
    BaseSpecification<IdentityServer4.EntityFramework.Entities.IdentityResource>
{
    public GetIdentityResourceById(int resourceId)
        : base(x => x.Id == resourceId)
    {
        AddIncludes("UserClaims");
    }
}
