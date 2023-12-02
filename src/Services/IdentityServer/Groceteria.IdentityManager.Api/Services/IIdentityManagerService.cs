using Groceteria.IdentityManager.Api.Models.Enums;

namespace Groceteria.IdentityManager.Api.Services
{
    public interface IIdentityManagerService
    {
        public IdentityManagerApis Type { get; set; }
    }
}
