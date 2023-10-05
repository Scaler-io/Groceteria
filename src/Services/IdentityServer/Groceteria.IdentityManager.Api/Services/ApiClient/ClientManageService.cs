﻿using AutoMapper;
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
        private readonly ConfigurationDbContext _dbContext;

        public ClientManageService(IUnitOfWork unitOfWork, 
            ILogger logger, 
            ConfigurationDbContext context, 
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dbContext = context;
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

                _logger.Here()
                    .WithCorrelationId(requestInformation.CorrelationId)
                    .MethodExited();
                return Result<bool>.Success(true);
            }

            var entity = _mapper.Map<Client>(clientEntity);

            _clientRepository.Add(entity);
            await _unitOfWork.Complete(_dbContext);

            _logger.WithCorrelationId(requestInformation.CorrelationId)
                .Information("request client insert completed");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }
    }
}
