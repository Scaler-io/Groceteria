using MimeKit;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailService
    {
        Task SendEmailAsync(MimeMessage email);
    }
}
