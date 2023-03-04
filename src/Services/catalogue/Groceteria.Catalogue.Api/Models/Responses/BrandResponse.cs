using Groceteria.Catalogue.Api.Models.Core;

namespace Groceteria.Catalogue.Api.Models.Responses
{
    public class BrandResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageLink { get; set; }
        public MetaData MetaData { get; set; }
    }
}
