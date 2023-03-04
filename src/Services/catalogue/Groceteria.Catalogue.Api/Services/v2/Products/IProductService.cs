using Groceteria.Catalogue.Api.Entities;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;

namespace Groceteria.Catalogue.Api.Services.v2.Products
{
    public interface IProductService
    {
        Task<Result<IReadOnlyList<ProductResponse>>> GetAllProducts();
    }
}
