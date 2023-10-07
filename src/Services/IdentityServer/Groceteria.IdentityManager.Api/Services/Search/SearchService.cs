using AutoMapper;
using Elasticsearch.Net;
using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Microsoft.Extensions.Options;
using Nest;

namespace Groceteria.IdentityManager.Api.Services.Search
{
    public class SearchService<TDocument> : ISearchService<TDocument> where TDocument: class
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ElasticSearchConfiguration _settings;

        public SearchService(ILogger logger, IOptions<ElasticSearchConfiguration> settings, IMapper mapper)
        {
            _logger = logger;
            _settings = settings.Value;
            _mapper = mapper;
            var elasticUri = new Uri(_settings.Uri);
            var connectionSetting = new ConnectionSettings(elasticUri)
                .DefaultIndex(_settings.IdetityClientIndex);
            _elasticClient = new ElasticClient(connectionSetting);
        }

        public async Task<Result<bool>> SeedDataAsync(TDocument document, string id, string index)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - storing data to elastic serach {index}", index);


            if (! await IndexExist(index))
            {
                await CreateNewIndex(index);
            }

            var indexResponse = await _elasticClient.IndexAsync(document, idx => idx.Index(index)
                .Id(id)
                .Refresh(Refresh.WaitFor));

            if (!indexResponse.IsValid)
            {
                _logger.Here().Error("Data seeding to elastci serach index {@index} failed", index);
                return Result<bool>.Failure(ErrorCodes.InternalServerError, ErrorMessages.InternalServerError);
            }

            _logger.Here().Information("Data seeding completed successfully for document {id}", id);
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SearchReIndex(IEnumerable<TDocument> documents, string index)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - reindex search data for {index}", index);

            if (!await IndexExist(index))
            {
                await CreateNewIndex(index);
            }

            var flushResponse = await _elasticClient.Indices.FlushAsync(index);
            if (!flushResponse.IsValid)
            {
                _logger.Here().Error("Index flushing failed");
                return Result<bool>.Failure(ErrorCodes.InternalServerError, "Index flush operation failed");
            }

            var bulkResponse = await _elasticClient.BulkAsync(b => b.Index(index).IndexMany(documents));

            if (!bulkResponse.IsValid)
            {
                _logger.Here().Error("Re-indexing failed");
                return Result<bool>.Failure(ErrorCodes.InternalServerError, "Re-index operation failed");
            }

            _logger.Here().Information("Re-index for {index} successful", index);
            return Result<bool>.Success(true);
        }

        private async Task<bool> IndexExist(string index)
        {
            _logger.Here().Information("No index found with name {index}", index);
            var indexResponse = await _elasticClient.Indices.ExistsAsync(index);
            return indexResponse.Exists;
        }

        private async Task<bool> CreateNewIndex(string index)
        {
            _logger.Here().Information("Creating new index with name {index}", index);
            var createIndexResponse = await _elasticClient.Indices.CreateAsync(index, c => c
            .Map<TDocument>(m => m.AutoMap())
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(0)
                ));

            return createIndexResponse.IsValid;
        }
    }
}
