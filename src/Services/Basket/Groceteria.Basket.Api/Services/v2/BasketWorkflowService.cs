using AutoMapper;
using Groceteria.Basket.Api.DataAccess.Interfaces;
using Groceteria.Basket.Api.Entities;
using Groceteria.Basket.Api.Models.Requests;
using Groceteria.Basket.Api.Models.Requests.BasketCheckout;
using Groceteria.Basket.Api.Models.Responses;
using Groceteria.Basket.Api.Services.Interfaces.Grpc;
using Groceteria.Basket.Api.Services.Interfaces.v2;
using Groceteria.Infrastructure.EventBus.Message.Events.BasketEvents;
using Groceteria.Infrastructure.EventBus.Message.Models;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using Grpc.Core;
using MassTransit;
using System.Net;
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
        private readonly IPublishEndpoint _busEvent;

        public BasketWorkflowService(ILogger logger,
            IBasketRepository basketRepository,
            IMapper mapper,
            IProductSearchService productSearchService,
            IDiscountGrpcService discountGrpcService,
            IPublishEndpoint busEvent)
        {
            _logger = logger;
            _basketRepository = basketRepository;
            _mapper = mapper;
            _productSearchService = productSearchService;
            _discountGrpcService = discountGrpcService;
            _busEvent = busEvent;
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

            var evaluatedItems = new List<ShoppingCartItem>(); 

            foreach(var item in basketItems)
            {
                try
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
                        evaluatedItems.Add(item);
                    }
                }catch(RpcException ex)
                {
                    if(ex.StatusCode == StatusCode.NotFound)
                    {
                        _logger.Here().Error("{@ErrorCode} - {@message}", ErrorCode.NotFound, ex.Message);
                        evaluatedItems.Add(item);
                    }
                    else
                    {
                        throw ex;
                    }                 
                }
                
            }
             
            basket.Items = evaluatedItems;

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

        public async Task<Result<EventResponse>> CheckoutBasket(BasketCheckoutRequest request, CancellationToken cancellationToken)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - basket checkout for {@username}", request.UserName);

            var basket = await _basketRepository.GetCart(request.UserName);
            if(basket == null)
            {
                _logger.Here().Error("No basket found for {@username}", request.UserName);
                return Result<EventResponse>.Failure(ErrorCode.NotFound);
            }

            var message = _mapper.Map<BasketCheckoutEvent>(request);
            
            var publishTask =  _busEvent.Publish(message);
            var deleteCartTask = _basketRepository.DeleteCart(request.UserName);

            await Task.WhenAll(publishTask, deleteCartTask);

            _logger.Here().Information("Basket checkout message published");
            _logger.Here().Information("basket deleted for {@username}", request.UserName);

            _logger.Here().MethodExited();
            return Result<EventResponse>.Success(new EventResponse((int)HttpStatusCode.OK, "Data processing in background"));
        }
    }
}
