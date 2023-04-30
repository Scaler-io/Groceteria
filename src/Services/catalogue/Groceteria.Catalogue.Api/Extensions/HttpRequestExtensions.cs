using Groceteria.Shared.Core;
using Newtonsoft.Json;

namespace Groceteria.Catalogue.Api.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string  GetRequestHeaderOrdefault(this HttpRequest request, string key, string defaultValue = null)
        {
            var header = request?.Headers?.FirstOrDefault(h => h.Key.Equals(key)).Value.FirstOrDefault();
            return header ?? defaultValue;
        }

        public static void AddPaginationResponseHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Header", "Pagination");
        }
    }
}
