using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.Shared.Extensions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Serilog;

namespace Groceteria.SalesOrder.Application.Factory.Mail
{
    public class MailFactoryBase
    {
        protected readonly EmailSettingsOption _settings;
        protected readonly ILogger _logger;

        public MailFactoryBase(IOptions<EmailSettingsOption> settings, 
            ILogger logger)
        {           
            _settings = settings.Value;
            _logger = logger;
        }

        protected async Task<SmtpClient> CreateMailClient()
        {
            var client = new SmtpClient();
            
            try
            {
                await client.ConnectAsync(_settings.Server, _settings.Port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
            }
            catch(Exception ex)
            {
                _logger.Here().Information("Failed to establish connection to SMTP server. {@stackTrace}", ex);
            }

            _logger.Here().Information("Mail client established {@clientDetails}", _settings);
            return client;
        }
    }
}
