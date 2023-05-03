using Groceteria.Discount.Grpc.Entities;

namespace Groceteria.Discount.Grpc.DataAccess.Repositories
{
    public interface IDiscountRepository
    {
        Task<IEnumerable<Coupon>> GetAllCoupons();
        Task<Coupon> GetCoupon(int id);
        Task<Coupon> GetCouponByProductName(string productName);
        Task<bool> CreateCoupon(Coupon discountCoupon);
        Task<bool> UpdateCoupon(Coupon discountCoupon);
        Task<bool> DeleteCoupon(int id);
    }
}
