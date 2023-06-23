using Groceteria.Shared.SharedEntities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor.DataAccess.Repositories
{
    public class NotificationHistoryRepository : INotificationRepository
    {
        private readonly NotificationProcessorContext _context;
        private readonly ILogger _logger;

        public NotificationHistoryRepository(NotificationProcessorContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationHistory>> GetNotificationHistory()
        {
            var histories = await _context.NotificationHistories.Where(history => history.IsPublished == false).ToListAsync();
            return histories;
        }

        public async Task UpdateNotificationHistory(NotificationHistory history)
        {
            _context.Entry(history).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
