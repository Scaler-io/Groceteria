using Groceteria.Infrastructure.EventBus.Message.Models.BasketCheckout;

namespace Groceteria.Infrastructure.EventBus.Message.Events.BasketEvents
{
    public class BasketCheckoutEvent : BaseEvent
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderItem> OrderedItems { get; set; }
        public BillingAddress BillingAddress { get; set; }
        public PaymentDetails PaymentDetails { get; set; }
    }
}
