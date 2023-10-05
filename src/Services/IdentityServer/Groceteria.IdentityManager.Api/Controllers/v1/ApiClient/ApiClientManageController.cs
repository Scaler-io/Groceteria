using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Filters;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiClient;
using Groceteria.IdentityManager.Api.Swagger;
using Groceteria.IdentityManager.Api.Swagger.Examples;
using Groceteria.IdentityManager.Api.Swagger.Examples.ErrorExamples;
using IdentityServer4.EntityFramework.Entities;
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

        public ApiClientManageController(ILogger logger,
            IIdentityService identityService,
            IClientManageService clientService)
            : base(logger, identityService)
        {
            _clientService = clientService;
        }

        [HttpGet("clients")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiClients", Description = "Fetches all api clients")]
        // 200
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ListAllApiClientExample))]
        [ProducesResponseType(typeof(IReadOnlyList<Client>), (int)HttpStatusCode.OK)]
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
            var result = await _clientService.GetApiClients(queryParams, RequestInformation);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }


        [HttpPost("client/create")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "CreateOrUpdateApiClient", Description = "Creates or updates api clients")]

        public async Task<IActionResult> CreateOrUpdateApiClient([FromBody] ApiClientDto client)
        {
            Logger.Here().MethodEnterd();
            var result = await _clientService.UpsertApiClient(client, RequestInformation);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}
