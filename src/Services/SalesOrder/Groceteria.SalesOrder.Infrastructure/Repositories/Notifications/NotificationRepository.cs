using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Infrastructure.Persistance;
using Groceteria.Shared.SharedEntities;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.SalesOrder.Infrastructure.Repositories.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationProccessorContext _context;

        public NotificationRepository(NotificationProccessorContext context)
        {
            _context = context;
        }
        public async Task<NotificationHistory> GetNotification(Guid id)
        {
            return await _context.NotificationHistories.FindAsync(id);
        }

        public async Task<bool> AddNotification(NotificationHistory notification)
        {
            _context.NotificationHistories.Add(notification);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> UpdateNotification(NotificationHistory notification)
        {
            _context.Entry(notification).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
