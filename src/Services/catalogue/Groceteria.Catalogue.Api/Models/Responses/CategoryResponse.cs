using Groceteria.Catalogue.Api.Models.Core;

namespace Groceteria.Catalogue.Api.Models.Responses
{
    public class CategoryResponse
    {
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public string ImageLink { get; set; }
        public MetaData MetaData { get; set; }
    }
}
