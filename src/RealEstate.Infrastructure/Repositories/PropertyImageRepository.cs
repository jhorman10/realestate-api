using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyImageRepository : BaseRepository<PropertyImage>, IPropertyImageRepository
{
    public PropertyImageRepository(IMongoDatabase database) : base(database, "property_images")
    {
    }

    public async Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(string propertyId, CancellationToken ct = default)
    {
        return await Collection
            .Find(img => img.PropertyId == propertyId && img.Enabled)
            .Sort(Builders<PropertyImage>.Sort.Ascending(img => img.CreatedAt))
            .ToListAsync(ct);
    }

    public async Task<PropertyImage?> GetImageByIdAsync(string id, CancellationToken ct = default)
    {
        return await Collection
            .Find(img => img.Id == id && img.Enabled)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PropertyImage> CreateImageAsync(PropertyImage image, CancellationToken ct = default)
    {
        image.CreatedAt = DateTime.UtcNow;
        
        await Collection.InsertOneAsync(image, cancellationToken: ct);
        return image;
    }

    public async Task<bool> DeleteImageAsync(string id, CancellationToken ct = default)
    {
        var filter = Builders<PropertyImage>.Filter.Eq(img => img.Id, id);
        var update = Builders<PropertyImage>.Update.Set(img => img.Enabled, false);

        var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: ct);
        return result.MatchedCount > 0;
    }
}