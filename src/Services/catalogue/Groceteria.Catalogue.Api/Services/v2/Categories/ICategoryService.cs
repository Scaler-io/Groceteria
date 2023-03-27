using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Requests;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;

namespace Groceteria.Catalogue.Api.Services.v2.Categories
{
    public interface ICategoryService
    {
        Task<Result<Pagination<CategoryResponse>>> GetAllCategories(RequestQuery query);
        Task<Result<CategoryResponse>> GetCatgoryById(string id);
        Task<Result<bool>> Createcategory(CategoryUpsertRequest request);
        Task DeleteCategory(string id);
    }
}
