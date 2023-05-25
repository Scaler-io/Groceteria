using Destructurama.Attributed;

namespace Groceteria.SalesOrder.Application.Models.Requests
{
    public class PaymentDetailsRequest
    {
        [LogMasked]
        public string CardName { get; set; }
        [LogMasked]
        public string CardNumber { get; set; }
        [LogMasked]
        public string Expiration { get; set; }
        [LogMasked]
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}