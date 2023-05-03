using Groceteria.Discount.Grpc.Entities;

namespace Groceteria.Discount.Grpc.DataAccess.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        public Task<bool> CreateCoupon(Coupon discountCoupon)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCoupon(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Coupon>> GetAllCoupons()
        {
            throw new NotImplementedException();
        }

        public Task<Coupon> GetCoupon(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Coupon> GetCouponByProductName(string productName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateCoupon(Coupon discountCoupon)
        {
            throw new NotImplementedException();
        }
    }
}
