using Groceteria.Discount.Grpc.Entities;
using Groceteria.Discount.Grpc.Protos;
using Groceteria.Shared.Core;

namespace Groceteria.Discount.Grpc.DataAccess.Repositories
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Coupon>> GetAllCoupons();
        Task<Coupon> GetCoupon(string productId, string productName);
        Task<Coupon> GetCouponByProductName(string productName);
        Task<bool> CreateCoupon(Coupon discountCoupon);
        Task<bool> UpdateCoupon(Coupon discountCoupon);
        Task<bool> DeleteCoupon(string productId, string productName);
    }
}
