using AutoMapper;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Filters;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiClient;
using Groceteria.IdentityManager.Api.Services.ApiScope;
using Groceteria.IdentityManager.Api.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Groceteria.IdentityManager.Api.Controllers.v1.Search
{
    [ApiVersion("1")]
    [Authorize]
    public class SearchIndexController: BaseApiController
    {
        private readonly IClientManageService _clientManagerService;
        private readonly IMapper _mapper;
        private readonly ElasticSearchConfiguration _settings;
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceFactory _serviceFactory;

        public SearchIndexController(ILogger logger,
            IIdentityService identityService,
            IClientManageService clientManagerService,
            IMapper mapper,
            IOptions<ElasticSearchConfiguration> settings,
            IServiceFactory serviceFactory,
            IServiceProvider serviceProvider)
            : base(logger, identityService)
        {
            _mapper = mapper;
            _settings = settings.Value;
            _clientManagerService = clientManagerService;
            _serviceFactory = serviceFactory;
            _serviceProvider = serviceProvider;
        }

        [HttpPost("reindex")]
        [EnsureOwnership(Roles.SuperAdmin)]
        public async Task<IActionResult> ReIndex([FromQuery] string index)
        {
            Logger.Here().MethodEnterd();
            var response = await ApplyReIndexing(index);
            Logger.Here().MethodExited();
            return OkOrFailure(response);
        }

        private string GetIndexPattern(string index)
        {
            return index switch
            {
                "apiclient" => _settings.IdetityClientIndex,
                "apiscope" => _settings.IdentityScopeIndex,
                _ => string.Empty
            };
        }

        private async Task<Result<bool>> ApplyReIndexing(string index)
        {
            var indexPattern = GetIndexPattern(index);
            Result<bool> response = new Result<bool>();

            switch (index)
            {
                case "apiclient":
                    var apiClientService = (IClientManageService)_serviceFactory.GetService(IdentityManagerApis.ApiClient);
                    var apiClients = await apiClientService.GetApiClients(new RequestQuery(), RequestInformation);
                    var apiClientSummary = _mapper.Map<IEnumerable<ApiClientSummary>>(apiClients.Value.Data.AsEnumerable());
                    using(var scope = _serviceProvider.CreateScope())
                    {
                        var serviceProvider = scope.ServiceProvider;
                        var searchService = serviceProvider.GetRequiredService<ISearchService<ApiClientSummary>>();
                        response = await searchService.SearchReIndex(apiClientSummary, indexPattern);
                    }
                    break;
                case "apiscope":
                    var apiScopeService = (IApiScopeManagerService)_serviceFactory.GetService(IdentityManagerApis.ApiScope);
                    var apiScopes = await apiScopeService.GetApiScopes(new RequestQuery(), RequestInformation.CorrelationId);
                    var apiScopeSummary = _mapper.Map<IEnumerable<ApiScopeSummary>>(apiScopes.Value.Data.AsEnumerable());
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var serviceProvider = scope.ServiceProvider;
                        var searchService = serviceProvider.GetRequiredService<ISearchService<ApiScopeSummary>>();
                        response = await searchService.SearchReIndex(apiScopeSummary, indexPattern);
                    }
                    break;
                default:
                    break;
            }

            return response;
        }
    }
}
