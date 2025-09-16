using MongoDB.Driver;
using RealEstate.Infrastructure.Configuration;

namespace RealEstate.Infrastructure.Repositories;

public abstract class BaseRepository<T>
{
    protected readonly IMongoCollection<T> Collection;
    protected readonly IMongoDatabase Database;

    protected BaseRepository(IMongoDatabase database, string collectionName)
    {
        Database = database;
        Collection = database.GetCollection<T>(collectionName);
    }
}