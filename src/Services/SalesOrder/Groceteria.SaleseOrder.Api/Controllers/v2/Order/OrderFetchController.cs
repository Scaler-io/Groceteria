using Groceteria.SalesOrder.Application.Features.Orders.Queries.GetOrderList;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swagger.Configurations;
using Swagger.Examples.CheckoutOrder;
using Swagger.Examples.Errors;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Groceteria.SaleseOrder.Api.Controllers.v2.Order
{
    [ApiVersion("2")]
    public class OrderFetchController : BaseApiController
    {
        private readonly IMediator _mediator;

        public OrderFetchController(Serilog.ILogger logger,
            IMediator mediator)
        : base(logger)
        {
            _mediator = mediator;
        }

        [HttpGet("order/{username}")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "GetOrdersForUser", Summary = "Fetches all the order history for a valid username")]
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
        public async Task<IActionResult> GetOrders([FromRoute] string username, [FromQuery] RequestQuery requestQuery)
        {
            Logger.Here().MethodEnterd();
            var query = new GetOrderListQuery(username, requestQuery);
            var order = await _mediator.Send(query);
            Logger.Here().MethodExited();
            return OkOrFailure(order);
        }
    }
}
