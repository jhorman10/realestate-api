using MongoDB.Driver;
using MongoDB.Bson;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Repositories;

public class OwnerRepository : BaseRepository<Owner>, IOwnerRepository
{
    public OwnerRepository(IMongoDatabase database) : base(database, "owners")
    {
    }

    public async Task<IEnumerable<Owner>> GetOwnersAsync(CancellationToken ct = default)
    {
        return await Collection
            .Find(_ => true)
            .Sort(Builders<Owner>.Sort.Ascending(o => o.Name))
            .ToListAsync(ct);
    }

    public async Task<Owner?> GetOwnerByIdAsync(string id, CancellationToken ct = default)
    {
        // First validate if the id is a valid ObjectId
        if (!ObjectId.TryParse(id, out _))
        {
            return null;
        }

        return await Collection
            .Find(o => o.Id == id)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<Owner> CreateOwnerAsync(Owner owner, CancellationToken ct = default)
    {
        owner.CreatedAt = DateTime.UtcNow;
        owner.UpdatedAt = DateTime.UtcNow;
        
        await Collection.InsertOneAsync(owner, cancellationToken: ct);
        return owner;
    }

    public async Task<Owner> UpdateOwnerAsync(Owner owner, CancellationToken ct = default)
    {
        owner.UpdatedAt = DateTime.UtcNow;
        
        var filter = Builders<Owner>.Filter.Eq(o => o.Id, owner.Id);
        var result = await Collection.ReplaceOneAsync(filter, owner, cancellationToken: ct);
        
        if (result.MatchedCount == 0)
            throw new InvalidOperationException($"Owner with ID {owner.Id} not found");

        return owner;
    }

    public async Task<bool> DeleteOwnerAsync(string id, CancellationToken ct = default)
    {
        var result = await Collection.DeleteOneAsync(o => o.Id == id, ct);
        return result.DeletedCount > 0;
    }

    public async Task<bool> OwnerExistsAsync(string id, CancellationToken ct = default)
    {
        // First validate if the id is a valid ObjectId
        if (!ObjectId.TryParse(id, out _))
        {
            return false;
        }

        var count = await Collection.CountDocumentsAsync(o => o.Id == id, cancellationToken: ct);
        return count > 0;
    }
}