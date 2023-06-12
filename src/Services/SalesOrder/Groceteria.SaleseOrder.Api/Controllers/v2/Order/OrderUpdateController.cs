using FluentValidation;
using FluentValidation.Results;
using Groceteria.SalesOrder.Application.Features.Orders.Commands.UpdateOrder;
using Groceteria.SalesOrder.Application.Models.Requests;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swagger.Configurations;
using Swagger.Examples.CheckoutOrder;
using Swagger.Examples.Errors;
using Swagger.Examples.UpdateOrder;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Groceteria.SaleseOrder.Api.Controllers.v2.Order
{
    [ApiVersion("2")]
    public class OrderUpdateController : BaseApiController
    {
        private readonly IValidator<UpdateOrderRequest> _validator;
        private readonly IMediator _mediator;

        public OrderUpdateController(Serilog.ILogger logger,
            IValidator<UpdateOrderRequest> validator,
            IMediator mediator) :
            base(logger)
        {
            _validator = validator;
            _mediator = mediator;
        }

        [HttpPut("order/update")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "Update order", Summary = "Update order")]
        [SwaggerRequestExample(typeof(UpdateOrderRequestExample), typeof(UpdateOrderRequestExample))]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CheckoutOrderResponseExample))]
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
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            Logger.Here().MethodEnterd();
            var validationResult = IsInvalidRequest(request);
            if (validationResult != null) return ProcessValidationResult(validationResult);
            var command = new UpdateOrderCommand { UpdateOrderRequest = request };
            var result = await _mediator.Send(command);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        private ValidationResult IsInvalidRequest(UpdateOrderRequest request)
        {
            Logger.Here().Information("Request -  order update {@request}", request);
            var validationResult = _validator.Validate(request);
            if (!IsInvalidResult(validationResult)) return validationResult;
            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCode.UnprocessableEntity, request);
            return validationResult;
        } 
    }
}
