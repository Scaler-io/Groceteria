using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;

namespace Groceteria.Catalogue.Api.Services.v2.Categories
{
    public interface ICategoryService
    {
        Task<Result<IReadOnlyList<CategoryResponse>>> GetAllCategories();
    }
}
