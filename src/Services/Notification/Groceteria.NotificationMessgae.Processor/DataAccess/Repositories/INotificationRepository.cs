using Groceteria.Shared.SharedEntities;

namespace Groceteria.NotificationMessgae.Processor.DataAccess.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<NotificationHistory>> GetNotificationHistory();
        Task UpdateNotificationHistory(NotificationHistory history);
    }
}
