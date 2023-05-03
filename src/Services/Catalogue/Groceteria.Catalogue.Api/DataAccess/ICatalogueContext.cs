using Groceteria.Catalogue.Api.Entities;
using MongoDB.Driver;

namespace Groceteria.Catalogue.Api.DataAccess
{
    public interface ICatalogueContext
    {
        IMongoDatabase GetMongoDatabaseInstance();
    }
}
