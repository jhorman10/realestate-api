# RealEstate API - Technical Documentation

## 📋 Project Overview

This document provides comprehensive technical information about the RealEstate API, a full-stack backend application built for property management following enterprise-level standards.

## 🎯 Requirements Fulfilled

### ✅ Backend Requirements
- [x] **.NET 9** API with C#
- [x] **MongoDB** as database
- [x] **Property filters** by name, address, and price range
- [x] **DTOs** with IdOwner, Name, Address, Price, and Image fields
- [x] **Clean Architecture** implementation
- [x] **Repository Pattern** with dependency injection
- [x] **Domain Driven Design** principles
- [x] **SOLID Principles** throughout codebase

### ✅ Technical Implementation
- [x] **NUnit** unit tests with mocking
- [x] **Clean Code** practices and conventions
- [x] **Error Handling** with global middleware
- [x] **Performance Optimization** with pagination and async operations
- [x] **Swagger Documentation** with OpenAPI specification
- [x] **Best Practices** in API design and architecture

## 🏗️ Detailed Architecture

### Clean Architecture Layers

```
┌─────────────────────────────────────────────────────────────┐
│                    RealEstate.Api                           │
│  Controllers │ Middleware │ Configuration │ Startup         │
└─────────────────────────┬───────────────────────────────────┘
                          │ Depends on
┌─────────────────────────▼───────────────────────────────────┐
│                RealEstate.Application                       │
│     DTOs │ Services │ Interfaces │ Business Logic           │
└─────────────────────────┬───────────────────────────────────┘
                          │ Depends on
┌─────────────────────────▼───────────────────────────────────┐
│                 RealEstate.Domain                           │
│       Entities │ Domain Interfaces │ Domain Logic           │
└─────────────────────────▲───────────────────────────────────┘
                          │ Implements
┌─────────────────────────┴───────────────────────────────────┐
│              RealEstate.Infrastructure                      │
│    Repositories │ MongoDB Configuration │ External Services │
└─────────────────────────────────────────────────────────────┘
```

### Dependency Flow
- **API** → **Application** → **Domain** ← **Infrastructure**
- Dependencies point inward (Dependency Inversion Principle)
- Infrastructure implements Domain interfaces
- Application uses Domain abstractions

## 📊 Database Schema (MongoDB)

### Collections Structure

#### `properties`
```json
{
  "_id": "ObjectId",
  "name": "string",
  "address": "string", 
  "price": "decimal",
  "codeInternal": "string",
  "year": "int",
  "ownerId": "ObjectId",
  "enabled": "bool",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

#### `owners`
```json
{
  "_id": "ObjectId",
  "name": "string",
  "address": "string",
  "photo": "string",
  "birthday": "DateTime",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

#### `property_images`
```json
{
  "_id": "ObjectId",
  "propertyId": "ObjectId",
  "file": "string",
  "enabled": "bool",
  "createdAt": "DateTime"
}
```

#### `property_traces`
```json
{
  "_id": "ObjectId",
  "propertyId": "ObjectId", 
  "dateSale": "DateTime",
  "name": "string",
  "value": "decimal",
  "tax": "decimal",
  "createdAt": "DateTime"
}
```

## 🚀 API Endpoints Documentation

### Base URL
- **Development**: `https://localhost:5001/api`
- **Swagger UI**: `https://localhost:5001`

### Authentication
- Currently no authentication required
- Ready for JWT/OAuth implementation

---

## 📖 Properties API

### 1. GET /api/properties
**Purpose**: Retrieve paginated list of properties with filtering

**Query Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `name` | string | No | Property name (partial match) |
| `address` | string | No | Property address (partial match) |
| `priceMin` | decimal | No | Minimum price filter |
| `priceMax` | decimal | No | Maximum price filter |
| `ownerId` | string | No | Filter by owner ID |
| `year` | int | No | Construction year |
| `enabled` | bool | No | Active/inactive filter (default: true) |
| `page` | int | No | Page number (default: 1) |
| `pageSize` | int | No | Items per page (default: 10, max: 100) |

**Response**:
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "items": [
      {
        "id": "string",
        "name": "string",
        "address": "string",
        "price": 0,
        "codeInternal": "string",
        "year": 0,
        "ownerId": "string",
        "enabled": true,
        "createdAt": "2023-01-01T00:00:00Z",
        "updatedAt": "2023-01-01T00:00:00Z",
        "owner": {
          "id": "string",
          "name": "string",
          "address": "string",
          "photo": "string",
          "birthday": "1980-01-01T00:00:00Z"
        },
        "imageUrl": "string",
        "images": [
          {
            "id": "string",
            "file": "string",
            "enabled": true
          }
        ]
      }
    ],
    "total": 100,
    "page": 1,
    "pageSize": 10,
    "totalPages": 10,
    "hasPreviousPage": false,
    "hasNextPage": true
  },
  "errors": []
}
```

**Example Request**:
```bash
GET /api/properties?name=modern&priceMin=500000&priceMax=1000000&page=1&pageSize=5
```

---

### 2. GET /api/properties/{id}
**Purpose**: Get detailed information about a specific property

**Path Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | string | Yes | Property ID |

**Response**:
```json
{
  "success": true,
  "message": "Success",
  "data": {
    "id": "string",
    "name": "string",
    "address": "string",
    "price": 0,
    "codeInternal": "string",
    "year": 0,
    "ownerId": "string",
    "enabled": true,
    "createdAt": "2023-01-01T00:00:00Z",
    "updatedAt": "2023-01-01T00:00:00Z",
    "owner": {
      "id": "string",
      "name": "string",
      "address": "string",
      "photo": "string",
      "birthday": "1980-01-01T00:00:00Z"
    },
    "imageUrl": "string",
    "images": [
      {
        "id": "string",
        "file": "string",
        "enabled": true
      }
    ],
    "traces": [
      {
        "id": "string",
        "dateSale": "2023-01-01T00:00:00Z",
        "name": "string",
        "value": 0,
        "tax": 0
      }
    ]
  },
  "errors": []
}
```

---

### 3. POST /api/properties
**Purpose**: Create a new property

**Request Body**:
```json
{
  "name": "string", // Required, 1-200 chars
  "address": "string", // Required, 1-500 chars
  "price": 0, // Required, > 0
  "codeInternal": "string", // Required, 1-50 chars
  "year": 0, // Required, 1800-2100
  "ownerId": "string" // Required, valid Owner ID
}
```

**Response**: Same as GET by ID

**Validation Rules**:
- Name: Required, 1-200 characters
- Address: Required, 1-500 characters  
- Price: Required, must be greater than 0
- CodeInternal: Required, 1-50 characters
- Year: Required, between 1800-2100
- OwnerId: Required, must exist in database

---

### 4. PUT /api/properties/{id}
**Purpose**: Update an existing property

**Path Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | string | Yes | Property ID to update |

**Request Body**:
```json
{
  "name": "string", // Required, 1-200 chars
  "address": "string", // Required, 1-500 chars
  "price": 0, // Required, > 0
  "codeInternal": "string", // Required, 1-50 chars
  "year": 0, // Required, 1800-2100
  "ownerId": "string", // Required, valid Owner ID
  "enabled": true // Optional, default true
}
```

**Response**: Same as GET by ID

---

### 5. DELETE /api/properties/{id}
**Purpose**: Soft delete a property (sets enabled = false)

**Path Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | string | Yes | Property ID to delete |

**Response**:
```json
{
  "success": true,
  "message": "Property deleted successfully",
  "data": true,
  "errors": []
}
```

---

## 🛠️ Technical Implementation Details

### Repository Pattern Implementation

```csharp
// Domain Interface (Abstraction)
public interface IPropertyRepository
{
    Task<(IEnumerable<Property> Items, long Total)> GetPropertiesAsync(
        PropertyFilter filter, int page, int pageSize, CancellationToken ct = default);
    Task<Property?> GetPropertyByIdAsync(string id, CancellationToken ct = default);
    Task<Property> CreatePropertyAsync(Property property, CancellationToken ct = default);
    Task<Property> UpdatePropertyAsync(Property property, CancellationToken ct = default);
    Task<bool> DeletePropertyAsync(string id, CancellationToken ct = default);
    Task<bool> PropertyExistsAsync(string id, CancellationToken ct = default);
}

// Infrastructure Implementation
public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
{
    // MongoDB specific implementation
}
```

### Service Layer Pattern

```csharp
public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    
    // Business logic implementation with proper error handling
    public async Task<ApiResponse<PropertyDto>> CreatePropertyAsync(
        CreatePropertyRequest request, CancellationToken ct = default)
    {
        // Validation
        // Business rules
        // Repository calls
        // DTO mapping
        // Error handling
    }
}
```

### Dependency Injection Configuration

```csharp
// MongoDB Configuration
builder.Services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
builder.Services.AddSingleton<IMongoDatabase>(sp => client.GetDatabase(databaseName));

// Repository Registration
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();

// Service Registration
builder.Services.AddScoped<IPropertyService, PropertyService>();
```

## 🧪 Testing Strategy

### Unit Tests Structure
```
tests/RealEstate.UnitTests/
├── PropertyServiceTests.cs          # Service layer tests
├── Controllers/                     # Controller tests (future)
└── Repositories/                    # Repository tests (future)
```

### Test Coverage
- ✅ **PropertyService**: 9 comprehensive tests
- ✅ **Success scenarios**: Happy path testing
- ✅ **Error scenarios**: Validation and business rule violations
- ✅ **Mocking**: All dependencies properly mocked
- ✅ **Async testing**: Proper async/await testing patterns

### Sample Test Implementation
```csharp
[Test]
public async Task GetPropertiesAsync_WithValidFilter_ReturnsPagedResult()
{
    // Arrange
    var filter = new PropertyFilterRequest { Name = "Test", Page = 1, PageSize = 10 };
    var expectedProperties = new List<Property> { /* test data */ };
    
    _repositoryMock.Setup(x => x.GetPropertiesAsync(It.IsAny<PropertyFilter>(), 1, 10, It.IsAny<CancellationToken>()))
                   .ReturnsAsync((expectedProperties, 1L));
    
    // Act
    var result = await _propertyService.GetPropertiesAsync(filter);
    
    // Assert
    Assert.That(result.Success, Is.True);
    Assert.That(result.Data.Items.Count(), Is.EqualTo(1));
}
```

## 🔧 Configuration & Setup

### Environment Configuration
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### CORS Configuration
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

## 📊 Performance Considerations

### Database Optimization
- **Indexes**: Automatic ObjectId indexing
- **Pagination**: Efficient skip/limit queries
- **Projection**: Only load required fields
- **Async Operations**: Non-blocking database calls

### Memory Management
- **Scoped Services**: Proper DI lifetime management
- **Async/Await**: Efficient thread utilization
- **Disposing**: Proper resource cleanup

### Query Optimization
```csharp
// Efficient filtering with MongoDB driver
var filterBuilder = Builders<Property>.Filter;
var filters = new List<FilterDefinition<Property>>();

if (!string.IsNullOrWhiteSpace(filter.Name))
    filters.Add(filterBuilder.Regex(p => p.Name, new BsonRegularExpression(filter.Name, "i")));

var combinedFilter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;
```

## 🚨 Error Handling Strategy

### Global Exception Middleware
```csharp
public class GlobalExceptionMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
}
```

### Structured Error Responses
```json
{
  "success": false,
  "message": "Error description",
  "data": null,
  "errors": ["Detailed error 1", "Detailed error 2"]
}
```

### HTTP Status Code Mapping
- **200 OK**: Successful operations
- **201 Created**: Resource creation
- **400 Bad Request**: Validation errors
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Unhandled exceptions

## 📈 Monitoring & Logging

### Health Check Endpoint
```
GET /health
Response: {"status": "Healthy", "timestamp": "2023-01-01T00:00:00Z"}
```

### Logging Strategy
- **Information**: Successful operations
- **Warning**: Recoverable issues
- **Error**: Exceptions and failures
- **Debug**: Development debugging (dev environment only)

## 🔒 Security Implementation

### Input Validation
- Data Annotations on DTOs
- Model State validation in controllers
- Business rule validation in services

### Database Security
- Parameterized queries (MongoDB driver handles this)
- No SQL injection vulnerabilities
- Soft deletes for data preservation

### CORS Protection
- Configured for specific frontend origins
- Credential support for authenticated requests
- Proper HTTP methods allowed

## 🚀 Deployment Considerations

### Environment Requirements
- .NET 9 Runtime
- MongoDB 5.0+ (preferably 7.0)
- 512MB+ RAM
- Modern CPU with async support

### Configuration Management
- Environment-specific appsettings files
- Secure connection string storage
- Logging level configuration per environment

### Scalability Options
- MongoDB replica sets for high availability
- Connection pooling (built into MongoDB driver)
- Horizontal scaling with load balancers
- Caching layer integration ready

---

## 📞 API Support Information

### Status Codes Reference
| Code | Description | When Used |
|------|-------------|-----------|
| 200 | OK | Successful GET, PUT, DELETE |
| 201 | Created | Successful POST |
| 400 | Bad Request | Validation errors, invalid data |
| 404 | Not Found | Resource doesn't exist |
| 500 | Internal Server Error | Unhandled server errors |

### Rate Limiting
- Currently not implemented
- Ready for implementation with middleware
- Recommended for production: 100 requests/minute per IP

### Content Types
- **Request**: `application/json`
- **Response**: `application/json`
- **Character Encoding**: UTF-8

---

This technical documentation provides comprehensive information for developers, testers, and DevOps teams to understand, maintain, and extend the RealEstate API effectively.