namespace Groceteria.Basket.Api.Entities
{
    public class ShoppingCart: BaseEntity
    {
        public string Username { get; set; }
        public IEnumerable<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {
            
        }

        public ShoppingCart(string username)
        {
            Username = username;
        }

        public decimal TotalPrice
        {
            get
            {
                var totalPrice = 0.0;
                foreach(var item in Items)
                {
                   totalPrice += item.Price * item.Quantity;
                }
                return (decimal)totalPrice;
            }
        }
    }
}
