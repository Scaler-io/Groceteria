using Groceteria.Discount.Grpc.Protos;
using Groceteria.Shared.Core;

namespace Groceteria.Basket.Api.Services.Interfaces.Grpc
{
    public interface IDiscountGrpcService
    {
        Task<Result<CouponModel>> GetDiscount(string productId, string productName);
    }
}
