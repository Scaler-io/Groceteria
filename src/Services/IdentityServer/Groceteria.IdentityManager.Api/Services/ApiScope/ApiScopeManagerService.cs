using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;
using Groceteria.IdentityManager.Api.Models.Enums;
using AutoMapper;
using Groceteria.IdentityManager.Api.Services.Search;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using IdentityServer4.EntityFramework.DbContexts;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Specifications.ApiScopes;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace Groceteria.IdentityManager.Api.Services.ApiScope
{
    public class ApiScopeManagerService : IApiScopeManagerService, IIdentityManagerService
    {
        private readonly IBaseRepository<IdentityServer4.EntityFramework.Entities.ApiScope> _apiScopeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ISearchService<ApiScopeSummary> _searchService;
        private readonly ElasticSearchConfiguration _elasticSettings;
        private readonly ConfigurationDbContext _dbContext;

        public ApiScopeManagerService(IUnitOfWork unitOfWork, ILogger logger, 
            IMapper mapper, 
            ISearchService<ApiScopeSummary> searchService, 
            IOptions<ElasticSearchConfiguration> elasticSettings, 
            ConfigurationDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _apiScopeRepository = _unitOfWork.Repository<IdentityServer4.EntityFramework.Entities.ApiScope>(dbContext);
            _logger = logger;
            _mapper = mapper;
            _searchService = searchService;
            _elasticSettings = elasticSettings.Value;
            _dbContext = dbContext;
        }

        public IdentityManagerApis Type { get; set; } = IdentityManagerApis.ApiScope;

        public async Task<Result<Pagination<ApiScopeDto>>> GetApiScopes(RequestQuery query, string correlationId)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().WithCorrelationId(correlationId).Information("Request - fetch all api scopes");

            var spec = new GetAllScopesWithPagination(query.PageIndex, query.PageSize);
            var apiScopes = await _apiScopeRepository.ListAsync(spec);
            var count = await _apiScopeRepository.CountASync(spec);
            
            if(apiScopes == null || apiScopes.Count == 0)
            {
                _logger.Here().WithCorrelationId(correlationId).Warning("No api scopes were found");
                return Result<Pagination<ApiScopeDto>>.Failure(ErrorCodes.NotFound);
            }

            var apiScopeDto = _mapper.Map<IReadOnlyList<ApiScopeDto>>(apiScopes);

            _logger.Here().WithCorrelationId(correlationId).Information("Total {count} api scope was fetched", apiScopes.Count);
            _logger.Here().MethodExited();

            return Result<Pagination<ApiScopeDto>>.Success(new Pagination<ApiScopeDto>(query.PageIndex, query.PageSize, count, apiScopeDto));
        }
    
        public async Task<Result<ApiScopeDto>> GetApiScope(int id, string correlationId)
        {
            _logger.Here().MethodEnterd();
            _logger.Here()
                .WithCorrelationId(correlationId)
                .Information("Request - get api scope with id - {id}", id);

            var spec = new GetApiScopeWithId(id);
            var apiScope = await _apiScopeRepository.GetEntityWithSpec(spec);
            if(apiScope is null)
            {
                _logger.Here()
                    .WithCorrelationId(correlationId)
                    .Error("No api scope was found with id {id}", id);
                return Result<ApiScopeDto>.Failure(ErrorCodes.NotFound);
            }

            var apiScopeDto = _mapper.Map<ApiScopeDto>(apiScope);

            _logger.Here().Information("Api scope fetch successfull");
            _logger.Here().MethodExited();
            return Result<ApiScopeDto>.Success(apiScopeDto);
        }

        public async Task<Result<bool>> UpsertApiScope(ApiScopeDto scopeEntity, string correlationId)
        {
            _logger.Here().MethodEnterd();
            
            if(IsUpdateRequest(scopeEntity)) {
                var spec = new GetApiScopeWithId(scopeEntity.Id);
                var existingApiScope = await _apiScopeRepository.GetEntityWithSpec(spec);

                if (existingApiScope is not null)
                {
                    _logger.Here()
                    .WithCorrelationId(correlationId)
                    .Information("Request - update api scope {apiScopeName}", scopeEntity.Name);

                    _mapper.Map(scopeEntity, existingApiScope, typeof(ApiScopeDto), typeof(IdentityServer4.EntityFramework.Entities.ApiScope));
                    _apiScopeRepository.Update(existingApiScope);
                    await _unitOfWork.Complete(_dbContext);

                    await UpdateSearchIndex(existingApiScope);

                    _logger.Here().MethodExited();
                    return Result<bool>.Success(true);
                }
            }
            else
            {
                _logger.Here()
                    .WithCorrelationId(correlationId)
                    .Information("Request - adding new api scope {apiScopeName}", scopeEntity.Name);

                var entity = _mapper.Map<IdentityServer4.EntityFramework.Entities.ApiScope>(scopeEntity);
                _apiScopeRepository.Add(entity);
                await _unitOfWork.Complete(_dbContext);

                await UpdateSearchIndex(entity);

                _logger.Here()
                    .WithCorrelationId(correlationId)
                    .Information("api scope inserted successfully");

                _logger.Here().MethodExited();
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(ErrorCodes.OperationFailed, "Backend operation failed");
        }

        private async Task UpdateSearchIndex(IdentityServer4.EntityFramework.Entities.ApiScope scopeEntity)
        {
            var apiScopeSummary = _mapper.Map<ApiScopeSummary>(scopeEntity);
            var fieldValue = new Dictionary<string, string>();
            fieldValue.Add("id", scopeEntity.Id.ToString());
            await _searchService.UpdateDocumentAsync(apiScopeSummary, fieldValue, _elasticSettings.IdentityScopeIndex);
        }

        private bool IsUpdateRequest(ApiScopeDto scope)
        {
            return scope.Id != 0;
        }
    }
}
