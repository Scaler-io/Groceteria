using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;

namespace Groceteria.Catalogue.Api.Services.v2.Brands
{
    public interface IBrandService
    {
        Task<Result<IReadOnlyList<BrandResponse>>> GetAllBrands();
    }
}
