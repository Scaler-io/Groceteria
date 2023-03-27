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

namespace Groceteria.Catalogue.Api.Services.v2.Categories
{
    public class CategoryService : ProductCatalogueServiceBase, ICategoryService
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly string _categoryCollection;

        public CategoryService(ILogger logger,
            IMongoRepository<Category> categoryRepository,
            IMapper mapper, 
            IHttpContextAccessor httpContext)
            :base(httpContext, logger)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _categoryCollection = MongodbCollectionNames.Categories;
        }

        public async Task<Result<bool>> Createcategory(CategoryUpsertRequest request)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - create category {@request}", request);

            var entity = _mapper.Map<Category>(request);
            await _categoryRepository.UpsertAsync(entity, _categoryCollection);

            _logger.Here().Information("New category created successfully");
            _logger.Here().MethodExited();
            return Result<bool>.Success(true);
        }

        public async Task DeleteCategory(string id)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - delete category {@id}", id);
            await _categoryRepository.DeleteAsync(id, _categoryCollection);
            _logger.Here().Information("Category deleted successfully");
            _logger.Here().MethodExited();
        }

        public async Task<Result<Pagination<CategoryResponse>>> GetAllCategories(RequestQuery query)
        {
            _logger.Here().MethodEnterd();

            var categories = await _categoryRepository.GetAllAsync(MongodbCollectionNames.Categories, query.PageSize, query.PageIndex);
            if (categories == null || categories.Count == 0)
            {
                _logger.Here().Warning("No categories available in store. {@errorCode}", ErrorCode.NotFound);
                return Result<Pagination<CategoryResponse>>.Failure(ErrorCode.NotFound, "No categories available in the store");
            }

            var response = _mapper.Map<IReadOnlyList<CategoryResponse>>(categories);
            SetupPaginationResponseHeader(query, response.Count);
            _logger.Here().Information("Category list fetch success. {@response}", response);
            _logger.Here().MethodExited();
            return Result<Pagination<CategoryResponse>>.Success(new Pagination<CategoryResponse>(query.PageIndex, query.PageSize, response.Count, response));
        }

        public async Task<Result<CategoryResponse>> GetCatgoryById(string id)
        {
            _logger.Here().MethodEnterd();
            _logger.Here().Information("Request - get category {@id}", id);

            var category = await _categoryRepository.GetByIdAsync(id, _categoryCollection);
            if (category == null)
            {
                _logger.Here().Warning("No category was found {@ErrorCode}", ErrorCode.NotFound);
                return Result<CategoryResponse>.Failure(ErrorCode.NotFound, "No category was found");
            }

            var response = _mapper.Map<CategoryResponse>(category);

            _logger.Here().Information("Category fetch success {response}", response);
            _logger.Here().MethodExited();

            return Result<CategoryResponse>.Success(response);
        }
    }
}
