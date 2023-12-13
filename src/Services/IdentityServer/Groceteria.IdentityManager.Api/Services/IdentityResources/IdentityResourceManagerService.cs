using AutoMapper;
using Elasticsearch.Net;
using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.IdentityResource;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services.Search;
using Groceteria.IdentityManager.Api.Specifications.IdentityResource;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.Extensions.Options;

namespace Groceteria.IdentityManager.Api.Services.IdentityResources;

public class IdentityResourceManagerService : IIdentityResourceManagerService
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IBaseRepository<IdentityResource> _idResourceRespository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConfigurationDbContext _dbContext;
    private readonly ElasticSearchConfiguration _elasticSettings;
    private readonly ISearchService<IdentityResourceSummary> _searchService;

    public IdentityManagerApis Type { get; set; } = IdentityManagerApis.IdentityResource;

    public IdentityResourceManagerService(IMapper mapper,
        ILogger logger,
        IUnitOfWork unitOfWork,
        ConfigurationDbContext dbContext,
        IOptions<ElasticSearchConfiguration> configuration,
        ISearchService<IdentityResourceSummary> searchService)
    {
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        _elasticSettings = configuration.Value;
        _idResourceRespository = _unitOfWork.Repository<IdentityResource>(_dbContext);
        _searchService = searchService;
    }

    public async Task<Result<IReadOnlyList<IdentityResourceDto>>> GetIdentityResources(string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId)
            .Information("Request - get all identity resources");

        var entities = await _idResourceRespository.ListAllAsync();
        var identityResources = _mapper.Map<IReadOnlyList<IdentityResourceDto>>(entities);

        _logger.Here().MethodExited();
        return Result<IReadOnlyList<IdentityResourceDto>>.Success(identityResources);
    }

    public async Task<Result<IdentityResourceDto>> GetIdentityResource(string resourecId, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId)
            .Information("Request - get identity resource by id");

        int.TryParse(resourecId, out int id); // Is a valid number?

        var entity = await GeIdentityResourceById(id);
        if (entity is null)
        {
            _logger.Here().WithCorrelationId(correlationId)
            .Warning("No identity resource was found by {resourceId}", resourecId);
            return Result<IdentityResourceDto>.Failure(ErrorCodes.NotFound, "No identity resource found");
        }

        var idResource = _mapper.Map<IdentityResourceDto>(entity);

        _logger.Here().WithCorrelationId(correlationId)
        .Information("Identity resource fetch successfull {@resource}", idResource);
        _logger.Here().MethodExited();

        return Result<IdentityResourceDto>.Success(idResource);
    }

    public async Task<Result<bool>> UpsertIdentityResource(IdentityResourceDto idResource, string correlationId)
    {
        _logger.Here().MethodEnterd();

        if (IsUpdateRequest(idResource.Id))
        {
            _logger.Here().WithCorrelationId(correlationId)
            .Information("Request - updates identity resource {@resource}", idResource);

            var entity = await GeIdentityResourceById(idResource.Id);
            if (entity is null)
            {
                _logger.Here().WithCorrelationId(correlationId)
                .Warning("Given id is incorrect. No resource was found with {id}", idResource.Id);
                return Result<bool>.Failure(ErrorCodes.OperationFailed, "Invalid update operation");
            }

            _mapper.Map(idResource, entity, typeof(IdentityResourceDto), typeof(IdentityResource));
            _idResourceRespository.Update(entity);
            await _unitOfWork.Complete(_dbContext);

            _logger.Here().WithCorrelationId(correlationId)
            .Information("Identity resource successfully updated in database");

            await UpdateSearchIndex(entity);

            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        _logger.Here().WithCorrelationId(correlationId)
            .Information("Request - creates new identity resource {@resource}", idResource);

        var createEntity = _mapper.Map<IdentityResource>(idResource);
        _idResourceRespository.Add(createEntity);
        await _unitOfWork.Complete(_dbContext);

        _logger.Here().WithCorrelationId(correlationId)
            .Information("Identity resource successfully created in database");

        await SeedOnDemand(createEntity);

        _logger.Here().MethodExited();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteIdentityResource(string resourceId, string correlationId)
    {
        _logger.Here().MethodEnterd();
        _logger.Here().WithCorrelationId(correlationId).Information("Request - Delete api resource with id {resourceId}", resourceId);

        int.TryParse(resourceId, out int id);

        var entity = await GeIdentityResourceById(id);

        if (entity is null)
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("No identity resource was found with id {resourceId}", resourceId);
            return Result<bool>.Failure(ErrorCodes.NotFound, "No identity resource was found");
        }
        if (entity.Required)
        {
            _logger.Here().WithCorrelationId(correlationId).Warning("Delete operation aborted. Resource is marked as required");
            return Result<bool>.Failure(ErrorCodes.BadRequest, "Identity resource cannot be deleted");
        }

        var deleteResponse = await RemoveFromIndex(resourceId);
        if (!deleteResponse.IsSuccess)
        {
            _logger.Here().WithCorrelationId(correlationId).Error(deleteResponse.ErrorMessage);
            return Result<bool>.Failure((ErrorCodes)deleteResponse.ErrorCode, deleteResponse.ErrorMessage);
        }

        _idResourceRespository.Delete(entity);
        await _unitOfWork.Complete(_dbContext);

        _logger.Here().WithCorrelationId(correlationId).Information("Identity resource deleted successfully");
        _logger.Here().MethodExited();

        return Result<bool>.Success(true);
    }

    private bool IsUpdateRequest(int id)
    {
        return id != 0;
    }

    private async Task<IdentityResource> GeIdentityResourceById(int id)
    {
        var spec = new GetIdentityResourceById(id);
        return await _idResourceRespository.GetEntityWithSpec(spec);
    }
    private async Task SeedOnDemand(IdentityResource entity)
    {
        var idResourceSummary = _mapper.Map<IdentityResourceSummary>(entity);
        await _searchService.SeedDataAsync(idResourceSummary, Guid.NewGuid().ToString(), _elasticSettings.IdentityResourceIndex);
    }
    private async Task UpdateSearchIndex(IdentityResource apiResource)
    {
        var idResourceSummary = _mapper.Map<IdentityResourceSummary>(apiResource);
        var fieldValue = new Dictionary<string, string>
        {
            { "resourceId", apiResource.Id.ToString() }
        };
        await _searchService.UpdateDocumentAsync(idResourceSummary, fieldValue, _elasticSettings.IdentityResourceIndex);
    }
    private async Task<Result<bool>> RemoveFromIndex(string resourceId)
    {
        var fieldValue = new Dictionary<string, object>
        {
            { "resourceId", resourceId }
        };
        return await _searchService.RemoveDocumentFromIndex(fieldValue, _elasticSettings.IdentityResourceIndex);
    }
}
