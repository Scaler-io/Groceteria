using Groceteria.IdentityManager.Api.Models.Core;
using IdentityModel;
using Newtonsoft.Json;
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
        public const string FirstNameClaim = ClaimTypes.GivenName;
        public const string LastNameClaim = ClaimTypes.Surname;
        public const string EmailClaim = ClaimTypes.Email;
        public const string UsernameClaim = JwtClaimTypes.PreferredUserName;

        public UserDto PrepareUser()
        {
            var claims = _contextAccessor.HttpContext.User.Claims;
            var roles = claims.Where(c => c.Type == RoleClaim).FirstOrDefault().Value;
            return new UserDto
            {
                FirstName = claims.Where(c => c.Type == FirstNameClaim).FirstOrDefault().Value,
                LastName = claims.Where(c => c.Type == LastNameClaim).FirstOrDefault().Value,
                Username = claims.Where(c => c.Type == UsernameClaim).FirstOrDefault().Value,
                Email = claims.Where(c => c.Type == EmailClaim).FirstOrDefault().Value,
                Roles = JsonConvert.DeserializeObject<List<string>>(claims.Where(c => c.Type == RoleClaim).FirstOrDefault().Value),
            };
        }
    }
}
