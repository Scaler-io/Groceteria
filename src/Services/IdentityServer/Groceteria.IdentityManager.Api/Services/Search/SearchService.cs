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
    public class SearchService<TDocument> : ISearchService<TDocument> where TDocument : class
    {
        private readonly ILogger _logger;
        private readonly ElasticSearchConfiguration _settings;

        public SearchService(ILogger logger, IOptions<ElasticSearchConfiguration> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<Result<bool>> SeedDataAsync(TDocument document, string id, string index)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - storing data to elastic serach {index}", index);

            var elasticClient = GetElasticClient(index);

            if (!await IndexExist(elasticClient, index))
            {
                await CreateNewIndex(elasticClient, index);
            }

            var indexResponse = await elasticClient.IndexAsync(document, idx => idx.Index(index)
                .Id(id)
                .Refresh(Refresh.WaitFor));

            if (!indexResponse.IsValid)
            {
                _logger.Here().Error("Data seeding to elastic serach index {@index} failed", index);
                return Result<bool>.Failure(ErrorCodes.OperationFailed, ErrorMessages.Operationfailed);
            }

            _logger.Here().Information("Data seeding completed successfully for document {id}", id);
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdateDocumentAsync(TDocument updatedDocument, Dictionary<string, string> fieldValue, string index)
        {
            _logger.Here().MethodEnterd();
            var elasticClient = GetElasticClient(index);

            var documentResponse = await elasticClient
                .SearchAsync<TDocument>(s => s
                .Index(index)
                .Query(q => q.Match(m => m
                    .Field(fieldValue.Keys.First())
                    .Query(fieldValue.Values.First())
                )));

            var docId = documentResponse.Hits.First().Id;
            var document = documentResponse.Documents.First();
            var documentUpdateResponse = await elasticClient.UpdateAsync<TDocument, object>(
                    new DocumentPath<TDocument>(docId),
                    u => u.Doc(updatedDocument)
                          .DocAsUpsert()
                );
            var docData = documentUpdateResponse.Result;

            if (!documentUpdateResponse.IsValid)
            {
                _logger.Here().Error("Elastic search docuemnt update failed");
                return Result<bool>.Failure(ErrorCodes.OperationFailed, "Elastic document update failed");
            }

            _logger.Here().Information("Elastic document update successfull");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> SearchReIndex(IEnumerable<TDocument> documents, string index)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - reindex search data for {index}", index);

            var elasticClient = GetElasticClient(index);

            if (await IndexExist(elasticClient, index))
            {
                var deleteResponse = await elasticClient.Indices.DeleteAsync(index);
                if (!deleteResponse.IsValid)
                {
                    _logger.Here().Error("Index deletion failed");
                    return Result<bool>.Failure(ErrorCodes.OperationFailed, "Index deletion failed");
                }
            }

            await CreateNewIndex(elasticClient, index);

            var bulkResponse = await elasticClient.BulkAsync(b => b.Index(index).IndexMany(documents));

            if (!bulkResponse.IsValid)
            {
                _logger.Here().Error("Re-indexing failed");
                return Result<bool>.Failure(ErrorCodes.OperationFailed, "Re-index operation failed");
            }

            _logger.Here().Information("Re-index for {index} successful", index);
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> RemoveDocumentFromIndex(Dictionary<string, object> query, string index)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - remove data from elastic search {index}", index);

            var elasticClient = GetElasticClient(index);
            if (!await IndexExist(elasticClient, index))
            {
                return Result<bool>.Failure(ErrorCodes.OperationFailed, $"Elastic operation failed, index {index} was not found");
            }

            var fieldName = query.Keys.First();
            var fieldValue = query.Values.First();

            var deleteResponse = await elasticClient.DeleteByQueryAsync<TDocument>(d => d
                    .Index(index)
                    .Query(q => q
                    .Match(m => m.Field(fieldName).Query(fieldValue.ToString()))));

            if (!deleteResponse.IsValid)
            {
                _logger.Here().Error("Elastic search delete operation failed");
                return Result<bool>.Failure(ErrorCodes.OperationFailed, "Elastic document delete operation failed");
            }

            _logger.Here().Information("Elastic document delete successful");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        private async Task<bool> IndexExist(ElasticClient elasticClient, string index)
        {
            _logger.Here().Information("No index found with name {index}", index);
            var indexResponse = await elasticClient.Indices.ExistsAsync(index);
            return indexResponse.Exists;
        }

        private async Task<bool> CreateNewIndex(ElasticClient elasticClient, string index)
        {
            _logger.Here().Information("Creating new index with name {index}", index);
            var createIndexResponse = await elasticClient.Indices.CreateAsync(index, c => c
            .Map<TDocument>(m => m.AutoMap())
            .Settings(s => s
                .NumberOfShards(1)
                .NumberOfReplicas(0)
                ));

            return createIndexResponse.IsValid;
        }

        private ElasticClient GetElasticClient(string index)
        {
            var elasticUri = new Uri(_settings.Uri);
            var connectionSetting = new ConnectionSettings(elasticUri)
                            .DefaultIndex(index);
            return new ElasticClient(connectionSetting);
        }
    }
}
