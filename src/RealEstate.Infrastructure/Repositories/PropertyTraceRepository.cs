using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Repositories;

public class PropertyTraceRepository : BaseRepository<PropertyTrace>, IPropertyTraceRepository
{
    public PropertyTraceRepository(IMongoDatabase database) : base(database, "property_traces")
    {
    }

    public async Task<IEnumerable<PropertyTrace>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken ct = default)
    {
        return await Collection
            .Find(trace => trace.PropertyId == propertyId)
            .Sort(Builders<PropertyTrace>.Sort.Descending(trace => trace.DateSale))
            .ToListAsync(ct);
    }

    public async Task<PropertyTrace?> GetTraceByIdAsync(string id, CancellationToken ct = default)
    {
        return await Collection
            .Find(trace => trace.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PropertyTrace> CreateTraceAsync(PropertyTrace trace, CancellationToken ct = default)
    {
        trace.CreatedAt = DateTime.UtcNow;
        
        await Collection.InsertOneAsync(trace, cancellationToken: ct);
        return trace;
    }

    public async Task<bool> DeleteTraceAsync(string id, CancellationToken ct = default)
    {
        var result = await Collection.DeleteOneAsync(trace => trace.Id == id, ct);
        return result.DeletedCount > 0;
    }
}