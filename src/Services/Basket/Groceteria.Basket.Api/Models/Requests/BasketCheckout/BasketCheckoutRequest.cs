namespace Groceteria.Basket.Api.Models.Requests.BasketCheckout
{
    public class BasketCheckoutRequest
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderItemRequest> OrderedItems { get; set; }
        public BillingAddressRequest BillingAddress { get; set; }
        public PaymentDetailsRequest PaymentDetails { get; set; }
    }
}
