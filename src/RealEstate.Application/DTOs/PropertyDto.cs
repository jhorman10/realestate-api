namespace RealEstate.Application.DTOs;

public class PropertyDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public string OwnerId { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Owner information
    public OwnerDto? Owner { get; set; }
    
    // Main image (first enabled image)
    public string? ImageUrl { get; set; }
    
    // All images
    public List<PropertyImageDto> Images { get; set; } = new();
}

public class PropertyDetailDto : PropertyDto
{
    public List<PropertyTraceDto> Traces { get; set; } = new();
}

public class OwnerDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}

public class PropertyImageDto
{
    public string Id { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

public class PropertyTraceDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
}