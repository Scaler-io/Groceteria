using AutoMapper;
using Groceteria.Catalogue.Api.DataAccess.Repositories;
using Groceteria.Catalogue.Api.Entities;
using Groceteria.Catalogue.Api.Models.Constants;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace Groceteria.Catalogue.Api.Services.v2.Products
{
    public class ProductService : IProductService
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public ProductService(ILogger logger, IMongoRepository<Product> productRepository, IMapper mapper)
        {
            _logger = logger;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<ProductResponse>>> GetAllProducts()
        {
            _logger.Here().MethodEnterd();
            var products = await _productRepository.GetAllAsync(MongodbCollectionNames.Products);
            if(products == null || products.Count == 0)
            {
                _logger.Here().Warning("No products available in store. {@ErrorCode}", ErrorCode.NotFound);
                return Result<IReadOnlyList<ProductResponse>>.Failure(ErrorCode.NotFound, "No products available in store.");
            }

            var response = _mapper.Map<IReadOnlyList<ProductResponse>>(products);

            _logger.Here().Information("Product list fetch success. {@response}", response);
            _logger.Here().MethodExited();
            return Result<IReadOnlyList<ProductResponse>>.Success(response);
        }
    }
}
