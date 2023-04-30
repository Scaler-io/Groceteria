using Groceteria.Basket.Api.Services.Interfaces.v2;
using Microsoft.AspNetCore.Mvc;
using Groceteria.Shared.Extensions;
using Groceteria.Shared.Core;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using Groceteria.Basket.Api.Swagger;
using Groceteria.Basket.Api.Swagger.Examples.Errors;
using Groceteria.Basket.Api.Models.Requests;
using Groceteria.Basket.Api.Swagger.Examples;

namespace Groceteria.Basket.Api.Controllers.v2
{
    [ApiVersion("2")]
    public class BasketController : BaseApiController
    {
        private readonly IBasketWorkflowService _basketWorkflowService;

        public BasketController(Serilog.ILogger logger, IBasketWorkflowService basketWorkflowService) :
        base(logger)
        {
            _basketWorkflowService = basketWorkflowService;
        }

        [HttpGet("basket")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "GetBasket", Summary = "Fetches basket for ther specified user")]
        [SwaggerRequestExample(typeof(ShoppingCartFetchRequestExample), typeof(ShoppingCartFetchRequestExample))]
        //200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ShoppingCartResponseExample))]
        [ProducesResponseType(typeof(ShoppingCartResponseExample), (int)HttpStatusCode.OK)]
        //400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBasket([FromQuery]ShoppingCartFetchRequest request, [FromQuery] RequestQuery queryParams)
        {
            Logger.Here().MethodEnterd();
            var result = await _basketWorkflowService.GetBasket(request, queryParams);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpPost("basket/update")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "CreateBasket", Summary = "Creates or updates basket against username")]
        [SwaggerRequestExample(typeof(ShoppingCartCreateRequest), typeof(ShoppingCartCreateRequest))]
        //200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ShoppingCartResponseExample))]
        [ProducesResponseType(typeof(ShoppingCartResponseExample), (int) HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOrUpdateBasket([FromBody] ShoppingCartCreateRequest request)
        {
            Logger.Here().MethodEnterd();
            var result = await _basketWorkflowService.UpdateBasket(request);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpDelete("basket/delete/{username}")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "DeleteBasket", Summary = "Deletes basket against username")]
        //204
        [SwaggerResponseExample((int)HttpStatusCode.NoContent, typeof(NoContentResult))]
        [ProducesResponseType(typeof(ShoppingCartResponseExample), (int)HttpStatusCode.NoContent)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteBasket([FromRoute] string username)
        {
            Logger.Here().MethodEnterd();
            await _basketWorkflowService.DeleteBasket(username);
            Logger.Here().MethodExited();
            return NoContent();
        }
    }
}
