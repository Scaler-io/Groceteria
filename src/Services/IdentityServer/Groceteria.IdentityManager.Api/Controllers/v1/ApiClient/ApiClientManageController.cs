using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiClient;
using Groceteria.IdentityManager.Api.Swagger;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        [ProducesResponseType(typeof(IReadOnlyList<Client>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetApiClients([FromQuery] RequestQuery queryParams)
        {
            Logger.Here().MethodEnterd();
            var result = await _clientService.GetApiClients(queryParams, RequestInformation);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}
