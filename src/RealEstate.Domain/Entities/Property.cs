using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.Domain.Entities;

public class Property
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;
    
    [BsonElement("price")]
    public decimal Price { get; set; }
    
    [BsonElement("codeInternal")]
    public string CodeInternal { get; set; } = string.Empty;
    
    [BsonElement("year")]
    public int Year { get; set; }
    
    [BsonElement("ownerId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string OwnerId { get; set; } = string.Empty;
    
    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [BsonIgnore]
    public Owner? Owner { get; set; }
    
    [BsonIgnore]
    public List<PropertyImage> Images { get; set; } = new();
    
    [BsonIgnore]
    public List<PropertyTrace> Traces { get; set; } = new();
}

public class Owner
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;
    
    [BsonElement("photo")]
    public string Photo { get; set; } = string.Empty;
    
    [BsonElement("birthday")]
    public DateTime Birthday { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class PropertyImage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    [BsonElement("propertyId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PropertyId { get; set; } = string.Empty;
    
    [BsonElement("file")]
    public string File { get; set; } = string.Empty;
    
    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class PropertyTrace
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    
    [BsonElement("propertyId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string PropertyId { get; set; } = string.Empty;
    
    [BsonElement("dateSale")]
    public DateTime DateSale { get; set; }
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("value")]
    public decimal Value { get; set; }
    
    [BsonElement("tax")]
    public decimal Tax { get; set; }
    
    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
