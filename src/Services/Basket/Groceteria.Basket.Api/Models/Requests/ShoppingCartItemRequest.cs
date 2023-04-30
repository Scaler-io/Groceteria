namespace Groceteria.Basket.Api.Models.Requests
{
    public class ShoppingCartItemRequest
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
