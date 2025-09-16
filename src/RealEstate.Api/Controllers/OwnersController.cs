using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnersController(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OwnerDto>>>> GetOwners(CancellationToken ct = default)
    {
        try
        {
            var owners = await _ownerRepository.GetOwnersAsync(ct);
            var ownerDtos = owners.Select(MapToOwnerDto).ToList();

            var response = ApiResponse<IEnumerable<OwnerDto>>.SuccessResult(ownerDtos);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<IEnumerable<OwnerDto>>.ErrorResult($"Error retrieving owners: {ex.Message}");
            return StatusCode(500, response);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OwnerDto?>>> GetOwner(string id, CancellationToken ct = default)
    {
        try
        {
            var owner = await _ownerRepository.GetOwnerByIdAsync(id, ct);
            
            if (owner == null)
            {
                var notFoundResponse = ApiResponse<OwnerDto?>.ErrorResult("Owner not found");
                return NotFound(notFoundResponse);
            }

            var ownerDto = MapToOwnerDto(owner);
            var response = ApiResponse<OwnerDto?>.SuccessResult(ownerDto);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<OwnerDto?>.ErrorResult($"Error retrieving owner: {ex.Message}");
            return StatusCode(500, response);
        }
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
}