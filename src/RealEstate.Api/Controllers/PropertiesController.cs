using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(IPropertyService propertyService, ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService;
        _logger = logger;
    }

    /// <summary>
    /// Get a paginated list of properties with optional filters
    /// </summary>
    /// <param name="request">Filter parameters and pagination settings</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Paginated list of properties</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PropertyDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PropertyDto>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<PropertyDto>>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PagedResult<PropertyDto>>>> GetProperties(
        [FromQuery] PropertyFilterRequest request, 
        CancellationToken ct = default)
    {
        _logger.LogInformation("Getting properties with filters: {@Request}", request);

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<PagedResult<PropertyDto>>.ErrorResult("Invalid request parameters", errors));
        }

        var result = await _propertyService.GetPropertiesAsync(request, ct);

        if (!result.Success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Get a specific property by ID with detailed information
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Property details including owner, images, and traces</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PropertyDetailDto>>> GetProperty(string id, CancellationToken ct = default)
    {
        _logger.LogInformation("Getting property with ID: {PropertyId}", id);

        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(ApiResponse<PropertyDetailDto>.ErrorResult("Property ID is required"));
        }

        var result = await _propertyService.GetPropertyByIdAsync(id, ct);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
            {
                return NotFound(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new property
    /// </summary>
    /// <param name="request">Property creation data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created property information</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PropertyDto>>> CreateProperty(
        [FromBody] CreatePropertyRequest request, 
        CancellationToken ct = default)
    {
        _logger.LogInformation("Creating new property: {@Request}", request);

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<PropertyDto>.ErrorResult("Invalid request data", errors));
        }

        var result = await _propertyService.CreatePropertyAsync(request, ct);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
            {
                return BadRequest(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return CreatedAtAction(
            nameof(GetProperty), 
            new { id = result.Data!.Id }, 
            result);
    }

    /// <summary>
    /// Update an existing property
    /// </summary>
    /// <param name="id">Property ID to update</param>
    /// <param name="request">Updated property data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated property information</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PropertyDto>>> UpdateProperty(
        string id, 
        [FromBody] UpdatePropertyRequest request, 
        CancellationToken ct = default)
    {
        _logger.LogInformation("Updating property {PropertyId}: {@Request}", id, request);

        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(ApiResponse<PropertyDto>.ErrorResult("Property ID is required"));
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<PropertyDto>.ErrorResult("Invalid request data", errors));
        }

        var result = await _propertyService.UpdatePropertyAsync(id, request, ct);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
            {
                return NotFound(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Delete a property (soft delete - sets Enabled to false)
    /// </summary>
    /// <param name="id">Property ID to delete</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProperty(string id, CancellationToken ct = default)
    {
        _logger.LogInformation("Deleting property with ID: {PropertyId}", id);

        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest(ApiResponse<bool>.ErrorResult("Property ID is required"));
        }

        var result = await _propertyService.DeletePropertyAsync(id, ct);

        if (!result.Success)
        {
            if (result.Message.Contains("not found"))
            {
                return NotFound(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return Ok(result);
    }
}