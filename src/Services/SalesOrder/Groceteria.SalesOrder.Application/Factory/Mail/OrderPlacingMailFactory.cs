using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.SalesOrder.Application.Models.Email;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Extensions;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;
using System.Reflection;
using System.Text;

namespace Groceteria.SalesOrder.Application.Factory.Mail
{
    public class OrderPlacingMailFactory : MailFactoryBase
    {

        public MimeMessage GenerateOrderPlacedMessage(Order order, EmailSettingsOption settings, ILogger logger)
        {
            return GenerateMailTemplate(order, settings, logger);
        }

        private MimeMessage GenerateMailTemplate(Order order, EmailSettingsOption settings, ILogger logger)
        {
            var emailTemplateText = ReadEmailTemplateText();
            emailTemplateText = EmbeddEmailFields(emailTemplateText, order, logger);

            var builder = new BodyBuilder();
            builder.HtmlBody = emailTemplateText;

            var email = new MimeMessage();
            email.To.Add(MailboxAddress.Parse(order.BillingAddress.EmailAddress));
            email.Subject = $"{order.UserName}, your order has been placed";
            email.Sender = MailboxAddress.Parse(settings.CompanyAddress);
            email.Body = builder.ToMessageBody();
            return email;
        }

        private string ReadEmailTemplateText()
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "../../../../Groceteria.SalesOrder.Application/bin/Debug/net6.0/Factory/Template/OrderPlacedEmailTemplate.html");
            using var str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            return mailText;
        }

        private string EmbeddEmailFields(string mailText, Order order, ILogger logger)
        {
            var emailFields = new List<EmailField>
            {
                new EmailField("[username]", order.UserName),
                new EmailField("[total]", order.TotalPrice.ToString()),
                new EmailField("[addressLine]", order.BillingAddress.AddressLine)
            };

            var builder = new StringBuilder(mailText);
            foreach (var field in emailFields)
            {
                logger.Here().Information($"field - {field.Key}, value - {field.Value}");
                builder.Replace(field.Key, field.Value);
            }

            return builder.ToString();
        }
    }
}
