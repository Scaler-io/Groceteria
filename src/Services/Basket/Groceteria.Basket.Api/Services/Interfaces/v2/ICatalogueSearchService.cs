namespace Groceteria.Basket.Api.Services.Interfaces.v2
{
    public interface ICatalogueSearchService
    {
        Task GetBulkCatalogue(string productIds);
    }
}
