using Groceteria.Catalogue.Api.Models.Core;

namespace Groceteria.Catalogue.Api.Models.Responses
{
    public class ProductResponse
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string SKU { get; set; }
        public MetaData MetaData { get; set; }
    }
}
