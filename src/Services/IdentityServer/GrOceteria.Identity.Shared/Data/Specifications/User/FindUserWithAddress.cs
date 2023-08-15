using Groceteria.Identity.Shared.Entities;

namespace Groceteria.Identity.Shared.Data.Specifications.User
{
    public class FindUserWithAddress: BaseSpecification<AppUser>
    {
        public FindUserWithAddress()
            
        {
            AddIncludes("Addresses");
        }
    }
}
