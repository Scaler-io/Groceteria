namespace Groceteria.SalesOrder.Application.Models.Requests
{
    public class CheckoutOrderRequest
    {
        public string Username { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderItemRequest> OrderedItems { get; set; }
        public BillingDetailsRequest BillingAddress { get; set; }
        public PaymentDetailsRequest PaymentDetails { get; set; }
    }
}
