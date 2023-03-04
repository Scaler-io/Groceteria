using AutoMapper;
using Groceteria.Catalogue.Api.DataAccess.Repositories;
using Groceteria.Catalogue.Api.Entities;
using Groceteria.Catalogue.Api.Models.Constants;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Groceteria.Shared.Extensions;
using ILogger = Serilog.ILogger;

namespace Groceteria.Catalogue.Api.Services.v2.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger _logger;
        private readonly IMongoRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ILogger logger,
            IMongoRepository<Category> categoryRepository,
            IMapper mapper)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<CategoryResponse>>> GetAllCategories()
        {
            _logger.Here().MethodEnterd();

            var categories = await _categoryRepository.GetAllAsync(MongodbCollectionNames.Categories);
            if (categories == null || categories.Count == 0)
            {
                _logger.Here().Warning("No categories available in store. {@errorCode}", ErrorCode.NotFound);
                return Result<IReadOnlyList<CategoryResponse>>.Failure(ErrorCode.NotFound, "No categories available in the store");
            }

            var response = _mapper.Map<IReadOnlyList<CategoryResponse>>(categories);

            _logger.Here().Information("Category list fetch success. {@response}", response);
            _logger.Here().MethodExited();
            return Result<IReadOnlyList<CategoryResponse>>.Success(response);
        }
    }
}
