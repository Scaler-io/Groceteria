namespace Groceteria.ApiGateway.Extensions.Environment
{
    public static class WebHostEnvironmentExtensions
    {
        public static bool IsLocal(this IWebHostEnvironment env)
        {
            return env.IsEnvironment("Local");
        }
    }
}
