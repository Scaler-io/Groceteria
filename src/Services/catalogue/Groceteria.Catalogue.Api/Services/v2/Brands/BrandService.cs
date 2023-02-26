using AutoMapper;
using Groceteria.Catalogue.Api.DataAccess.Repositories;
using Groceteria.Catalogue.Api.Entities;
using Groceteria.Catalogue.Api.Models.Constants;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace Groceteria.Catalogue.Api.Services.v2.Brands
{
    public class BrandService : IBrandService
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(ILogger logger, IMongoRepository<Brand> brandRepository, IMapper mapper)
        {
            _logger = logger;
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<BrandResponse>>> GetAllBrands()
        {
            _logger.Here().MethodEnterd();

            var brands = await _brandRepository.GetAllAsync(MongodbCollectionNames.Brands);
            if (brands == null || brands.Count == 0)
            {
                _logger.Here().Warning("No brands available in store. {@errorCode}", ErrorCode.NotFound);
                return Result<IReadOnlyList<BrandResponse>>.Failure(ErrorCode.NotFound, "No brands available in the store");
            }
            var response = _mapper.Map<IReadOnlyList<BrandResponse>>(brands);

            _logger.Here().Information("Brand list fetch success. {@response}", response);
            _logger.Here().MethodExited();
            return Result<IReadOnlyList<BrandResponse>>.Success(response);
        }
    }
}
