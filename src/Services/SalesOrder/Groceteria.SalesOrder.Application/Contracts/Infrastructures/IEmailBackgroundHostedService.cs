using Groceteria.SalesOrder.Domain.Enums;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailBackgroundHostedService
    {
        Task SendMailAsync(object arg, EmailServiceType type);
    }
}
