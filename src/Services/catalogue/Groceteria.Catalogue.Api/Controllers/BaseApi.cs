using Groceteria.Catalogue.Api.Extensions;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Groceteria.Catalogue.Api.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class BaseApi : ControllerBase
    {
        public ILogger Logger { get; set; }
        public string CorrelationId { get; set; }

        public BaseApi(ILogger logger)
        {
            Logger = logger;
            CorrelationId = GetOrGenerateCorelationId();
        }

        public IActionResult OkOrFailure<T>(Result<T> result)
        {
            if (result == null) return NotFound(new ApiResponse(ErrorCode.NotFound));
            if (result.IsSuccess && result.Value == null) return NotFound(new ApiResponse(ErrorCode.NotFound));
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            return result.ErrorCode switch
            {
                ErrorCode.NotFound => NotFound(new ApiResponse(ErrorCode.NotFound, result.ErrorMessage)),
                ErrorCode.UnprocessableEntity => BadRequest(new ApiValidationResponse(result.ErrorMessage)),
                ErrorCode.Unauthorized => Unauthorized(new ApiResponse(ErrorCode.Unauthorized, result.ErrorMessage)),
                ErrorCode.OperationFailed => BadRequest(new ApiResponse(ErrorCode.OperationFailed, result.ErrorMessage)),
                _ =>  BadRequest(new ApiResponse(ErrorCode.BadRequest, result.ErrorMessage))
            };
        }

        public IActionResult CreatedWithRoute<T>(Result<T> result, string routeName, object param)
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
