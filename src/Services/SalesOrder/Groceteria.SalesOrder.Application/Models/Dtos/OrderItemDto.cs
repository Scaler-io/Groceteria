namespace Groceteria.SalesOrder.Application.Models.Dtos
{
    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
    }
}