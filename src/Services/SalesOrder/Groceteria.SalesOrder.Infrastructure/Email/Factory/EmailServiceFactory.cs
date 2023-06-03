using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Domain.Enums;

namespace Groceteria.SalesOrder.Infrastructure.Email.Factory
{
    public class EmailServiceFactory : IEmailServiceFactory
    {
        private readonly IEnumerable<IEmailService> _services;

        public EmailServiceFactory(IEnumerable<IEmailService> services)
        {
            _services = services;
        }

        public IEmailService GetService(EmailServiceType type)
        {
            IEmailService emailService = _services.FirstOrDefault(x => x.Type == type);
            if (emailService == null)
            {
                throw new ArgumentException($"Invalid service type: {type}");
            }
            return emailService;
        }
    }
}
