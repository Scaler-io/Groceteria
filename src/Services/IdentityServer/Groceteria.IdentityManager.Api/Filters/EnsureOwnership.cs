using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Groceteria.IdentityManager.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EnsureOwnership : Attribute, IAuthorizationFilter
    {
        private readonly Roles[] _roles;

        public EnsureOwnership(params Roles[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (_roles.Contains(Roles.All))
            {
                return;
            }
            if (context != null)
            {
                var user = context.HttpContext.User;
                if (user.Identity.IsAuthenticated)
                {
                    var roleString = user.Identities.Select(identity => identity.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value).FirstOrDefault();
                    var roles = JsonConvert.DeserializeObject<List<string>>(roleString);
                    foreach (var role in roles)
                    {
                        foreach (var item in _roles)
                        {
                            if (role == item.ToString())
                            {
                                return;
                            }
                        }
                    }
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(ErrorCodes.Unauthorized)
                    {
                        ContentType = "application/json",
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Value = new ApiResponse(ErrorCodes.Unauthorized)
                    };
                }
            }
        }
    }
}
