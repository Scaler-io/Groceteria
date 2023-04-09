namespace Groceteria.Basket.Api.DependencyInjections
{
    public static class StackExchangeRedisServiceExtensions
    {
        public static IServiceCollection AddRedisCacheService(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.InstanceName = configuration.GetSection("RedisCache").GetValue<string>("InstanceName");
                options.Configuration = configuration.GetSection("RedisCache").GetValue<string>("ConnectionString");
            });
            return services;
        }
    }
}
