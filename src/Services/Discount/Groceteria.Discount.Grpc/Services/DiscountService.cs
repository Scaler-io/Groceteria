using AutoMapper;
using Groceteria.Discount.Grpc.DataAccess.Repositories;
using Groceteria.Discount.Grpc.Entities;
using Groceteria.Discount.Grpc.Protos;
using Groceteria.Shared.Constants;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using Grpc.Core;
using ILogger = Serilog.ILogger;

namespace Groceteria.Discount.Grpc.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(ILogger logger, IMapper mapper, 
            IDiscountRepository discountRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _discountRepository = discountRepository;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            _logger.Here().MethodEnterd();
            var coupon = await _discountRepository.GetCoupon(request.ProductId, request.ProductName);          
            if(coupon == null)
            {
                _logger.Here().Error("No coupon was found for product {@productName}", request.ProductName);
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with productId {request.ProductId} was not found."));
            }
            var result = _mapper.Map<CouponModel>(coupon);
            _logger.Here().Information("Coupon found for product {@productName}", result.ProductName);
            _logger.Here().MethodExited();
            return result;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            _logger.Here().MethodEnterd();
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var isCouponCreated = await _discountRepository.CreateCoupon(coupon);
            if (!isCouponCreated)
            {
                _logger.Here().Error("{ErrorCode} The discount coupon creation failed.", ErrorCode.OperationFailed);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessages.Operationfailed));
            }

            _logger.Here().Information("Coupon is created, {@coupon}", coupon);
            _logger.Here().MethodExited();
            return request.Coupon;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            _logger.Here().MethodEnterd();
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            var isCouponUpdated = await _discountRepository.UpdateCoupon(coupon);
            if (!isCouponUpdated)
            {
                _logger.Here().Error("{ErrorCode} The discount coupon update failed.", ErrorCode.OperationFailed);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessages.Operationfailed));
            }

            _logger.Here().Information("Coupon is updated, {@coupon}", coupon);
            _logger.Here().MethodExited();
            return request.Coupon;
        }

        public override async Task<DeletedResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            _logger.Here().MethodEnterd();

            var isCouponDeleted = await _discountRepository.DeleteCoupon(request.ProductId, request.ProductName);
            if (!isCouponDeleted)
            {
                _logger.Here().Error("{ErrorCode} The discount coupon delete failed.", ErrorCode.OperationFailed);
                throw new RpcException(new Status(StatusCode.Internal, ErrorMessages.Operationfailed));
            }

            _logger.Here().Information("Coupon is deleted");
            _logger.Here().MethodExited();
            return new DeletedResponse { Success =  true };
        }

    }
}
