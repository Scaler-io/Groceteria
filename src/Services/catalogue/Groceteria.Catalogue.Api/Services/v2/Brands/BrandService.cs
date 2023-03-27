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

namespace Groceteria.Catalogue.Api.Services.v2.Brands
{
    public class BrandService : ProductCatalogueServiceBase, IBrandService
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;
        private readonly string _brandCollection;

        public BrandService(ILogger logger, 
            IMongoRepository<Brand> brandRepository, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor, logger)
        {
            _logger = logger;
            _brandRepository = brandRepository;
            _mapper = mapper;
            _brandCollection = MongodbCollectionNames.Brands;
        }

        public async Task<Result<Pagination<BrandResponse>>> GetAllBrands(RequestQuery query)
        {
            _logger.Here().MethodEnterd();

            var brands = await _brandRepository.GetAllAsync(_brandCollection, query.PageSize, query.PageIndex);
            if (brands == null || brands.Count == 0)
            {
                _logger.Here().Warning("No brands available in store. {@errorCode}", ErrorCode.NotFound);
                return Result<Pagination<BrandResponse>>.Failure(ErrorCode.NotFound, "No brands available in the store");
            }
            var response = _mapper.Map<IReadOnlyList<BrandResponse>>(brands);
            SetupPaginationResponseHeader(query, response.Count);
            _logger.Here().Information("Brand list fetch success. {@response}", response);
            _logger.Here().MethodExited();
            return Result<Pagination<BrandResponse>>.Success(new Pagination<BrandResponse>(query.PageIndex, query.PageSize, response.Count, response));
        }

        public async Task<Result<BrandResponse>> GetBrandById(string id)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - get brand {@id}", id);

            var brand = await _brandRepository.GetByIdAsync(id, _brandCollection);
            if(brand == null)
            {
                _logger.Here().Warning("No brand was found. {@ErrorCode}", ErrorCode.NotFound);
                return Result<BrandResponse>.Failure(ErrorCode.NotFound, "No brand was found");
            }

            var response = _mapper.Map<BrandResponse>(brand);

            _logger.Here().Information("Brand fetch success - {@brand}", brand);
            _logger.Here().MethodExited();
            return Result<BrandResponse>.Success(response);
        }

        public async Task<Result<bool>> CreateBrand(BrandUpsertRequest request)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - create brand {@request}", request);

            var entity = _mapper.Map<Brand>(request);
            await _brandRepository.UpsertAsync(entity, _brandCollection);

            _logger.Here().Information("New brand created successfully");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task DeleteBrand(string id)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - delete brand {@id}", id);
            await _brandRepository.DeleteAsync(id, _brandCollection);
            _logger.Here().Information("Brand deleted successfully");
            _logger.Here().MethodExited();
        }
    }}
