using Moq;
using NUnit.Framework;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.UnitTests.Services;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _propertyRepositoryMock;
    private Mock<IOwnerRepository> _ownerRepositoryMock;
    private Mock<IPropertyTraceRepository> _traceRepositoryMock;
    private PropertyService _propertyService;

    [SetUp]
    public void Setup()
    {
        _propertyRepositoryMock = new Mock<IPropertyRepository>();
        _ownerRepositoryMock = new Mock<IOwnerRepository>();
        _traceRepositoryMock = new Mock<IPropertyTraceRepository>();
        
        _propertyService = new PropertyService(
            _propertyRepositoryMock.Object,
            _ownerRepositoryMock.Object,
            _traceRepositoryMock.Object);
    }

    [Test]
    public async Task GetPropertiesAsync_WithValidFilter_ReturnsPagedResult()
    {
        // Arrange
        var filter = new PropertyFilterRequest
        {
            Name = "Test Property",
            Page = 1,
            PageSize = 10
        };

        var properties = new List<Property>
        {
            new Property
            {
                Id = "1",
                Name = "Test Property 1",
                Address = "Test Address 1",
                Price = 100000,
                CodeInternal = "PROP001",
                Year = 2023,
                OwnerId = "owner1",
                Enabled = true
            }
        };

        _propertyRepositoryMock
            .Setup(x => x.GetPropertiesAsync(It.IsAny<PropertyFilter>(), filter.Page, filter.PageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync((properties, 1L));

        // Act
        var result = await _propertyService.GetPropertiesAsync(filter);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Items.Count(), Is.EqualTo(1));
        Assert.That(result.Data.Total, Is.EqualTo(1));
        Assert.That(result.Data.Page, Is.EqualTo(1));
        Assert.That(result.Data.PageSize, Is.EqualTo(10));
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithValidId_ReturnsProperty()
    {
        // Arrange
        var propertyId = "test-id";
        var property = new Property
        {
            Id = propertyId,
            Name = "Test Property",
            Address = "Test Address",
            Price = 100000,
            CodeInternal = "PROP001",
            Year = 2023,
            OwnerId = "owner1",
            Enabled = true
        };

        var traces = new List<PropertyTrace>
        {
            new PropertyTrace
            {
                Id = "trace1",
                PropertyId = propertyId,
                DateSale = DateTime.UtcNow,
                Name = "Sale",
                Value = 100000,
                Tax = 5000
            }
        };

        _propertyRepositoryMock
            .Setup(x => x.GetPropertyByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(property);

        _traceRepositoryMock
            .Setup(x => x.GetTracesByPropertyIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(traces);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Id, Is.EqualTo(propertyId));
        Assert.That(result.Data.Traces.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithInvalidId_ReturnsError()
    {
        // Arrange
        var propertyId = "invalid-id";

        _propertyRepositoryMock
            .Setup(x => x.GetPropertyByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("not found"));
    }

    [Test]
    public async Task CreatePropertyAsync_WithValidRequest_ReturnsCreatedProperty()
    {
        // Arrange
        var request = new CreatePropertyRequest
        {
            Name = "New Property",
            Address = "New Address",
            Price = 150000,
            CodeInternal = "PROP002",
            Year = 2024,
            OwnerId = "owner1"
        };

        _ownerRepositoryMock
            .Setup(x => x.OwnerExistsAsync(request.OwnerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _propertyRepositoryMock
            .Setup(x => x.CreatePropertyAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property property, CancellationToken ct) => property);

        // Act
        var result = await _propertyService.CreatePropertyAsync(request);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Name, Is.EqualTo(request.Name));
        Assert.That(result.Data.Address, Is.EqualTo(request.Address));
        Assert.That(result.Data.Price, Is.EqualTo(request.Price));
    }

    [Test]
    public async Task CreatePropertyAsync_WithInvalidOwner_ReturnsError()
    {
        // Arrange
        var request = new CreatePropertyRequest
        {
            Name = "New Property",
            Address = "New Address",
            Price = 150000,
            CodeInternal = "PROP002",
            Year = 2024,
            OwnerId = "invalid-owner"
        };

        _ownerRepositoryMock
            .Setup(x => x.OwnerExistsAsync(request.OwnerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _propertyService.CreatePropertyAsync(request);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("Owner not found"));
    }

    [Test]
    public async Task UpdatePropertyAsync_WithValidRequest_ReturnsUpdatedProperty()
    {
        // Arrange
        var propertyId = "test-id";
        var existingProperty = new Property
        {
            Id = propertyId,
            Name = "Old Name",
            Address = "Old Address",
            Price = 100000,
            CodeInternal = "PROP001",
            Year = 2023,
            OwnerId = "owner1",
            Enabled = true
        };

        var request = new UpdatePropertyRequest
        {
            Name = "Updated Name",
            Address = "Updated Address",
            Price = 120000,
            CodeInternal = "PROP001-UPD",
            Year = 2024,
            OwnerId = "owner1",
            Enabled = true
        };

        _propertyRepositoryMock
            .Setup(x => x.GetPropertyByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProperty);

        _ownerRepositoryMock
            .Setup(x => x.OwnerExistsAsync(request.OwnerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _propertyRepositoryMock
            .Setup(x => x.UpdatePropertyAsync(It.IsAny<Property>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property property, CancellationToken ct) => property);

        // Act
        var result = await _propertyService.UpdatePropertyAsync(propertyId, request);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.Not.Null);
        Assert.That(result.Data.Name, Is.EqualTo(request.Name));
        Assert.That(result.Data.Address, Is.EqualTo(request.Address));
        Assert.That(result.Data.Price, Is.EqualTo(request.Price));
    }

    [Test]
    public async Task UpdatePropertyAsync_WithInvalidPropertyId_ReturnsError()
    {
        // Arrange
        var propertyId = "invalid-id";
        var request = new UpdatePropertyRequest
        {
            Name = "Updated Name",
            Address = "Updated Address",
            Price = 120000,
            CodeInternal = "PROP001-UPD",
            Year = 2024,
            OwnerId = "owner1",
            Enabled = true
        };

        _propertyRepositoryMock
            .Setup(x => x.GetPropertyByIdAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await _propertyService.UpdatePropertyAsync(propertyId, request);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("Property not found"));
    }

    [Test]
    public async Task DeletePropertyAsync_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var propertyId = "test-id";

        _propertyRepositoryMock
            .Setup(x => x.PropertyExistsAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _propertyRepositoryMock
            .Setup(x => x.DeletePropertyAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _propertyService.DeletePropertyAsync(propertyId);

        // Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Data, Is.True);
    }

    [Test]
    public async Task DeletePropertyAsync_WithInvalidId_ReturnsError()
    {
        // Arrange
        var propertyId = "invalid-id";

        _propertyRepositoryMock
            .Setup(x => x.PropertyExistsAsync(propertyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _propertyService.DeletePropertyAsync(propertyId);

        // Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Does.Contain("Property not found"));
    }
}