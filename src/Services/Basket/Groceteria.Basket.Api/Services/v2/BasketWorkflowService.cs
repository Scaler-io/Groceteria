using AutoMapper;
using Groceteria.Basket.Api.DataAccess.Interfaces;
using Groceteria.Basket.Api.Entities;
using Groceteria.Basket.Api.Models.Requests;
using Groceteria.Basket.Api.Models.Responses;
using Groceteria.Basket.Api.Services.Grpc;
using Groceteria.Basket.Api.Services.Interfaces.Grpc;
using Groceteria.Basket.Api.Services.Interfaces.v2;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace Groceteria.Basket.Api.Services.v2
{
    public class BasketWorkflowService: IBasketWorkflowService
    { 
        private readonly ILogger _logger;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IProductSearchService _productSearchService;
        private readonly IDiscountGrpcService _discountGrpcService;

        public BasketWorkflowService(ILogger logger,
            IBasketRepository basketRepository,
            IMapper mapper,
            IProductSearchService productSearchService,
            IDiscountGrpcService discountGrpcService)
        {
            _logger = logger;
            _basketRepository = basketRepository;
            _mapper = mapper;
            _productSearchService = productSearchService;
            _discountGrpcService = discountGrpcService;
        }

        public async Task<Result<ShoppingCartResponse>> GetBasket(ShoppingCartFetchRequest request, RequestQuery queryParams)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request getBasket, {@username}", request.Username);

            var basket = await _basketRepository.GetCart(request.Username);

            if(basket == null)
            {
                _logger.Here().Warning("No basket was found with username {@username}", request.Username);
                return Result<ShoppingCartResponse>.Success(new ShoppingCartResponse(request.Username));
            }

            var response = _mapper.Map<ShoppingCartResponse>(basket);

            if(queryParams != null)
            {
                response.Items = response.Items.Skip((queryParams.PageIndex-1)* queryParams.PageSize).Take(queryParams.PageSize);
            }

            _logger.Here().Information("basket fetched - {@basket}", response);
            _logger.Here().MethodExited();

            return Result<ShoppingCartResponse>.Success(response);
        }

        public async Task<Result<ShoppingCartResponse>> UpdateBasket(ShoppingCartCreateRequest request)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request UpdateBasket - {@request}", request);


            var basket = await _basketRepository.GetCart(request.Username);
            if(basket == null)
            {
                basket = new ShoppingCart
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }

            var basketItems = await PrepareBasketItemsAsync(request);
            _logger.Here()
                   .Information("shopping cart items prepared {@items}", basketItems);

            var itemsWithDiscount = new List<ShoppingCartItem>(); 


            foreach(var item in basketItems)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductId, item.Name);
                if (coupon.IsSuccess)
                {
                    item.Price -= coupon.Value.Amount;
                    _logger.Here()
                        .Information("Coupon is applied of amount {@amount}. current price of {@productName} is {@currentPrice}", 
                        coupon.Value.Amount,
                        item.Name,
                        item.Price);
                    itemsWithDiscount.Add(item);
                }
            }

            if (itemsWithDiscount.Any())
            {
                basket.Items = itemsWithDiscount;
            }
            else
            {
                basket.Items = basketItems;
            }

            var basketResponse = await _basketRepository.UpdateBasket(basket);
            if(basketResponse == null)
            {
                _logger.Here().Error("{@ErrorCode} - basket update failure", ErrorCode.InternalServerError);
                return Result<ShoppingCartResponse>.Failure(ErrorCode.InternalServerError);
            }

            var mappedResult = _mapper.Map<ShoppingCartResponse>(basketResponse);

            _logger.Here().Information("basket update success {@basket}", mappedResult);
            _logger.Here().MethodExited();
            return Result<ShoppingCartResponse>.Success(mappedResult);
        }

        public async Task DeleteBasket(string username)
        {
            _logger.Here().MethodEnterd();
            await _basketRepository.DeleteCart(username);
            _logger.Here().Information("Cart delete for username {@username}", username);
            _logger.Here().MethodExited();
        }

        private async Task<IEnumerable<ShoppingCartItem>> PrepareBasketItemsAsync(ShoppingCartCreateRequest request)
        {  
            var productIds = string.Join(",", request.Items.Select(item => item.ProductId));
            var productCatalogues = await _productSearchService.ProductSearchAsync(productIds);

            return request.Items.Select(basketItem =>
            {
                var catalogue = productCatalogues.Value.Where(catalogue => catalogue.Id == basketItem.ProductId).FirstOrDefault();
                return new ShoppingCartItem
                {
                    ProductId = catalogue.Id,
                    Brand = catalogue.Brand,
                    Category = catalogue.Category,
                    Color = catalogue.Color,
                    Description = catalogue.Description,
                    Image = catalogue.Image,
                    Name = catalogue.Name,
                    Price  = catalogue.Price,
                    Quantity = basketItem.Quantity,
                    SKU = catalogue.SKU,
                    Summary = catalogue.Summary
                };
            });
        }
    }
}
