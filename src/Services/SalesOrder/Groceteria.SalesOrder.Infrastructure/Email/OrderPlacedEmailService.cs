﻿using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Application.Factory.Mail;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.SalesOrder.Domain.Enums;
using Groceteria.Shared.Extensions;
using MimeKit;
using Newtonsoft.Json;
using Serilog;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class OrderPlacedEmailService : OrderPlacingMailFactory, IEmailService
    {
        private readonly IBaseRepository<NotificationEmailHistory> _notificationRepository;
        public OrderPlacedEmailService(EmailSettingsOption settings,
            ILogger logger,
            IBaseRepository<NotificationEmailHistory> notificationRepository)
            : base(settings, logger)
        {
            _notificationRepository = notificationRepository;
        }

        public EmailServiceType Type => EmailServiceType.OrderPlaced;

        public async Task SendEmailAsync(object arg)
        {
            _logger.Here().MethodEnterd();

            Order order = (Order) arg;
            var mailClient = await CreateMailClient();
            var orderPlacedEmail = GenerateOrderPlacedMessage(order);

            _logger.Here().Debug("Preparing for sending mail");
            var notificationHistory = await AddOrOverwriteNotificationStatus(orderPlacedEmail, EmailNotificationStatus.Draft);

            try
            {
                await mailClient.SendAsync(orderPlacedEmail);
                _logger.Here().Information("Mail sent to {@recipient}", orderPlacedEmail.To);
            }
            catch (Exception ex)
            {
                _logger.Here().Error("Failed to send mail", ex);
            }
            finally
            {
                if(notificationHistory != null)
                {
                    await AddOrOverwriteNotificationStatus(notificationHistory, EmailNotificationStatus.Sent);
                }
                mailClient.Disconnect(true);
            }

            _logger.Here().MethodExited();
        }

        private async Task<NotificationEmailHistory> AddOrOverwriteNotificationStatus(MimeMessage email,
            EmailNotificationStatus status)
        {
            var notificationHistory = new NotificationEmailHistory
            {
                Data = JsonConvert.SerializeObject(new
                {
                    Sender = email.Sender.Address,
                    Recipient = email.To,
                    Body = email.HtmlBody
                }),
                Status = status
            };

            try
            {
                notificationHistory = await _notificationRepository.AddAsync(notificationHistory);
                await _notificationRepository.Completed();
                _logger.Here().Information("mail history table updated.");
            }
            catch (Exception ex)
            {
                _logger.Here().Error("mail history table updation failed. {@stackTrace}", ex.StackTrace);
            }

            return notificationHistory;
        }

        private async Task AddOrOverwriteNotificationStatus(NotificationEmailHistory notificationHistory,
            EmailNotificationStatus status)
        {
            try
            {
                notificationHistory.MailSentAt = DateTime.UtcNow;
                notificationHistory.Status = status;
                await _notificationRepository.UpdateAsync(notificationHistory);
                await _notificationRepository.Completed();
                _logger.Here().Information("mail history table updated.");
            }
            catch (Exception ex)
            {
                _logger.Here().Error("mail history table updation failed. {@stackTrace}", ex.StackTrace);
            }
        }
    }
}
