using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces;

public interface IPropertyService
{
    Task<ApiResponse<PagedResult<PropertyDto>>> GetPropertiesAsync(PropertyFilterRequest request, CancellationToken ct = default);
    Task<ApiResponse<PropertyDetailDto?>> GetPropertyByIdAsync(string id, CancellationToken ct = default);
    Task<ApiResponse<PropertyDto>> CreatePropertyAsync(CreatePropertyRequest request, CancellationToken ct = default);
    Task<ApiResponse<PropertyDto>> UpdatePropertyAsync(string id, UpdatePropertyRequest request, CancellationToken ct = default);
    Task<ApiResponse<bool>> DeletePropertyAsync(string id, CancellationToken ct = default);
}