using Groceteria.IdentityManager.Api.Models.Enums;

namespace Groceteria.IdentityManager.Api.Services
{
    public interface IServiceFactory
    {
        IIdentityManagerService GetService(IdentityManagerApis type);
    }
}
