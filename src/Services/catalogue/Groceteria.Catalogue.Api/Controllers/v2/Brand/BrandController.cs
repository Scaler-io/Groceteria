﻿using Groceteria.Catalogue.Api.Models.Responses;
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
        public async Task<IActionResult> GetBrandList()
        {
            Logger.Here().MethodEnterd();
            var result = await _brandService.GetAllBrands();
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}
