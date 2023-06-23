using Groceteria.NotificationMessgae.Processor.Configurations;
using Groceteria.Shared.Extensions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Serilog;

namespace Groceteria.NotificationMessgae.Processor.Factories
{
    public class MailFactoryBase
    {
        protected async Task<SmtpClient> CreateMailClient(EmailSettingsOption settings, ILogger logger)
        {
            var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(settings.Server, settings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(settings.Username, settings.Password);
            }
            catch (Exception ex)
            {
                logger.Here().Information("Failed to establish connection to SMTP server. {@stackTrace}", ex);
            }

            logger.Here().Information("Mail client established {@clientDetails}", settings);
            return client;
        }
    }
}
