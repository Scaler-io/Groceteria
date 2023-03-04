namespace Groceteria.Catalogue.Api.Configurations
{
    public class MongodbSettingOptions
    {
        public const string MongodbSettings = "MongodbSettings";
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
