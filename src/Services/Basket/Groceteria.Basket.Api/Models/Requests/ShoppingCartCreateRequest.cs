namespace Groceteria.Basket.Api.Models.Requests
{
    public class ShoppingCartCreateRequest
    {
        public string Username { get; set; }
        public IEnumerable<ShoppingCartItemRequest> Items { get; set; }
    }
}
