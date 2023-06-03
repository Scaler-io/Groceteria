using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.SalesOrder.Application.Models.Email;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Extensions;
using MimeKit;
using Serilog;
using System.Reflection;
using System.Text;

namespace Groceteria.SalesOrder.Application.Factory.Mail
{
    public class OrderPlacingMailFactory : MailFactoryBase
    {
        public OrderPlacingMailFactory(EmailSettingsOption settings, 
            ILogger logger) 
            : base(settings, logger)
        {
        }

        public MimeMessage GenerateOrderPlacedMessage(Order order)
        {
            return GenerateMailTemplate(order);
        }

        private MimeMessage GenerateMailTemplate(Order order)
        {
            var emailTemplateText = ReadEmailTemplateText();
            emailTemplateText = EmbeddEmailFields(emailTemplateText, order);

            var builder = new BodyBuilder();
            builder.HtmlBody = emailTemplateText;

            var email = new MimeMessage();
            email.To.Add(MailboxAddress.Parse(order.BillingAddress.EmailAddress));
            email.Subject = $"{order.UserName}, your order has been placed";
            email.Sender = MailboxAddress.Parse(_settings.CompanyAddress);
            email.Body = builder.ToMessageBody();
            return email;
        }

        private string ReadEmailTemplateText()
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                "Factory/Templates/OrderPlacedEmailTemplate.html");
            using var str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            return mailText;
        }

        private string EmbeddEmailFields(string mailText, Order order)
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
                _logger.Here().Information($"field - {field.Key}, value - {field.Value}");
                builder.Replace(field.Key, field.Value);
            }

            return builder.ToString();
        }
    }
}
