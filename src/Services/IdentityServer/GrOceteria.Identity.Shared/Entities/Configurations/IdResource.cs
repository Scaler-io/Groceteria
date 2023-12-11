using IdentityServer4.EntityFramework.Entities;

namespace GrOceteria.Identity.Shared.Entities.Configurations;

public class IdResource : IdentityResource
{
    public bool IsDefault { get; set; }
}
