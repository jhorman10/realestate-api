using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<(IEnumerable<Property> Items, long Total)> GetPropertiesAsync(PropertyFilter filter, int page, int pageSize, CancellationToken ct = default);
    Task<Property?> GetPropertyByIdAsync(string id, CancellationToken ct = default);
    Task<Property> CreatePropertyAsync(Property property, CancellationToken ct = default);
    Task<Property> UpdatePropertyAsync(Property property, CancellationToken ct = default);
    Task<bool> DeletePropertyAsync(string id, CancellationToken ct = default);
    Task<bool> PropertyExistsAsync(string id, CancellationToken ct = default);
}

public interface IOwnerRepository
{
    Task<IEnumerable<Owner>> GetOwnersAsync(CancellationToken ct = default);
    Task<Owner?> GetOwnerByIdAsync(string id, CancellationToken ct = default);
    Task<Owner> CreateOwnerAsync(Owner owner, CancellationToken ct = default);
    Task<Owner> UpdateOwnerAsync(Owner owner, CancellationToken ct = default);
    Task<bool> DeleteOwnerAsync(string id, CancellationToken ct = default);
    Task<bool> OwnerExistsAsync(string id, CancellationToken ct = default);
}

public interface IPropertyImageRepository
{
    Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(string propertyId, CancellationToken ct = default);
    Task<PropertyImage?> GetImageByIdAsync(string id, CancellationToken ct = default);
    Task<PropertyImage> CreateImageAsync(PropertyImage image, CancellationToken ct = default);
    Task<bool> DeleteImageAsync(string id, CancellationToken ct = default);
}

public interface IPropertyTraceRepository
{
    Task<IEnumerable<PropertyTrace>> GetTracesByPropertyIdAsync(string propertyId, CancellationToken ct = default);
    Task<PropertyTrace?> GetTraceByIdAsync(string id, CancellationToken ct = default);
    Task<PropertyTrace> CreateTraceAsync(PropertyTrace trace, CancellationToken ct = default);
    Task<bool> DeleteTraceAsync(string id, CancellationToken ct = default);
}

public class PropertyFilter
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public string? OwnerId { get; set; }
    public int? Year { get; set; }
    public bool? Enabled { get; set; } = true;
}
