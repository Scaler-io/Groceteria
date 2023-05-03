using Groceteria.Catalogue.Api.Entities;
using Groceteria.Catalogue.Api.Models.Core;

namespace Groceteria.Catalogue.Api.Models.Responses
{
    public class ProductResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string SKU { get; set; }
        public string Image { get; set; }
        public MetaData MetaData { get; set; }
    }
}
