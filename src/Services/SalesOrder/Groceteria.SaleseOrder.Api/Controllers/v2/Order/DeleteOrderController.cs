using FluentValidation;
using FluentValidation.Results;
using Groceteria.SalesOrder.Application.Features.Orders.Commands.DeleteOrder;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swagger.Configurations;
using Swagger.Examples.DeleteOrder;
using Swagger.Examples.Errors;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Groceteria.SaleseOrder.Api.Controllers.v2.Order
{
    [ApiVersion("2")]
    public class DeleteOrderController : BaseApiController
    {
        private readonly IValidator<DeleteOrderRequest> _validator;
        private readonly IMediator _mediator;

        public DeleteOrderController(Serilog.ILogger logger, 
            IValidator<DeleteOrderRequest> validator,
        IMediator mediator) :
            base(logger)
        {
            _validator = validator;
            _mediator = mediator;
        }

        [HttpDelete("order/delete")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "Delete order", Summary = "Deletes user order")]
        [SwaggerRequestExample(typeof(DeleteOrderRequestExample), typeof(DeleteOrderRequestExample))]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(bool))]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteOrder([FromBody] DeleteOrderCommand command)
        {
            Logger.Here().MethodEnterd();
            var validationResult = IsInvalidRequest(command.DeleteOrderRequest);
            if (validationResult != null) return ProcessValidationResult(validationResult);
            var result = await _mediator.Send(command);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        private ValidationResult IsInvalidRequest(DeleteOrderRequest request)
        {
            Logger.Here().Information("Request -  order delete {@request}", request);
            var validationResult = _validator.Validate(request);
            if (!IsInvalidResult(validationResult)) return validationResult;
            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCode.UnprocessableEntity, request);
            return validationResult;
        }
    }
}
