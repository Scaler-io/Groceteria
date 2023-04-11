namespace Groceteria.Basket.Api.Models.Requests
{
    public class ShoppingCartRequest
    {
        public string Username { get; set; }
        public IEnumerable<ShoppingCartItem> Items { get; set; }
    }
}
