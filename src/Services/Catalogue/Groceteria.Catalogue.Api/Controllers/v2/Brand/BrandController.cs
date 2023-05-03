using Groceteria.Catalogue.Api.Models.Requests;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Catalogue.Api.Services.v2.Brands;
using Groceteria.Catalogue.Api.Swagger;
using Groceteria.Catalogue.Api.Swagger.Examples;
using Groceteria.Catalogue.Api.Swagger.Examples.Error;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Groceteria.Catalogue.Api.Controllers.v2.Brand
{
    [ApiVersion("2")]
    public class BrandController : BaseApiController
    {
        private readonly IBrandService _brandService;
        public BrandController(Serilog.ILogger logger, IBrandService brandService)
            : base(logger)
        {
            _brandService = brandService;
        }

        [HttpGet]
        [Route("brands")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "GetBrandsCollection", Summary = "Fetches all stored brand details")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(BrandResponseExample)) ]
        [ProducesResponseType(typeof(List<BrandResponse>), (int)HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBrandList([FromQuery] RequestQuery query)
        {
            Logger.Here().MethodEnterd();
            var result = await _brandService.GetAllBrands(query);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpGet]
        [Route("brand")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "GetBrandById", Summary = "Fetches selected brand details")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(BrandResponseExample))]
        [ProducesResponseType(typeof(List<BrandResponse>), (int)HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBrandById([FromQuery]string id)
        {
            Logger.Here().MethodEnterd();
            var result = await _brandService.GetBrandById(id);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpPost]
        [Route("brand/upsert")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "Create new brand", Summary = "Creates new brand")]
        // 200
        [SwaggerRequestExample(typeof(CreateBrandRequestExample), typeof(CreateBrandRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.Created, typeof(CreateBrandRequestExample))]
        [ProducesResponseType(typeof(List<BrandResponse>), (int)HttpStatusCode.Created)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOrUpdateBrand([FromBody] BrandUpsertRequest request)
        {
            Logger.Here().MethodEnterd();
            var result = await _brandService.CreateBrand(request);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpDelete]
        [Route("brand/delete")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "Delete brand", Summary = "Deletes a brand")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.NoContent, typeof(NoContentResponseExample))]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteBrand([FromQuery] string id)
        {
            Logger.Here().MethodEnterd();
            await _brandService.DeleteBrand(id);
            Logger.Here().MethodExited();
            return NoContent();
        }
    }
}
