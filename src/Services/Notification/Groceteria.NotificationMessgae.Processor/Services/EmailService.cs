using Groceteria.NotificationMessgae.Processor.Configurations;
using Groceteria.NotificationMessgae.Processor.DataAccess.Repositories;
using Groceteria.NotificationMessgae.Processor.Factories;
using Groceteria.NotificationMessgae.Processor.Services.Interfaces;
using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using Groceteria.Shared.SharedEntities;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace Groceteria.NotificationMessgae.Processor.Services
{
    public class EmailService: MailFactoryBase, IEmailService
    {
        private readonly ILogger _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly EmailSettingsOption _emailSettings;

        public EmailService(ILogger logger, 
            INotificationRepository notificationRepository,
            IOptions<EmailSettingsOption> emailSettings)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendMailAsync()
        {
            var notificationsToProcess = await _notificationRepository.GetNotificationHistory();

            if(notificationsToProcess == null || notificationsToProcess.Count() == 0 ) 
            {
                return;
            }

            var mailClient = await CreateMailClient(_emailSettings, _logger);

            foreach (var notification in notificationsToProcess)
            {
                _logger.Here().Information("Messgae processing {@subject}", notification.Subject);
                var mail = ProcessMessage(notification);
                try
                {
                    await mailClient.SendAsync(mail);
                    notification.IsPublished = true;
                    notification.PublishTime = DateTime.UtcNow;
                    await _notificationRepository.UpdateNotificationHistory(notification);

                }
                catch (Exception e)
                {
                    _logger.Here().Error("{@message} - {@trace}", e.Message, e.StackTrace);
                }
            }
            await mailClient.DisconnectAsync(true);
        }

        private MimeMessage ProcessMessage(NotificationHistory notification)
        {
            var emailTemplateText = ReadEmailTemplateText($"{notification.TemplateName}.html");
            var emailFields = JsonConvert.DeserializeObject<List<EmailField>>(notification.Data);

            var builder = new BodyBuilder();
            
            var emailBuilder = new StringBuilder(emailTemplateText);
            foreach (var field in emailFields)
            {
                _logger.Here().Information($"field - {field.Key}, value - {field.Value}");
                emailBuilder.Replace(field.Key, field.Value);
            }

            var email = new MimeMessage();
            email.To.Add(MailboxAddress.Parse(notification.RecipientEmail));
            email.Subject = notification.Subject;
            email.Sender = MailboxAddress.Parse(_emailSettings.CompanyAddress);
            builder.HtmlBody = emailBuilder.ToString();
            email.Body = builder.ToMessageBody();
            return email;
        }
    

        private string ReadEmailTemplateText(string templateName)
        {
            return File.ReadAllText($"./Templates/{templateName}");
        }
    }
}
