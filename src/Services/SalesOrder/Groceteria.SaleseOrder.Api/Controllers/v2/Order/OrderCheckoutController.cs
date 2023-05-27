using FluentValidation;
using FluentValidation.Results;
using Groceteria.SalesOrder.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Groceteria.Shared.Extensions;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Core;
using Swagger.Examples.CheckoutOrder;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Swagger.Configurations;
using Swagger.Examples.Errors;
using MediatR;
using Groceteria.SalesOrder.Application.Features.Orders.Commands.CheckoutOrder;

namespace Groceteria.SaleseOrder.Api.Controllers.v2.Order
{
    [ApiVersion("2")]
    public class OrderCheckoutController : BaseApiController
    {
        private readonly IValidator<CheckoutOrderRequest> _validator;
        private readonly IMediator _mediator;

        public OrderCheckoutController(Serilog.ILogger logger,
        IValidator<CheckoutOrderRequest> validator,
        IMediator mediator) :
            base(logger)
        {
            _validator = validator;
            _mediator = mediator;
        }

        [HttpPost("order/checkout")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "CheckoutOrder", Summary = "Cehckouts new order")]
        [SwaggerRequestExample(typeof(CheckoutOrderRequestExample), typeof(CheckoutOrderRequestExample))]
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
        public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrderCommand command, CancellationToken cancellationToken)
        {
            Logger.Here().MethodEnterd();
            var validationResult = IsValidRequest(command.CheckoutOrderRequest);
            if (IsInvalidResult(validationResult)) return ProcessValidationResult(validationResult);
            var result = await _mediator.Send(command, cancellationToken);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        private ValidationResult IsValidRequest(CheckoutOrderRequest request)
        {
            Logger.Here().Information("Request -  order checkout {@request}", request);
            var validationResult = _validator.Validate(request);
            if (!IsInvalidResult(validationResult)) return validationResult;
            Logger.Here().Warning("{@ErrorCode}-{@Request} Request validation failed", ErrorCode.UnprocessableEntity, request);
            return validationResult;
        }
    }
}
