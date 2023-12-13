using Groceteria.IdentityManager.Api.Configurations.ElasticSearch;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Constants;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using Nest;

namespace Groceteria.IdentityManager.Api.Services.PaginatedRequest
{
    public class PaginatedService<TDocument> : IPaginatedService<TDocument> where TDocument : class
    {
        private readonly ILogger _logger;
        private readonly ElasticSearchConfiguration _settings;
        private readonly ElasticClient _elasticClient;

        public PaginatedService(ILogger logger, IOptions<ElasticSearchConfiguration> settings)
        {
            _logger = logger;
            _settings = settings.Value;
            var elasticUri = new Uri(_settings.Uri, UriKind.Absolute);
            _elasticClient = new ElasticClient(elasticUri);
        }

        public async Task<Result<long>> GetCount(string correlationId, SearchIndex searchIndex)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().WithCorrelationId(correlationId).Information("Request - get requested data count in total from elastic search");

            string indexName = GetIndexPattern(searchIndex);
            if (string.IsNullOrEmpty(indexName))
            {
                _logger.Here().WithCorrelationId(correlationId).Error("No actual index found with index type {serachIndex}", searchIndex);
                return Result<long>.Failure(ErrorCodes.NotFound, "Search index not found");
            }

            var countResponse = await _elasticClient.CountAsync<TDocument>(s => s.Index(indexName));

            if (!countResponse.IsValid)
            {
                _logger.Here().WithCorrelationId(correlationId).Error("{documentType} search failed", typeof(TDocument).Name);
                return Result<long>.Failure(ErrorCodes.InternalServerError, ErrorMessages.InternalServerError);
            }

            _logger.Here().WithCorrelationId(correlationId).Information("total {count} items found of type {documentType}", typeof(TDocument).Name, countResponse.Count);
            _logger.Here().MethodExited();
            return Result<long>.Success(countResponse.Count);
        }

        public async Task<Result<Pagination<TDocument>>> GetPaginatedData(RequestQuery query, string correlationId, SearchIndex searchIndex)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().WithCorrelationId(correlationId).Information("Request - get pagincated data from elastic search");

            string indexName = GetIndexPattern(searchIndex);
            if (string.IsNullOrEmpty(indexName))
            {
                _logger.Here().WithCorrelationId(correlationId).Error("No actual index found with index type {serachIndex}", searchIndex);
                return Result<Pagination<TDocument>>.Failure(ErrorCodes.NotFound, "Search index not found");
            }

            var searchResponse = await _elasticClient.SearchAsync<TDocument>(s => s
            .Index(indexName)
            .Size(query.PageSize)
            .From((query.PageIndex - 1) * query.PageSize)
            .Sort(sort => sort.Field(query.SortField, query.SortOrder == "Asc" ? SortOrder.Ascending : SortOrder.Descending))
            .Query(q => q.MatchAll()));

            if (!searchResponse.IsValid)
            {
                _logger.Here().WithCorrelationId(correlationId).Error("{documentType} search failed", typeof(TDocument).Name);
                return Result<Pagination<TDocument>>.Failure(ErrorCodes.InternalServerError, ErrorMessages.InternalServerError);
            }

            var paginatedResult = new Pagination<TDocument>(query.PageIndex, query.PageSize, searchResponse.Documents.Count, searchResponse.Documents.ToList());

            _logger.Here().WithCorrelationId(correlationId)
                .ForContext("maxSearchScore", searchResponse.MaxScore)
                .Information("{documentType} document search successfull", typeof(TDocument).Name);
            _logger.Here().MethodExited();
            return Result<Pagination<TDocument>>.Success(paginatedResult);
        }


        private string GetIndexPattern(SearchIndex searchIndex)
        {
            return searchIndex switch
            {
                SearchIndex.ApiClient => _settings.IdetityClientIndex,
                SearchIndex.ApiScope => _settings.IdentityScopeIndex,
                SearchIndex.ApiResource => _settings.IdentityApiResourceIndex,
                SearchIndex.IdentityResource => _settings.IdentityResourceIndex,
                _ => string.Empty
            }; ;
        }
    }
}
