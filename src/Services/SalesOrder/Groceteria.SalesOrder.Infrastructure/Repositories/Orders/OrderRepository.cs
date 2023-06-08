using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.SalesOrder.Infrastructure.Persistance;
using Groceteria.Shared.Core;
using System.Linq.Expressions;

namespace Groceteria.SalesOrder.Infrastructure.Repositories.Orders
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(SalesOrderContext context) 
            : base(context)
        {
        }

        public async Task<IReadOnlyList<Order>> GetOrdersByUserName(string username, RequestQuery query)
        {
            var includes = new List<Expression<Func<Order, object>>>()
            {
                o => o.BillingAddress,
                o => o.OrderedItems
            };
            var orderList = await GetAsync(query, o => o.UserName == username, null, includes);
            return orderList;
        }
    }
}
