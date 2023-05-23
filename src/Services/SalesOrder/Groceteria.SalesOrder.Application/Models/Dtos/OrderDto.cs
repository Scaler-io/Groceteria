namespace Groceteria.SalesOrder.Application.Models.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public IReadOnlyList<OrderItemDto> Items { get; set; }
        public BillingAddressDto Address { get; set; }
        public MetadataDto MetaData { get; set; }
    }
}
