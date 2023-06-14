using Groceteria.SalesOrder.Application.Contracts.Infrastructures;
using Groceteria.SalesOrder.Application.Models.Email;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groceteria.SalesOrder.Infrastructure.Email
{
    public class EmailQueue : IEmailQueue
    {
        public EmailQueue()
        {
            Queue = new ConcurrentQueue<object>();
        }
        public ConcurrentQueue<object> Queue { get; set; }
    }
}
