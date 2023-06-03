﻿using Groceteria.SalesOrder.Domain.Enums;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailService
    {
        public EmailServiceType Type { get; }
        Task SendEmailAsync(object arg);
    }

}
