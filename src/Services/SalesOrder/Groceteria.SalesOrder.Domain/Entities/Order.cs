using Groceteria.SalesOrder.Domain.Common;

namespace Groceteria.SalesOrder.Domain.Entities
{
    public class Order: EntityBase
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderItem> OrderedItems { get; set; }
        public BilingAddress BillingAddress { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
        public Guid BillingId { get; set; }
        public Guid PaymentId { get; set; }
    }
}
