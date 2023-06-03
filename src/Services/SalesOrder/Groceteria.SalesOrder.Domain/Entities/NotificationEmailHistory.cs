using Groceteria.SalesOrder.Domain.Common;
using Groceteria.SalesOrder.Domain.Enums;

namespace Groceteria.SalesOrder.Domain.Entities
{
    public class NotificationEmailHistory: EntityBase
    {
        public string Data { get; set; }
        public DateTime MailSentAt { get; set; }
        public EmailNotificationStatus Status { get; set; }
    }
}
