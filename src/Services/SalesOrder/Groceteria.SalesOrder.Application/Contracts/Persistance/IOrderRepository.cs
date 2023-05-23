using Groceteria.SalesOrder.Domain.Entities;

namespace Groceteria.SalesOrder.Application.Contracts.Persistance
{
    public interface IOrderRepository: IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserName(string username);
    }
}
