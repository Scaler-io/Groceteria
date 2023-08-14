using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Groceteria.IdentityManager.Api.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class BaseApiController: ControllerBase
    {
        public ILogger Logger { get; set; }
        public RequestInformation RequestInformation { get; set; }

        public BaseApiController(ILogger logger)
        {
            Logger = logger;
            RequestInformation = new RequestInformation
            {
                CorrelationId = GetOrGenerateCorelationId(),
            };
        }

        public IActionResult OkOrFailure<T>(Result<T> result) where T : class
        {
            if (result == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            return result.ErrorCode switch
            {
                ErrorCodes.NotFound => NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage)),
                ErrorCodes.Unauthorized => Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage)),
                ErrorCodes.OperationFailed => BadRequest(new ApiResponse(ErrorCodes.OperationFailed, result.ErrorMessage)),
                _ => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage))
            };
        }


        public IActionResult CreatedWithRoute<T>(Result<T> result, string routeName, object param) where T : class
        {
            if (result.IsSuccess && result.Value != null) return CreatedAtRoute(
                    routeName,
                    param,
                    result.Value
                );

            return OkOrFailure(result);
        }

        protected string GetOrGenerateCorelationId() => Request?.GetRequestHeaderOrdefault("CorrelationId", $"GEN-{Guid.NewGuid().ToString()}");
    }
}
