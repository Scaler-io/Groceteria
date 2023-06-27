namespace Groceteria.SalesOrder.Application.Models.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public IReadOnlyList<OrderItemDto> OrderedItems { get; set; }
        public BillingAddressDto BillingAddress { get; set; }
        public MetadataDto MetaData { get; set; }
    }
}
