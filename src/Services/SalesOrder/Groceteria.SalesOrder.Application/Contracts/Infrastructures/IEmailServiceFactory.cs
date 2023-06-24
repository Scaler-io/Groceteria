using Groceteria.SalesOrder.Application.Models.Enums;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailServiceFactory
    {
        IEmailService GetService(EmailServiceType type);
    }
}
