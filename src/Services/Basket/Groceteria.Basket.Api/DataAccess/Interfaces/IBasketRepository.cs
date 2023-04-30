using Groceteria.Basket.Api.Entities;
using System.Linq.Expressions;

namespace Groceteria.Basket.Api.DataAccess.Interfaces
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetCart(string username);
        Task<ShoppingCart> UpdateBasket(ShoppingCart cart);
        Task DeleteCart(string username);
    }
}
