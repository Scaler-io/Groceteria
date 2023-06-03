using Groceteria.SalesOrder.Domain.Enums;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailServiceFactory
    {
        IEmailService GetService(EmailServiceType type);
    }
}
