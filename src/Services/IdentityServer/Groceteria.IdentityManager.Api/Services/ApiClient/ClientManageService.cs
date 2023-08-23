using AutoMapper;
using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Specifications.ApiClient;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Services.ApiClient
{
    public class ClientManageService : IClientManageService
    {
        private readonly IBaseRepository<Client> _clientRepository;  
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ClientManageService(IUnitOfWork unitOfWork, 
            ILogger logger, 
            ConfigurationDbContext context, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = _unitOfWork.Repository<Client>(context);
            _logger = logger;
            _mapper = mapper;
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
    }
}
