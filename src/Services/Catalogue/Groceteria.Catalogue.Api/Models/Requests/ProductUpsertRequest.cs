using Destructurama.Attributed;
using Groceteria.Catalogue.Api.Entities;

namespace Groceteria.Catalogue.Api.Models.Requests
{
    public class ProductUpsertRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [LogMasked]
        public string CategoryId { get; set; }
        [LogMasked]
        public string BrandId { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        [LogMasked]
        public string SKU { get; set; }
        public string Image { get; set; }
    }
}
