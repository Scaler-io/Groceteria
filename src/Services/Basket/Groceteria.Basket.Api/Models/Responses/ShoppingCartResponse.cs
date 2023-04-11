using Groceteria.Basket.Api.Models.Core;

namespace Groceteria.Basket.Api.Models.Responses
{
    public class ShoppingCartResponse
    {
        public string Username { get; set; }
        public IReadOnlyList<ShoppingCartItemResponse> Items { get; set; }
        public Metadata MetaData { get; set; }
    }
}
