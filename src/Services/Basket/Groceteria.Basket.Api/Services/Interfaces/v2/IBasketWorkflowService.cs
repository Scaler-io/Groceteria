using Groceteria.Basket.Api.Models.Requests;
using Groceteria.Basket.Api.Models.Requests.BasketCheckout;
using Groceteria.Basket.Api.Models.Responses;
using Groceteria.Infrastructure.EventBus.Message.Models;
using Groceteria.Shared.Core;

namespace Groceteria.Basket.Api.Services.Interfaces.v2
{
    public interface IBasketWorkflowService
    {
        Task<Result<ShoppingCartResponse>> GetBasket(ShoppingCartFetchRequest request, RequestQuery queryParams);
        Task<Result<ShoppingCartResponse>> UpdateBasket(ShoppingCartCreateRequest request);
        Task DeleteBasket(string username);
        Task<Result<EventResponse>> CheckoutBasket(BasketCheckoutRequest request, CancellationToken cancellationToken);
    }
}
