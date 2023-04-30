using Groceteria.Catalogue.Api.Extensions;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace Groceteria.Catalogue.Api.Services.v2
{
    public class ProductCatalogueServiceBase
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogger _logger;

        public ProductCatalogueServiceBase(IHttpContextAccessor httpContext, ILogger logger)
        {
            _httpContext = httpContext;
            _logger = logger;
        }
        protected void SetupPaginationResponseHeader(RequestQuery query, int totalItems)
        {
            _logger.Here().MethodEnterd();
            var response = _httpContext.HttpContext.Response;
            var totalPages = (int)Math.Ceiling((double)totalItems / query.PageSize);
            response.AddPaginationResponseHeader(query.PageIndex, query.PageSize, totalItems, totalPages);
        }
    }
}
