using Destructurama.Attributed;

namespace Groceteria.SalesOrder.Application.Models.Requests
{
    public class BillingDetailsRequest: ContactDetailsRequest
    {
        [LogMasked]
        public string FirstName { get; set; }
        [LogMasked]
        public string LastName { get; set; }
        [LogMasked]
        public string EmailAddress { get; set; }
        [LogMasked]
        public string AddressLine { get; set; }
        [LogMasked]
        public string City { get; set; }
        [LogMasked]
        public string State { get; set; }
        [LogMasked]
        public string ZipCode { get; set; }
    }
}