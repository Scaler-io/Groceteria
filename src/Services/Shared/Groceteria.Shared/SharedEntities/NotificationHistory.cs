using Groceteria.Shared.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace Groceteria.Shared.SharedEntities
{
    [Table("NotificationHistory")]
    public class NotificationHistory
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string RecipientEmail { get; set; }
        public string Data { get; set; }
        public string TemplateName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishTime { get; set; }
    }
}
