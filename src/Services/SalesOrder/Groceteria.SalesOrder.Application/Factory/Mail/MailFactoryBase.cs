using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using Groceteria.Shared.SharedEntities;
using Newtonsoft.Json;
using Serilog;

namespace Groceteria.SalesOrder.Application.Factory.Mail
{
    public class MailFactoryBase
    {
        private readonly INotificationRepository _notificationRepository;
        protected readonly ILogger _logger;

        public MailFactoryBase(INotificationRepository notificationRepository, ILogger logger)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
        }

        public async Task<bool> AddOrOverwriteNotificationAsync(
            Guid notificationId,
            string recipent, 
            string subject, 
            string emailTemplate, 
            List<EmailField> emailFields)
        {
            _logger.Here().MethodEnterd();
            var notificationHistory = GenerateNotificationMesage(notificationId, recipent, subject, emailTemplate, emailFields);
            var createdResult = await _notificationRepository.AddNotification(notificationHistory);
            _logger.Here().MethodExited();
            return createdResult;
        }

        private NotificationHistory GenerateNotificationMesage(Guid notificationId,
            string recipent,
            string subject,
            string emailTemplate,
            List<EmailField> emailFields)
        {
            var emailData = JsonConvert.SerializeObject(emailFields);
            return new NotificationHistory
            {
                Id = notificationId,
                RecipientEmail = recipent,
                Subject = subject,
                Data = emailData,
                TemplateName = emailTemplate,
                IsPublished = false,
            };
        }
    }
}
