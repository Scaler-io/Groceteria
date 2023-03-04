using Groceteria.Catalogue.Api.Models.Requests;
using Groceteria.Catalogue.Api.Models.Responses;
using Groceteria.Shared.Core;

namespace Groceteria.Catalogue.Api.Services.v2.Brands
{
    public interface IBrandService
    {
        Task<Result<IReadOnlyList<BrandResponse>>> GetAllBrands();
        Task<Result<BrandResponse>> GetBrandById(string id);
        Task<Result<bool>> CreateBrand(BrandUpsertRequest request);
        Task DeleteBrand(string id);
    }
}
