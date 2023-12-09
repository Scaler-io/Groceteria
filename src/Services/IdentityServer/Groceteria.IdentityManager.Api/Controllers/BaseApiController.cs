using FluentValidation.Results;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Groceteria.IdentityManager.Api.Controllers
{
    [Route("api/v{version:apiVersion}/identity")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected readonly IIdentityService IdentityService;
        protected readonly ILogger Logger;

        public BaseApiController(ILogger logger, IIdentityService identityService)
        {
            Logger = logger;
            IdentityService = identityService;
        }

        protected UserDto CurrentUser => IdentityService.PrepareUser();

        protected RequestInformation RequestInformation => new RequestInformation
        {
            CorrelationId = GetOrGenerateCorelationId(),
            CurrentUser = CurrentUser
        };

        protected IActionResult OkOrFailure<T>(Result<T> result)
        {
            if (result == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value == null) return NotFound(new ApiResponse(ErrorCodes.NotFound));
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);

            return result.ErrorCode switch
            {
                ErrorCodes.BadRequest => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage)),
                ErrorCodes.InternalServerError => InternalServerError(new ApiResponse(ErrorCodes.InternalServerError, result.ErrorMessage)),
                ErrorCodes.NotFound => NotFound(new ApiResponse(ErrorCodes.NotFound, result.ErrorMessage)),
                ErrorCodes.Unauthorized => Unauthorized(new ApiResponse(ErrorCodes.Unauthorized, result.ErrorMessage)),
                ErrorCodes.OperationFailed => BadRequest(new ApiResponse(ErrorCodes.OperationFailed, result.ErrorMessage)),
                _ => BadRequest(new ApiResponse(ErrorCodes.BadRequest, result.ErrorMessage))
            };;
        }


        protected IActionResult Failure(ValidationResult validationResult)
        {
            var errors = validationResult.Errors;
            var apiValidationResponse = new ApiValidationResponse
            {
                Errors = new List<FieldLevelError>(),
                ErrorMessage = "Validation failed"
            };
            foreach(var error in errors)
            {
                var fieldLevelError = new FieldLevelError
                {
                    Code = error.ErrorCode,
                    Field = error.PropertyName,
                    Message = error.ErrorMessage
                };
                apiValidationResponse.Errors.Add(fieldLevelError);
            }
            return BadRequest(apiValidationResponse);
        }

        protected IActionResult CreatedWithRoute<T>(Result<T> result, string routeName, object param) where T : class
        {
            if (result.IsSuccess && result.Value != null) return CreatedAtRoute(
                    routeName,
                    param,
                    result.Value
                );

            return OkOrFailure(result);
        }

        protected string GetOrGenerateCorelationId() => Request?.GetRequestHeaderOrdefault("CorrelationId", $"GEN-{Guid.NewGuid().ToString()}");
    
        private ObjectResult InternalServerError(ApiResponse response)
        {
            return new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                ContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection
                {
                    "application/json"
                }
            };
        }
    }
}
