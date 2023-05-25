namespace Groceteria.SalesOrder.Application.Models.Requests
{
    public class UpdateOrderRequest
    {
        public string OrderId { get; set; }
        public string Username { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderItemRequest> OrderedItems { get; set; }
        public BillingDetailsRequest BillingAddress { get; set; }
        public PaymentDetailsRequest PaymentDetails { get; set; }
    }
}
