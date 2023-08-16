using Groceteria.IdentityManager.Api.Models.Core;
using System.Security.Claims;

namespace Groceteria.IdentityManager.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public IdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public const string RoleClaim = ClaimTypes.Role;
        public const string FirstNameClaim = "firstName";
        public const string LastNameClaim = "lastName";
        public const string EmailClaim = ClaimTypes.Email;
        public const string UsernameClaim = "username";

        public UserDto PrepareUser()
        {
            var claims = _contextAccessor.HttpContext.User.Claims;

            return new UserDto
            {
                FirstName = claims.Where(c => c.Type == FirstNameClaim).FirstOrDefault().Value,
                LastName = claims.Where(c => c.Type == LastNameClaim).FirstOrDefault().Value,
                Username = claims.Where(c => c.Type == UsernameClaim).FirstOrDefault().Value,
                Email = claims.Where(c => c.Type == EmailClaim).FirstOrDefault().Value,
                Roles = claims.Where(c => c.Type == RoleClaim).FirstOrDefault().Value,
            };
        }
    }
}
