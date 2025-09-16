using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IPropertyTraceRepository _traceRepository;

    public PropertyService(
        IPropertyRepository propertyRepository, 
        IOwnerRepository ownerRepository,
        IPropertyTraceRepository traceRepository)
    {
        _propertyRepository = propertyRepository;
        _ownerRepository = ownerRepository;
        _traceRepository = traceRepository;
    }

    public async Task<ApiResponse<PagedResult<PropertyDto>>> GetPropertiesAsync(PropertyFilterRequest request, CancellationToken ct = default)
    {
        try
        {
            var filter = new PropertyFilter
            {
                Name = request.Name,
                Address = request.Address,
                PriceMin = request.PriceMin,
                PriceMax = request.PriceMax,
                OwnerId = request.OwnerId,
                Year = request.Year,
                Enabled = request.Enabled
            };

            var (items, total) = await _propertyRepository.GetPropertiesAsync(filter, request.Page, request.PageSize, ct);

            var propertyDtos = items.Select(MapToPropertyDto).ToList();

            var result = new PagedResult<PropertyDto>
            {
                Items = propertyDtos,
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return ApiResponse<PagedResult<PropertyDto>>.SuccessResult(result);
        }
        catch (Exception ex)
        {
            return ApiResponse<PagedResult<PropertyDto>>.ErrorResult($"Error retrieving properties: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PropertyDetailDto?>> GetPropertyByIdAsync(string id, CancellationToken ct = default)
    {
        try
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(id, ct);
            
            if (property == null)
            {
                return ApiResponse<PropertyDetailDto?>.ErrorResult("Property not found");
            }

            // Load traces
            var traces = await _traceRepository.GetTracesByPropertyIdAsync(id, ct);
            property.Traces = traces.ToList();

            var propertyDetailDto = MapToPropertyDetailDto(property);

            return ApiResponse<PropertyDetailDto?>.SuccessResult(propertyDetailDto);
        }
        catch (Exception ex)
        {
            return ApiResponse<PropertyDetailDto?>.ErrorResult($"Error retrieving property: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PropertyDto>> CreatePropertyAsync(CreatePropertyRequest request, CancellationToken ct = default)
    {
        try
        {
            // Validate owner exists
            if (!await _ownerRepository.OwnerExistsAsync(request.OwnerId, ct))
            {
                return ApiResponse<PropertyDto>.ErrorResult("Owner not found");
            }

            var property = new Property
            {
                Name = request.Name,
                Address = request.Address,
                Price = request.Price,
                CodeInternal = request.CodeInternal,
                Year = request.Year,
                OwnerId = request.OwnerId,
                Enabled = true
            };

            var createdProperty = await _propertyRepository.CreatePropertyAsync(property, ct);
            var propertyDto = MapToPropertyDto(createdProperty);

            return ApiResponse<PropertyDto>.SuccessResult(propertyDto, "Property created successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PropertyDto>.ErrorResult($"Error creating property: {ex.Message}");
        }
    }

    public async Task<ApiResponse<PropertyDto>> UpdatePropertyAsync(string id, UpdatePropertyRequest request, CancellationToken ct = default)
    {
        try
        {
            var existingProperty = await _propertyRepository.GetPropertyByIdAsync(id, ct);
            if (existingProperty == null)
            {
                return ApiResponse<PropertyDto>.ErrorResult("Property not found");
            }

            // Validate owner exists
            if (!await _ownerRepository.OwnerExistsAsync(request.OwnerId, ct))
            {
                return ApiResponse<PropertyDto>.ErrorResult("Owner not found");
            }

            existingProperty.Name = request.Name;
            existingProperty.Address = request.Address;
            existingProperty.Price = request.Price;
            existingProperty.CodeInternal = request.CodeInternal;
            existingProperty.Year = request.Year;
            existingProperty.OwnerId = request.OwnerId;
            existingProperty.Enabled = request.Enabled;

            var updatedProperty = await _propertyRepository.UpdatePropertyAsync(existingProperty, ct);
            var propertyDto = MapToPropertyDto(updatedProperty);

            return ApiResponse<PropertyDto>.SuccessResult(propertyDto, "Property updated successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<PropertyDto>.ErrorResult($"Error updating property: {ex.Message}");
        }
    }

    public async Task<ApiResponse<bool>> DeletePropertyAsync(string id, CancellationToken ct = default)
    {
        try
        {
            var exists = await _propertyRepository.PropertyExistsAsync(id, ct);
            if (!exists)
            {
                return ApiResponse<bool>.ErrorResult("Property not found");
            }

            var result = await _propertyRepository.DeletePropertyAsync(id, ct);

            if (result)
            {
                return ApiResponse<bool>.SuccessResult(true, "Property deleted successfully");
            }

            return ApiResponse<bool>.ErrorResult("Failed to delete property");
        }
        catch (Exception ex)
        {
            return ApiResponse<bool>.ErrorResult($"Error deleting property: {ex.Message}");
        }
    }

    private static PropertyDto MapToPropertyDto(Property property)
    {
        return new PropertyDto
        {
            Id = property.Id,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            OwnerId = property.OwnerId,
            Enabled = property.Enabled,
            CreatedAt = property.CreatedAt,
            UpdatedAt = property.UpdatedAt,
            Owner = property.Owner != null ? MapToOwnerDto(property.Owner) : null,
            ImageUrl = property.Images?.FirstOrDefault(img => img.Enabled)?.File,
            Images = property.Images?.Select(MapToPropertyImageDto).ToList() ?? new List<PropertyImageDto>()
        };
    }

    private static PropertyDetailDto MapToPropertyDetailDto(Property property)
    {
        return new PropertyDetailDto
        {
            Id = property.Id,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            CodeInternal = property.CodeInternal,
            Year = property.Year,
            OwnerId = property.OwnerId,
            Enabled = property.Enabled,
            CreatedAt = property.CreatedAt,
            UpdatedAt = property.UpdatedAt,
            Owner = property.Owner != null ? MapToOwnerDto(property.Owner) : null,
            ImageUrl = property.Images?.FirstOrDefault(img => img.Enabled)?.File,
            Images = property.Images?.Select(MapToPropertyImageDto).ToList() ?? new List<PropertyImageDto>(),
            Traces = property.Traces?.Select(MapToPropertyTraceDto).ToList() ?? new List<PropertyTraceDto>()
        };
    }

    private static OwnerDto MapToOwnerDto(Owner owner)
    {
        return new OwnerDto
        {
            Id = owner.Id,
            Name = owner.Name,
            Address = owner.Address,
            Photo = owner.Photo,
            Birthday = owner.Birthday
        };
    }

    private static PropertyImageDto MapToPropertyImageDto(PropertyImage image)
    {
        return new PropertyImageDto
        {
            Id = image.Id,
            File = image.File,
            Enabled = image.Enabled
        };
    }

    private static PropertyTraceDto MapToPropertyTraceDto(PropertyTrace trace)
    {
        return new PropertyTraceDto
        {
            Id = trace.Id,
            DateSale = trace.DateSale,
            Name = trace.Name,
            Value = trace.Value,
            Tax = trace.Tax
        };
    }
}