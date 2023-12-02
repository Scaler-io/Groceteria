using Groceteria.IdentityManager.Api.Models.Enums;

namespace Groceteria.IdentityManager.Api.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IEnumerable<IIdentityManagerService> _services;

        public ServiceFactory(IEnumerable<IIdentityManagerService> services)
        {
            _services = services;
        }

        public IIdentityManagerService GetService(IdentityManagerApis type)
        {
            IIdentityManagerService identityManagerService = _services.FirstOrDefault(x => x.Type == type);
            if (identityManagerService == null)
            {
                throw new ArgumentException($"Invalid service type: {type}");
            }
            return identityManagerService;
        }
    }
}
