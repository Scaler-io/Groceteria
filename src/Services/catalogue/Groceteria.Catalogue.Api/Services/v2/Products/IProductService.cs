using Groceteria.Catalogue.Api.Models.Core;
using Groceteria.Catalogue.Api.Models.Requests;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;

namespace Groceteria.Catalogue.Api.Services.v2.Products
{
    public interface IProductService
    {
        Task<Result<Pagination<ProductResponse>>> GetAllProducts(RequestQuery query);
        Task<Result<ProductResponse>> GetProductById(string id);
        Task<Result<bool>> CreateProduct(ProductUpsertRequest request);
        Task DeleteProduct(string id);
    }
}
