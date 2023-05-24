using Groceteria.SalesOrder.Domain.Entities;
using Groceteria.Shared.Core;

namespace Groceteria.SalesOrder.Application.Contracts.Persistance
{
    public interface IOrderRepository: IBaseRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetOrdersByUserName(string username, RequestQuery query);
    }
}
