using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Catalogue.Api.Services.v2.Products;
using Groceteria.Catalogue.Api.Swagger;
using Groceteria.Catalogue.Api.Swagger.Examples;
using Groceteria.Catalogue.Api.Swagger.Examples.Error;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Groceteria.Catalogue.Api.Controllers.v2.Product
{
    [ApiVersion("2")]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;
        public ProductController(Serilog.ILogger logger, IProductService productService)
            : base(logger)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("products")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        // 200
        [SwaggerOperation(OperationId = "GetProductsCollection", Summary = "Fetches all stored product details")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProductResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), (int)HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBrandList()
        {
            Logger.Here().MethodEnterd();
            var result = await _productService.GetAllProducts();
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}
