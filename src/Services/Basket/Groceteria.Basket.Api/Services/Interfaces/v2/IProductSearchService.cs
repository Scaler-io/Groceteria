using Groceteria.Basket.Api.Models.Responses;
using Groceteria.Shared.Core;

namespace Groceteria.Basket.Api.Services.Interfaces.v2
{
    public interface IProductSearchService
    {
        Task<Result<IEnumerable<ProductSearchResponse>>> ProductSearchAsync(string prodictIds);
    }
}
