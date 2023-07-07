namespace Groceteria.Basket.Api.Models.Requests.BasketCheckout
{
    public class PaymentDetailsRequest
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}