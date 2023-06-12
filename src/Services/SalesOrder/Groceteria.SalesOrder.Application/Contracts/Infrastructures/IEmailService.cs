using Groceteria.SalesOrder.Application.Configurations;
using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.SalesOrder.Domain.Enums;
using Serilog;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailService
    {
        EmailServiceType Type { get; }
        Task SendEmailAsync(object arg);
    }

}
