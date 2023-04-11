using AutoMapper;
using Groceteria.Catalogue.Api.DataAccess.Repositories;
using Groceteria.Catalogue.Api.Entities;
using Groceteria.Catalogue.Api.Models.Constants;
using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Requests;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace Groceteria.Catalogue.Api.Services.v2.Products
{
    public class ProductService : ProductCatalogueServiceBase, IProductService
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly string _productCollection;

        public ProductService(ILogger logger, 
            IMongoRepository<Product> productRepository, 
            IMapper mapper,
            IHttpContextAccessor httpContext)
            :base(httpContext, logger)
        {
            _logger = logger;
            _productRepository = productRepository;
            _mapper = mapper;
            _productCollection = MongodbCollectionNames.Products;
        }

        public async Task<Result<bool>> CreateProduct(ProductUpsertRequest request)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - create product. {@request}", request);

            var product = _mapper.Map<Product>(request);
            await _productRepository.UpsertAsync(product, _productCollection);


            _logger.Here().Information("Product created successfully. {@id}", product.Id);
            _logger.Here().MethodExited();

            return Result<bool>.Success(true);
        }

        public async Task DeleteProduct(string id)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - delete product {@id}", id);
            await _productRepository.DeleteAsync(id, _productCollection);
            _logger.Here().Information("Product deleted successfully");
            _logger.Here().MethodExited();
        }

        public async Task<Result<Pagination<ProductResponse>>> GetAllProducts(RequestQuery query)
        {
            _logger.Here().MethodEnterd();
            var products = await _productRepository.GetAllAsync(MongodbCollectionNames.Products, 6, 1);
            if(products == null || products.Count == 0)
            {
                _logger.Here().Warning("No products available in store. {@ErrorCode}", ErrorCode.NotFound);
                return Result<Pagination<ProductResponse>>.Failure(ErrorCode.NotFound, "No products available in store.");
            }
            var response = _mapper.Map<IReadOnlyList<ProductResponse>>(products);
            SetupPaginationResponseHeader(query, response.Count);
            _logger.Here().Information("Product list fetch success. {@response}", response);
            _logger.Here().MethodExited();
            return Result<Pagination<ProductResponse>>.Success(new Pagination<ProductResponse>(query.PageIndex, query.PageSize, response.Count, response));
        }

        public async Task<Result<ProductResponse>> GetProductById(string id)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - get product {@id}", id);

            var product = await _productRepository.GetByIdAsync(id, _productCollection);
            if (product == null)
            {
                _logger.Here().Warning("No product was found. {@ErrorCode}", ErrorCode.NotFound);
                return Result<ProductResponse>.Failure(ErrorCode.NotFound, "No brand was found");
            }

            var response = _mapper.Map<ProductResponse>(product);

            _logger.Here().Information("Brand fetch success - {@brand}", product);
            _logger.Here().MethodExited();
            return Result<ProductResponse>.Success(response);
        }

        public async Task<Result<IEnumerable<ProductResponse>>> GetProductsFromBulkRequest(string productIds)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - get product list {@ids}", productIds);

            var ids = productIds.Split(",");
            var products = new List<Product>();
            foreach(var id in ids)
            {
                var product = await _productRepository.GetByIdAsync(id, _productCollection);             
                products.Add(product);
            }

            var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
            _logger.Here().Information("Brand fetch success - {@products}", products);
            _logger.Here().MethodExited();

            return Result<IEnumerable<ProductResponse>>.Success(productResponses);
        }
    }
}
