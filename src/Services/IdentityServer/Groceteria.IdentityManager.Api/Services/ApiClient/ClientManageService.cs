using AutoMapper;
using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services.Search;
using Groceteria.IdentityManager.Api.Specifications.ApiClient;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.Extensions.Options;

namespace Groceteria.IdentityManager.Api.Services.ApiClient
{
    public class ClientManageService : IClientManageService, IIdentityManagerService
    {
        private readonly IBaseRepository<Identity.Shared.Entities.ApiClient> _clientRepository;  
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly ISearchService<ApiClientSummary> _searchService;
        private readonly ElasticSearchConfiguration _settings;
        private readonly GroceteriaOauthDbContext _dbContext;

        public IdentityManagerApis Type { get; set; } = IdentityManagerApis.ApiClient;

        public ClientManageService(IUnitOfWork unitOfWork,
            ILogger logger,
            GroceteriaOauthDbContext context,
            IMapper mapper,
            ISearchService<ApiClientSummary> searchService,
            IOptions<ElasticSearchConfiguration> settings)
        {
            _unitOfWork = unitOfWork;
            _dbContext = context;
            _clientRepository = _unitOfWork.Repository<Identity.Shared.Entities.ApiClient>(context);
            _logger = logger;
            _mapper = mapper;
            _settings = settings.Value;
            _searchService = searchService;
        }

        public async Task<Result<Pagination<ApiClientDto>>> GetApiClients(RequestQuery queryParams, RequestInformation requestInformation)
        {
            _logger.Here().MethodEnterd();

            var spec = new GetAllClientsWithPagination(queryParams.PageIndex, queryParams.PageSize);
            var clients = await _clientRepository.ListAsync(spec);
            var count = await _clientRepository.CountASync(spec);

            if(clients == null || clients.Count == 0)
            {
                _logger.Here().WithCorrelationId(requestInformation.CorrelationId).Error("No clients were found");
                return Result<Pagination<ApiClientDto>>.Failure(ErrorCodes.NotFound);
            }

            var response = _mapper.Map<IReadOnlyList<ApiClientDto>>(clients);
            _logger.Here().Information("Api clients fetched {@count}", clients.Count);
            _logger.Here().MethodExited();

            return Result<Pagination<ApiClientDto>>.Success(new Pagination<ApiClientDto>(queryParams.PageIndex, queryParams.PageSize, count, response));
        }

        public async Task<Result<ApiClientDto>> GetApiClient(string clientId)
        {
            _logger.Here().MethodEnterd();

            var spec = new GetClientByClientSpecification(clientId);
            var client = await _clientRepository.GetEntityWithSpec(spec);

            if(client == null)
            {
                _logger.Here().Error("No api client was found with client id {clientid}", clientId);
                return Result<ApiClientDto>.Failure(ErrorCodes.NotFound);
            }

            var clientDto = _mapper.Map<ApiClientDto>(client);

            _logger.Here().Information("Api client fetch successfull");
            _logger.Here().MethodExited();
            return Result<ApiClientDto>.Success(clientDto);
        }

        public async Task<Result<bool>> UpsertApiClient(ApiClientDto clientEntity, RequestInformation requestInformation)
        {
            _logger.Here().MethodEnterd();           

            var spec = new GetClientByClientSpecification(clientEntity.ClientId);
            var existingClient = await _clientRepository.GetEntityWithSpec(spec);

            if(existingClient != null)
            {
                _logger.WithCorrelationId(requestInformation.CorrelationId)
                    .Information("request - update client {ClientName}", existingClient.ClientName);


                _mapper.Map(clientEntity, existingClient, typeof(ApiClientDto), typeof(Client));
                
                existingClient.Updated = DateTime.Now;
                existingClient.LastAccessed = DateTime.Now;

                _clientRepository.Update(existingClient);

                await _unitOfWork.Complete(_dbContext);

                await UpdateSearchIndex(existingClient);

                _logger.Here()
                    .WithCorrelationId(requestInformation.CorrelationId)
                    .MethodExited();
                return Result<bool>.Success(true);
            }

            var entity = _mapper.Map<Groceteria.Identity.Shared.Entities.ApiClient>(clientEntity);

            _clientRepository.Add(entity);
            await _unitOfWork.Complete(_dbContext);          

            _logger.WithCorrelationId(requestInformation.CorrelationId)
                .Information("request client insert completed");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        private async Task UpdateSearchIndex(Client clientEntity)
        {
            var clientSummary = _mapper.Map<ApiClientSummary>(clientEntity);
            var fieldValue = new Dictionary<string, string>();
            fieldValue.Add("clientId", clientEntity.ClientId);
            await _searchService.UpdateDocumentAsync(clientSummary, fieldValue, _settings.IdetityClientIndex);
        }
    }
}
