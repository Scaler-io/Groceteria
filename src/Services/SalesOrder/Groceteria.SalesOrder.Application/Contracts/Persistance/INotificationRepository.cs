using Groceteria.Shared.SharedEntities;

namespace Groceteria.SalesOrder.Application.Contracts.Persistance
{
    public interface INotificationRepository
    {
        Task<NotificationHistory> GetNotification(Guid id); 
        Task<bool> AddNotification(NotificationHistory notification);
        Task<bool> UpdateNotification(NotificationHistory notification);
    }
}
