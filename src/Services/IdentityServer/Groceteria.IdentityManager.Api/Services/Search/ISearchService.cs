using Groceteria.IdentityManager.Api.Models.Core;

namespace Groceteria.IdentityManager.Api.Services.Search
{
    public interface ISearchService<TDocument> where TDocument : class
    {
        Task<Result<bool>> SeedDataAsync(TDocument document, string id, string index);
        Task<Result<bool>> UpdateDocumentAsync(TDocument updatedDocument, Dictionary<string, string> fieldValue, string index);
        Task<Result<bool>> SearchReIndex(IEnumerable<TDocument> documents, string index);
        Task<Result<bool>> RemoveDocumentFromIndex(Dictionary<string, object> query, string index);
    }
}
