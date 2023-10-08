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
        private readonly ElasticSearchConfiguration _settings;

        public SearchService(ILogger logger, IOptions<ElasticSearchConfiguration> settings)
        {
            _logger = logger;
            _settings = settings.Value;
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
                _logger.Here().Error("Data seeding to elastic serach index {@index} failed", index);
                return Result<bool>.Failure(ErrorCodes.InternalServerError, ErrorMessages.InternalServerError);
            }

            _logger.Here().Information("Data seeding completed successfully for document {id}", id);
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateDocumentAsync(TDocument updatedDocument, Dictionary<string, string> fieldValue, string index)
        {
            _logger.Here().MethodEnterd();

            var documentResponse = await _elasticClient
                .SearchAsync<TDocument>(s => s
                .Index(index)
                .Query(q => q.Match(m => m
                    .Field(fieldValue.Keys.First())
                    .Query(fieldValue.Values.First())
                )));

            var docId = documentResponse.Hits.First().Id;
            var document = documentResponse.Documents.First(); 
            var documentUpdateResponse = await _elasticClient.UpdateAsync<TDocument, object>(
                    new DocumentPath<TDocument>(docId) ,
                    u => u.Doc(updatedDocument)
                          .DocAsUpsert()
                );

            if (!documentUpdateResponse.IsValid)
            {
                _logger.Here().Error("Elastic search docuemnt update failed");
                return Result<bool>.Failure(ErrorCodes.InternalServerError, "Elastic document update failed");
            }

            _logger.Here().Information("Elastic document update successfull");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SearchReIndex(IEnumerable<TDocument> documents, string index)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - reindex search data for {index}", index);

            if (await IndexExist(index))
            {
                var deleteResponse = await _elasticClient.Indices.DeleteAsync(index);
                if (!deleteResponse.IsValid)
                {
                    _logger.Here().Error("Index deletion failed");
                    return Result<bool>.Failure(ErrorCodes.InternalServerError, "Index deletion failed");
                }
            }

            await CreateNewIndex(index);

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
