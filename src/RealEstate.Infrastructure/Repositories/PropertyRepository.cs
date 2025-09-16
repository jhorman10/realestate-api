using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
{
    private readonly IMongoDatabase _database;

    public PropertyRepository(IMongoDatabase database) : base(database, "properties")
    {
        _database = database;
    }

    public async Task<(IEnumerable<Property> Items, long Total)> GetPropertiesAsync(PropertyFilter filter, int page, int pageSize, CancellationToken ct = default)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        if (filter.Enabled.HasValue)
            filters.Add(filterBuilder.Eq(p => p.Enabled, filter.Enabled.Value));

        if (!string.IsNullOrWhiteSpace(filter.Name))
            filters.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filter.Name, "i")));

        if (!string.IsNullOrWhiteSpace(filter.Address))
            filters.Add(filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(filter.Address, "i")));

        if (filter.PriceMin.HasValue)
            filters.Add(filterBuilder.Gte(p => p.Price, filter.PriceMin.Value));

        if (filter.PriceMax.HasValue)
            filters.Add(filterBuilder.Lte(p => p.Price, filter.PriceMax.Value));

        if (!string.IsNullOrWhiteSpace(filter.OwnerId))
            filters.Add(filterBuilder.Eq(p => p.OwnerId, filter.OwnerId));

        if (filter.Year.HasValue)
            filters.Add(filterBuilder.Eq(p => p.Year, filter.Year.Value));

        var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

        var totalTask = Collection.CountDocumentsAsync(combinedFilter, cancellationToken: ct);
        
        var itemsTask = Collection
            .Find(combinedFilter)
            .Sort(Builders<Property>.Sort.Descending(p => p.CreatedAt))
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(ct);

        await Task.WhenAll(totalTask, itemsTask);

        var properties = itemsTask.Result;

        // Load related data
        var ownersCollection = _database.GetCollection<Owner>("owners");
        var imagesCollection = _database.GetCollection<PropertyImage>("property_images");

        foreach (var property in properties)
        {
            if (!string.IsNullOrEmpty(property.OwnerId))
            {
                property.Owner = await ownersCollection
                    .Find(o => o.Id == property.OwnerId)
                    .FirstOrDefaultAsync(ct);
            }

            property.Images = await imagesCollection
                .Find(img => img.PropertyId == property.Id && img.Enabled)
                .ToListAsync(ct);
        }

        return (properties, totalTask.Result);
    }

    public async Task<Property?> GetPropertyByIdAsync(string id, CancellationToken ct = default)
    {
        var property = await Collection
            .Find(p => p.Id == id && p.Enabled)
            .FirstOrDefaultAsync(ct);

        if (property != null)
        {
            var ownersCollection = _database.GetCollection<Owner>("owners");
            var imagesCollection = _database.GetCollection<PropertyImage>("property_images");

            if (!string.IsNullOrEmpty(property.OwnerId))
            {
                property.Owner = await ownersCollection
                    .Find(o => o.Id == property.OwnerId)
                    .FirstOrDefaultAsync(ct);
            }

            property.Images = await imagesCollection
                .Find(img => img.PropertyId == property.Id && img.Enabled)
                .ToListAsync(ct);
        }

        return property;
    }

    public async Task<Property> CreatePropertyAsync(Property property, CancellationToken ct = default)
    {
        property.CreatedAt = DateTime.UtcNow;
        property.UpdatedAt = DateTime.UtcNow;
        
        await Collection.InsertOneAsync(property, cancellationToken: ct);
        return property;
    }

    public async Task<Property> UpdatePropertyAsync(Property property, CancellationToken ct = default)
    {
        property.UpdatedAt = DateTime.UtcNow;
        
        var filter = Builders<Property>.Filter.Eq(p => p.Id, property.Id);
        var result = await Collection.ReplaceOneAsync(filter, property, cancellationToken: ct);
        
        if (result.MatchedCount == 0)
            throw new InvalidOperationException($"Property with ID {property.Id} not found");

        return property;
    }

    public async Task<bool> DeletePropertyAsync(string id, CancellationToken ct = default)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.Id, id);
        var update = Builders<Property>.Update
            .Set(p => p.Enabled, false)
            .Set(p => p.UpdatedAt, DateTime.UtcNow);

        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: ct);
        return result.MatchedCount > 0;
    }

    public async Task<bool> PropertyExistsAsync(string id, CancellationToken ct = default)
    {
        var count = await Collection.CountDocumentsAsync(p => p.Id == id && p.Enabled, cancellationToken: ct);
        return count > 0;
    }
}