using Groceteria.IdentityManager.Api.Models.Core;

namespace Groceteria.IdentityManager.Api.Services.Search
{
    public interface ISearchService<TDocument> where TDocument : class
    {
        Task<Result<bool>> SeedDataAsync(TDocument document, string id, string index);
        Task<Result<bool>> SearchReIndex(IEnumerable<TDocument> documents, string index);
    }
}
