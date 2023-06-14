using Groceteria.SalesOrder.Application.Models.Email;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groceteria.SalesOrder.Application.Contracts.Infrastructures
{
    public interface IEmailQueue
    {
        public ConcurrentQueue<object> Queue { get; set; }
    }
}
