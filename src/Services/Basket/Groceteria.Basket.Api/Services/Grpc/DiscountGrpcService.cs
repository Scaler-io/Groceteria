using Groceteria.Basket.Api.Services.Interfaces.Grpc;
using Groceteria.Discount.Grpc.Protos;
using Groceteria.Shared.Constants;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace Groceteria.Basket.Api.Services.Grpc
{
    public class DiscountGrpcService: IDiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountGrpcService;
        private readonly ILogger _logger;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountGrpcService, ILogger logger)
        {
            _discountGrpcService = discountGrpcService;
            _logger = logger;
        }

        public async Task<Result<CouponModel>> GetDiscount(string productId, string productName)
        {
            _logger.Here().MethodEnterd();

            var discountRequest = new GetDiscountRequest { ProductId = productId, ProductName = productName };
            var response = new CouponModel ();
            response = await _discountGrpcService.GetDiscountAsync(discountRequest);
            if (response == null)
            {
                _logger.Here().Error("No discount was found for the product {@productName}", productName);
                return Result<CouponModel>.Failure(ErrorCode.NotFound, ErrorMessages.NotFound);
            }

            _logger.Here().Information("Coupon found for product {@productName}", productName);
            _logger.Here().MethodExited();
            return Result<CouponModel>.Success(response);
        }
    }
}
