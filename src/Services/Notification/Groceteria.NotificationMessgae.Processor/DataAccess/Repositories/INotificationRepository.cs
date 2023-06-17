using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groceteria.NotificationMessgae.Processor.DataAccess.Repositories
{
    public interface INotificationRepository
    {
        Task<NotificationHistory> GetNotificationHistory();
    }
}
