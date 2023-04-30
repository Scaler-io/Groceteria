namespace Groceteria.Basket.Api.Entities
{
    public class ShoppingCartItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string SKU { get; set; }
        public string Image { get; set; }
    }   
}