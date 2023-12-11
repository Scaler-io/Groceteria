using AutoMapper;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Filters;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiClient;
using Groceteria.IdentityManager.Api.Services.ApiResource;
using Groceteria.IdentityManager.Api.Services.ApiScope;
using Groceteria.IdentityManager.Api.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Groceteria.IdentityManager.Api.Controllers.v1.Search
{
    [ApiVersion("1")]
    [Authorize]
    public class SearchIndexController : BaseApiController
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

        private async Task<Result<bool>> ApplyReIndexing(string index)
        {
            Result<bool> response = new Result<bool>();
            switch (index)
            {
                case "apiclient":
                    response = await ReIndexService<ApiClientSummary, IClientManageService>(IdentityManagerApis.ApiClient, index);
                    break;
                case "apiscope":
                    response = await ReIndexService<ApiScopeSummary, IApiScopeManagerService>(IdentityManagerApis.ApiScope, index);
                    break;
                case "apiresource":
                    response = await ReIndexService<ApiResourceSummary, IApiResourceManagerService>(IdentityManagerApis.ApiResource, index);
                    break;
                default:
                    break;
            }
            return response;
        }

        private string GetIndexPattern(string index)
        {
            return index switch
            {
                "apiclient" => _settings.IdetityClientIndex,
                "apiscope" => _settings.IdentityScopeIndex,
                "apiresource" => _settings.IdentityApiResourceIndex,
                _ => string.Empty
            };
        }

        private async Task<Result<bool>> ReIndexService<T, TService>(IdentityManagerApis serviceType, string index)
            where T : class
            where TService : IIdentityManagerService
        {
            var service = (TService)_serviceFactory.GetService(serviceType);
            var data = await GetData<T>(service);

            using (var scope = _serviceProvider.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var searchService = serviceProvider.GetRequiredService<ISearchService<T>>();
                var indexPattern = GetIndexPattern(index);
                return await searchService.SearchReIndex(data, indexPattern);
            }
        }

        private async Task<IEnumerable<T>> GetData<T>(dynamic service)
        {
            var requestQuery = new RequestQuery();
            var correlationId = RequestInformation?.CorrelationId;

            if (service is IClientManageService apiClientService)
            {
                var apiClients = await apiClientService.GetApiClients(requestQuery, RequestInformation);
                return _mapper.Map<IEnumerable<T>>(apiClients.Value.Data.AsEnumerable());
            }
            else if (service is IApiScopeManagerService apiScopeService)
            {
                var apiScopes = await apiScopeService.GetApiScopes(requestQuery, correlationId);
                return _mapper.Map<IEnumerable<T>>(apiScopes.Value.Data.AsEnumerable());
            }
            else if (service is IApiResourceManagerService apiResourceService)
            {
                var apiResources = await apiResourceService.GetallApiResources(requestQuery, correlationId);
                return _mapper.Map<IEnumerable<T>>(apiResources.Value.Data.AsEnumerable());
            }

            return Enumerable.Empty<T>();
        }
    }
}
