namespace Groceteria.IdentityManager.Api.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetRequestHeaderOrdefault(this HttpRequest request, string key, string defaultValue = null)
        {
            var header = request?.Headers?.FirstOrDefault(h => h.Key.Equals(key)).Value.FirstOrDefault();
            return header ?? defaultValue;
        }
    }
}
