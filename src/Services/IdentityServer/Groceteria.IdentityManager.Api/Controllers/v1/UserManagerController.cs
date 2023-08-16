using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Groceteria.IdentityManager.Api.Controllers.v1
{
    [ApiVersion("1")]
    [Authorize]
    public class UserManagerController : BaseApiController
    {
        public UserManagerController(ILogger logger, IIdentityService identityService) : 
            base(logger, identityService)
        {

        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(CurrentUser);
        }
    }
}
