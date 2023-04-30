namespace Groceteria.Basket.Api.Models.Responses
{
    public class ProductSearchResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string SKU { get; set; }
        public string Image { get; set; }
    }
}
