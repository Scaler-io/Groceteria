using AutoMapper;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Filters;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiClient;
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
        private readonly ISearchService<ApiClientSummary> _searchService;

        public SearchIndexController(ILogger logger,
            IIdentityService identityService,
            IClientManageService clientManagerService,
            IMapper mapper,
            IOptions<ElasticSearchConfiguration> settings
,
            ISearchService<ApiClientSummary> searchService)
            : base(logger, identityService)
        {
            _mapper = mapper;
            _settings = settings.Value;
            _clientManagerService = clientManagerService;
            _searchService = searchService;
        }

        [HttpPost("reindex/{index}")]
        [EnsureOwnership(Roles.SystemAdmin)]
        public async Task<IActionResult> ReIndex([FromRoute] string index)
        {
            Logger.Here().MethodEnterd();

            var indexPattern = GetIndexPattern(index);
            var apiClients = await _clientManagerService.GetApiClients(new RequestQuery(), RequestInformation);

            var apiClientSummary = _mapper.Map<IEnumerable<ApiClientSummary>>(apiClients.Value.Data.AsEnumerable());

            var response = await _searchService.SearchReIndex(apiClientSummary, indexPattern);

            Logger.Here().MethodExited();
            return OkOrFailure(response);
        }

        private string GetIndexPattern(string index)
        {
            return index switch
            {
                "apiclient" => _settings.IdetityClientIndex,
                _ => string.Empty
            };
        }
    }
}
