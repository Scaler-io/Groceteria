namespace Groceteria.SalesOrder.Domain.Entities
{
    public class BillingAddress: ContactDetails
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public List<Order> Orders { get; set; }
    }
}