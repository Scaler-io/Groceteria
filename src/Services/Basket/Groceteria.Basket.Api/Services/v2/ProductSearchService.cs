using Groceteria.Basket.Api.Configurations;
using Groceteria.Basket.Api.Models.Responses;
using Groceteria.Basket.Api.Services.Interfaces.v2;
using Groceteria.Shared.Constants;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Mime;
using ILogger = Serilog.ILogger;

namespace Groceteria.Basket.Api.Services.v2
{
    public class ProductSearchService : IProductSearchService
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CatalogueApiSettings _catalogueApi;

        public ProductSearchService(ILogger logger
            , IHttpClientFactory httpClientFactory,
            IOptions<CatalogueApiSettings> catalogueApi)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _catalogueApi = catalogueApi.Value;
        }

        public async Task<Result<IEnumerable<ProductSearchResponse>>> ProductSearchAsync(string prodictIds)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("SearchProduct - {@ids}", prodictIds);

            using var client = _httpClientFactory.CreateClient(HttpClientNames.CatalogueApi);
            var route = $"{_catalogueApi.BaseAddress}/bulk/products?productIds={prodictIds}";
            using var request = new HttpRequestMessage(HttpMethod.Get, route);
            request.Headers.Accept.Add(System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));
            var httpResponse = await client.SendAsync(request);
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                _logger.Here().Error("{@ErrorCode} post code search failed", ErrorCode.InternalServerError);
                return Result<IEnumerable<ProductSearchResponse>>.Failure(ErrorCode.InternalServerError);
            }

            var productSearchResponse = await httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(productSearchResponse))
            {
                _logger.Here().Error("StatusCode:{0}", httpResponse.StatusCode.ToString());
                return Result<IEnumerable<ProductSearchResponse>>.Failure(ErrorCode.InternalServerError, productSearchResponse);
            }

            var response = JsonConvert.DeserializeObject<IEnumerable<ProductSearchResponse>>(productSearchResponse);
            _logger.Here().Information("prouct serach success {@response}", productSearchResponse);
            _logger.Here().MethodExited();

            return Result<IEnumerable<ProductSearchResponse>>.Success(response);
        }
    }
}
