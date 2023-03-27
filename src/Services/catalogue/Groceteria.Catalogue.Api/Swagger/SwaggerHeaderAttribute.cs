namespace Groceteria.Catalogue.Api.Swagger
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerHeaderAttribute: Attribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }

        public SwaggerHeaderAttribute(string name, string type, string description = "", bool required = false)
        {
            Name = name;
            Type = type;
            Description = description;
            Required = required;
        }
    }
}
