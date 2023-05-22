namespace Groceteria.SalesOrder.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
    }
}