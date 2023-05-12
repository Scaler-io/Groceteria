using AutoMapper;
using Groceteria.Discount.Grpc.Entities;
using Groceteria.Discount.Grpc.Protos;

namespace Groceteria.Discount.Grpc.Mappers
{
    public class CouponMappingProfile: Profile
    {
        public CouponMappingProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
