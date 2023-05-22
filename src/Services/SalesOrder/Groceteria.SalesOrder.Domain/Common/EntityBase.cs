namespace Groceteria.SalesOrder.Domain.Common
{
    public class EntityBase
    {
        public Guid Id { get; protected set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
