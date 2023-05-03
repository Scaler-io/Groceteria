namespace Groceteria.Catalogue.Api.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Color { get; set; }
        public string SKU { get; set; }
        public string Image { get; set; }
    }
}
