using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using MimeKit;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(MimeMessage email)
        {
            await Task.Yield();
        }
    }
}
