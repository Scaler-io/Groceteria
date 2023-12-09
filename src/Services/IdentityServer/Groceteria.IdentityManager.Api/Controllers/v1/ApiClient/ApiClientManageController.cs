using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Filters;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiClient;
using Groceteria.IdentityManager.Api.Services.PaginatedRequest;
using Groceteria.IdentityManager.Api.Swagger;
using Groceteria.IdentityManager.Api.Swagger.Examples;
using Groceteria.IdentityManager.Api.Swagger.Examples.ErrorExamples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Groceteria.IdentityManager.Api.Controllers.v1.ApiClient
{
    [ApiVersion("1")]
    [Authorize]
    public class ApiClientManageController : BaseApiController
    {
        private readonly IClientManageService _clientService;
        private readonly IPaginatedService<ApiClientSummary> _paginatedService;

        public ApiClientManageController(ILogger logger,
            IIdentityService identityService,
            IClientManageService clientService,
            IPaginatedService<ApiClientSummary> paginatedService)
            : base(logger, identityService)
        {
            _clientService = clientService;
            _paginatedService = paginatedService;
        }

        [HttpGet("clients")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiClients", Description = "Fetches all api clients")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ListAllApiClientExample))]
        [ProducesResponseType(typeof(IReadOnlyList<IdentityServer4.EntityFramework.Entities.Client>), (int)HttpStatusCode.OK)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        [EnsureOwnership(Roles.SuperAdmin, Roles.SystemAdmin)]
        public async Task<IActionResult> GetApiClients([FromQuery] RequestQuery queryParams)
        {
            Logger.Here().MethodEnterd();
            queryParams.SortField = "created";
            var result = await _paginatedService.GetPaginatedData(queryParams, RequestInformation.CorrelationId, SearchIndex.ApiClient);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }


        [HttpGet("clients/{clientId}")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiClient", Description = "Fetches api client bt client id")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ApiClientResultExample))]
        [ProducesResponseType(typeof(ApiClientDto), (int)HttpStatusCode.OK)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetApiClient([FromRoute] string clientId)
        {
            Logger.Here().MethodEnterd();
            var result = await _clientService.GetApiClient(clientId);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }


        [HttpPost("client/create")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "CreateOrUpdateApiClient", Description = "Creates or updates api clients")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(bool))]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOrUpdateApiClient([FromBody] ApiClientDto client)
        {
            Logger.Here().MethodEnterd();
            var result = await _clientService.UpsertApiClient(client, RequestInformation);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpGet("clients/count")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiClientCount", Description = "Fetches total api client count")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(int))]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        // 404
        [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        // 400
        [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestErrorExample))]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        // 500
        [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorExample))]
        [ProducesResponseType(typeof(ApiExceptionResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetApiClientCount() 
        {
            Logger.Here().MethodEnterd();
            var response = await _paginatedService.GetCount(RequestInformation.CorrelationId, SearchIndex.ApiClient);
            Logger.Here().MethodExited();
            return OkOrFailure(response);
        }
    }
}
