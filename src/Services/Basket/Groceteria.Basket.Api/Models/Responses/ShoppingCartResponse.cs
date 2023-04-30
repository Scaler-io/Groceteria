using Groceteria.Basket.Api.Models.Core;
using Newtonsoft.Json;

namespace Groceteria.Basket.Api.Models.Responses
{
    public class ShoppingCartResponse
    {
        public string Username { get; set; }
        public IEnumerable<ShoppingCartItemResponse> Items { get; set; }
        public decimal BasketTotal { get; set; }

        [JsonProperty("metadata")]
        public Metadata MetaData { get; set; }

        public ShoppingCartResponse(string username)
        {
            Username = username;
        }
    }
}
