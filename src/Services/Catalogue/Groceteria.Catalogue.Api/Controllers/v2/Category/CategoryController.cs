using Amazon.Runtime.Internal;
using Groceteria.Catalogue.Api.Models.Requests;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Catalogue.Api.Services.v2.Categories;
using Groceteria.Catalogue.Api.Swagger;
using Groceteria.Catalogue.Api.Swagger.Examples;
using Groceteria.Catalogue.Api.Swagger.Examples.Error;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Groceteria.Catalogue.Api.Controllers.v2.Category
{
    [ApiVersion("2")]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(Serilog.ILogger logger, ICategoryService categoryService)
            : base(logger)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("categories")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "GetCategoriesCollection", Summary = "Fetches all stored category details")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CategoryResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<CategoryResponse>), (int)HttpStatusCode.OK)]
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
            var result = await _categoryService.GetAllCategories(query);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpGet]
        [Route("categories/{id}")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "GetCategory", Summary = "Fetches selected category details")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CategoryResponseExample))]
        [ProducesResponseType(typeof(CategoryResponse), (int)HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBrandById([FromRoute] string id)
        {
            Logger.Here().MethodEnterd();
            var result = await _categoryService.GetCatgoryById(id);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpPost]
        [Route("category/upsert")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "CreateNewCategory", Summary = "Creates new category or updates category details")]
        // 200
        [SwaggerRequestExample(typeof(CreateCategoryRequestExample), typeof(CreateCategoryRequestExample))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(bool))]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponseExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponseExample))]
        [ProducesResponseType(typeof(IReadOnlyList<ApiResponse>), (int)HttpStatusCode.NotFound)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrrorResponseExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateNewCategory([FromBody] CategoryUpsertRequest request)
        {
            Logger.Here().MethodEnterd();
            var result = await _categoryService.Createcategory(request);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpDelete]
        [Route("category/delete/{id}")]
        [SwaggerHeader("CorrelationId", "string", "", false)]
        [SwaggerOperation(OperationId = "Delete category", Summary = "Deletes a catgory")]
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
        public async Task<IActionResult> DeleteCategory(string id)
        {
            Logger.Here().MethodEnterd();
            await _categoryService.DeleteCategory(id);
            Logger.Here().MethodExited();
            return NoContent();
        }
    }
}
