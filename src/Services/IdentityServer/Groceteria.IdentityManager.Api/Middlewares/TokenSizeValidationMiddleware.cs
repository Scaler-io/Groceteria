using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using System.Text;

namespace Groceteria.IdentityManager.Api.Middlewares
{
    public class TokenSizeValidationMiddleware
    {
        private const int MAX_TOKEN_SIZE = 2048; 
        private readonly RequestDelegate _next;
        private ILogger _logger;
        private readonly int _maxSize;

        public TokenSizeValidationMiddleware(RequestDelegate next, 
            ILogger logger, 
            int? maxSize = null)
        {
            _next = next;
            _logger = logger;
            _maxSize = maxSize ?? MAX_TOKEN_SIZE;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if(!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length);
                if(Encoding.UTF8.GetByteCount(token) > _maxSize)
                {
                    _logger.Here().Error("Token size is greater than max allowed size. Contact");
                    context.Response.StatusCode = 500; // Bad Request
                    await context.Response.WriteAsJsonAsync(new FieldLevelError
                    {
                        Code = ErrorCodes.BadRequest.ToString(),
                        Message = ErrorMessages.BadRequest
                    });

                    return;
                }
            }

            await _next(context);
        }
    }
}
