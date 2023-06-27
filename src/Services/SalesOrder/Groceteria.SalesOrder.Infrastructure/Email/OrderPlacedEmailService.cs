using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Application.Extensions;
using Groceteria.SalesOrder.Application.Factory.Mail;
using Groceteria.SalesOrder.Application.Models.Constants;
using Groceteria.SalesOrder.Application.Models.Enums;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using Microsoft.Extensions.Options;
using Serilog;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class OrderPlacedEmailService : MailFactoryBase, IEmailService
    {
        private readonly EmailTemplates _emailTemplates;
        public EmailServiceType Type => EmailServiceType.OrderPlaced;

        public OrderPlacedEmailService(
            INotificationRepository notificationRepository, 
            ILogger logger,
            IOptions<EmailTemplates> emailTemplates)
            :base(notificationRepository, logger) 
        {
            _emailTemplates = emailTemplates.Value;
        }

        public async Task SendEmailAsync(object arg)
        {
            _logger.Here().MethodEnterd();

            var order = (Order)arg;
            _logger.Here().WithOrderId(order.Id).Information("Preparing notification message");

            var notificationCreated = await AddOrOverwriteNotificationAsync(
                order.Id,
                order.BillingAddress.EmailAddress,
                EmailServiceConstants.OrderPlacedSubject,
                _emailTemplates.OrderPlaced,
                GetOrderPlacedEmailFields(order)
            );

            if (!notificationCreated)
            {
                _logger.Here().WithOrderId(order.Id).Error("{@ErrorCode} - Failed to create notification message", ErrorCode.OperationFailed);
                return;
            }

            _logger.Here().WithOrderId(order.Id).Information("Notification message added to db.");
            _logger.Here().MethodExited();
        }

        private List<EmailField> GetOrderPlacedEmailFields(Order order)
        {
            return new List<EmailField>
            {
                new EmailField("[username]", order.UserName),
                new EmailField("[address]", order.BillingAddress.AddressLine),
                new EmailField("[total]", order.TotalPrice.ToString())
            };
        }
    }
}
