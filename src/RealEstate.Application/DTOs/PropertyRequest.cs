using System.ComponentModel.DataAnnotations;
using RealEstate.Application.Attributes;

namespace RealEstate.Application.DTOs;

public class CreatePropertyRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string CodeInternal { get; set; } = string.Empty;
    
    [Required]
    [Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100")]
    public int Year { get; set; }
    
    [Required]
    [ObjectId(ErrorMessage = "OwnerId must be a valid ObjectId")]
    public string OwnerId { get; set; } = string.Empty;
}

public class UpdatePropertyRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string CodeInternal { get; set; } = string.Empty;
    
    [Required]
    [Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100")]
    public int Year { get; set; }
    
    [Required]
    [ObjectId(ErrorMessage = "OwnerId must be a valid ObjectId")]
    public string OwnerId { get; set; } = string.Empty;
    
    public bool Enabled { get; set; } = true;
}

public class PropertyFilterRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? PriceMin { get; set; }
    public decimal? PriceMax { get; set; }
    public string? OwnerId { get; set; }
    public int? Year { get; set; }
    public bool? Enabled { get; set; } = true;
    
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
    
    [Range(1, 100)]
    public int PageSize { get; set; } = 10;
}