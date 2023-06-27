using Groceteria.SalesOrder.Application.Models.Enums;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailService
    {
        EmailServiceType Type { get; }
        Task SendEmailAsync(object arg);
    }

}
