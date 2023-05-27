using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Groceteria.SaleseOrder.Api.Extensions;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace Groceteria.SaleseOrder.Api.Controllers
{
    [Route("api/v{version:apiVersion}")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        public BaseApiController(ILogger logger)
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
                _ => BadRequest(new ApiResponse(ErrorCode.BadRequest, result.ErrorMessage))
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

        public ILogger Logger { get; set; }
        public string CorrelationId { get; set; }
        protected string GetOrGenerateCorelationId() => Request?.GetRequestHeaderOrDefault("CorrelationId", $"GEN-{Guid.NewGuid().ToString()}");
    
        protected IActionResult ProcessValidationResult(ValidationResult validationResult)
        {
            validationResult.AddToModelState(ModelState);
            var errors = ModelState.Where(err => err.Value?.Errors.Count > 0).ToList();
            var validationError = new ApiValidationResponse()
            {
                Errors = new List<FieldLevelError>()
            };

            validationError.Errors.AddRange(
             errors.Select(error => new FieldLevelError
             {
                 Code = "Invalid",
                 Field = error.Key,
                 Message = error.Value?.Errors?.First().ErrorMessage
             })
            );

            return new UnprocessableEntityObjectResult(validationError);
        }

        public static bool IsInvalidResult(ValidationResult validationResult)
        {
            return validationResult != null && !validationResult.IsValid;
        }
    }
}
