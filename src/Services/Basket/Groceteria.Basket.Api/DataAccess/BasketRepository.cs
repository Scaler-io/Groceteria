using Groceteria.Basket.Api.DataAccess.Interfaces;
using Groceteria.Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Groceteria.Basket.Api.DataAccess
{
    public class BasketRepository: IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task DeleteCart(string username)
        {
            await _redisCache.RemoveAsync(username);
        }

        public async Task<ShoppingCart> GetCart(string username)
        {
            var cart = await _redisCache.GetStringAsync(username);
            if (string.IsNullOrEmpty(cart))
                return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(cart);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
        {
            await _redisCache.SetStringAsync(cart.Username, JsonConvert.SerializeObject(cart));
            return await GetCart(cart.Username);
        }
    }
}
