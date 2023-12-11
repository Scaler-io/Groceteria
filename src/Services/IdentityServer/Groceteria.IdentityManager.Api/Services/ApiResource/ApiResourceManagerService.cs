using AutoMapper;
using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services.Search;
using Groceteria.IdentityManager.Api.Specifications.ApiResources;
using Microsoft.Extensions.Options;

namespace Groceteria.IdentityManager.Api.Services.ApiResource;

public class ApiResourceManagerService : IApiResourceManagerService
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IBaseRepository<ApiResourceExtended> _apiResourceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly GroceteriaOauthDbContext _dbContext;
    private readonly ElasticSearchConfiguration _elasticSettings;
    private readonly ISearchService<ApiResourceSummary> _searchService;

    public IdentityManagerApis Type { get; set; } = IdentityManagerApis.ApiResource;

    public ApiResourceManagerService(ILogger logger,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        GroceteriaOauthDbContext dbContext,
        ISearchService<ApiResourceSummary> searchService,
        IOptions<ElasticSearchConfiguration> options)
    {
        _logger = logger;
        _mapper = mapper;
        _apiResourceRepository = unitOfWork.Repository<ApiResourceExtended>(dbContext);
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _searchService = searchService;
        _elasticSettings = options.Value;
    }

    public async Task<Result<Pagination<ApiResourceDto>>> GetallApiResources(RequestQuery query, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here()
        .WithCorrelationId(correlationId)
        .Information("Request - get all api resources");

        var spec = new GetAllApiResourcesWithPagination(query.PageIndex, query.PageSize);
        var entity = await _apiResourceRepository.ListAsync(spec);
        var count = await _apiResourceRepository.CountASync(spec);
        if (entity.Count < 1)
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("No api resources were found");
            return Result<Pagination<ApiResourceDto>>.Failure(ErrorCodes.NotFound, "No api resources were found");
        }

        var apiResourceList = _mapper.Map<IReadOnlyList<ApiResourceDto>>(entity);

        _logger.Here().WithCorrelationId(correlationId).Information("Total {count} api resources are fetched", entity.Count);
        _logger.Here().MethodExited();
        return Result<Pagination<ApiResourceDto>>.Success(new Pagination<ApiResourceDto>(query.PageIndex, query.PageSize, count, apiResourceList));
    }
    public async Task<Result<ApiResourceDto>> GetApiResource(string resourceId, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId).Information("Request - get api resource by id - {resourceId}", resourceId);

        var entity = await GetApiResourceById(resourceId);
        if (entity is null)
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("No api resource was found with id {resourceId}", resourceId);
            return Result<ApiResourceDto>.Failure(ErrorCodes.NotFound, "No api resource found");
        }

        var apiResource = _mapper.Map<ApiResourceDto>(entity);

        _logger.Here().WithCorrelationId(correlationId).Information("api resource fetch successful with {resourceId}", resourceId);
        _logger.Here().MethodExited();
        return Result<ApiResourceDto>.Success(apiResource);
    }
    public async Task<Result<bool>> UpsertApiResource(ApiResourceDto apiResource, string correlationId)
    {
        _logger.Here().MethodEnterd();

        if (IsUpdateRequest(apiResource.Id))
        {
            _logger.Here()
            .WithCorrelationId(correlationId)
            .Information("Request - Update api resource");

            var entity = await GetApiResourceById(apiResource.Id.ToString());
            entity.Updated = DateTime.Now;
            entity.LastAccessed = DateTime.Now;

            _mapper.Map(apiResource, entity, typeof(ApiResourceDto), typeof(ApiResourceExtended));
            _apiResourceRepository.Update(entity);
            await _unitOfWork.Complete(_dbContext);

            _logger.Here().WithCorrelationId(correlationId).Information("Api resource updated successfully");

            await UpdateSearchIndex(entity);

            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        _logger.Here()
            .WithCorrelationId(correlationId)
            .Information("Request - Create new api resource");

        var createEntity = _mapper.Map<ApiResourceExtended>(apiResource);
        _apiResourceRepository.Add(createEntity);
        await _unitOfWork.Complete(_dbContext);

        _logger.Here().WithCorrelationId(correlationId).Information("New api resource create successfull");

        await SeedOnDemand(createEntity);

        _logger.Here().MethodExited();
        return Result<bool>.Success(true);
    }
    public async Task<Result<bool>> DeleteApiResource(string resourceId, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId).Information("Request - Delete api resource with id {resourceId}", resourceId);

        var entity = await GetApiResourceById(resourceId);

        if (entity is null)
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("No api resource was found with id {resourceId}", resourceId);
            return Result<bool>.Failure(ErrorCodes.NotFound, "No api resource was found");
        }
        if (entity.IsDefault)
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("Delete operation aborted. Default api resource cannot be deleted");
            return Result<bool>.Failure(ErrorCodes.BadRequest, "Default api cannot be deleted");
        }

        var deleteResponse = await RemoveFromIndex(resourceId);
        if (!deleteResponse.IsSuccess)
        {
            _logger.Here().WithCorrelationId(correlationId).Error(deleteResponse.ErrorMessage);
            return Result<bool>.Failure((ErrorCodes)deleteResponse.ErrorCode, deleteResponse.ErrorMessage);
        }

        _apiResourceRepository.Delete(entity);
        await _unitOfWork.Complete(_dbContext);

        _logger.Here().WithCorrelationId(correlationId).Information("api resource deleted successfully");
        _logger.Here().MethodExited();

        return Result<bool>.Success(true);
    }
    private async Task<ApiResourceExtended> GetApiResourceById(string resourceId)
    {
        var spec = new GetApiResourceById(resourceId);
        return await _apiResourceRepository.GetEntityWithSpec(spec);
    }
    private async Task SeedOnDemand(ApiResourceExtended entity)
    {
        var apiResourceSummary = _mapper.Map<ApiResourceSummary>(entity);
        await _searchService.SeedDataAsync(apiResourceSummary, Guid.NewGuid().ToString(), _elasticSettings.IdentityApiResourceIndex);
    }
    private async Task UpdateSearchIndex(ApiResourceExtended apiResource)
    {
        var apiResourceSummary = _mapper.Map<ApiResourceSummary>(apiResource);
        var fieldValue = new Dictionary<string, string>
        {
            { "resourceId", apiResource.Id.ToString() }
        };
        await _searchService.UpdateDocumentAsync(apiResourceSummary, fieldValue, _elasticSettings.IdentityApiResourceIndex);
    }
    private async Task<Result<bool>> RemoveFromIndex(string resourceId)
    {
        var fieldValue = new Dictionary<string, object>
        {
            { "resourceId", resourceId }
        };
        return await _searchService.RemoveDocumentFromIndex(fieldValue, _elasticSettings.IdentityApiResourceIndex);
    }
    private bool IsUpdateRequest(int id)
    {
        return id != 0;
    }
}
