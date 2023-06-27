namespace Groceteria.SalesOrder.Domain.Entities
{
    public class PaymentDetails
    {
        public Guid Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
        public List<Order> Orders { get; set; }
    }
}