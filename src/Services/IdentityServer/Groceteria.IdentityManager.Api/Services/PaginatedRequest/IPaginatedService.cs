using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;

namespace Groceteria.IdentityManager.Api.Services.PaginatedRequest
{
    public interface IPaginatedService<TDocument> where TDocument: class
    {
        Task<Result<Pagination<TDocument>>> GetPaginatedData(RequestQuery query, string correlationId, SearchIndex searchIndex);
        Task<Result<long>> GetCount(string correlationID, SearchIndex searchIndex);
    }
}
