using Groceteria.IdentityManager.Api.Models.Core;
using System.Security.Claims;

namespace Groceteria.IdentityManager.Api.Services
{
    public interface IIdentityService
    {
        UserDto PrepareUser();
    }
}
