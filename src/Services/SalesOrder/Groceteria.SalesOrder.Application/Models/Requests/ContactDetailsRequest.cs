using Destructurama.Attributed;

namespace Groceteria.SalesOrder.Application.Models.Requests
{
    public class ContactDetailsRequest
    {
        [LogMasked]
        public string Mobile { get; set; }
        [LogMasked]
        public string AlternateMobile { get; set; }
    }
}
