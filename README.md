# RealEstate API - Technical Test

A comprehensive Real Estate API built with .NET 9, MongoDB, and Clean Architecture principles.

## 🏗️ Architecture

This project follows Clean Architecture principles with clear separation of concerns:

```
src/
├── RealEstate.Api/           # Controllers, Middleware, Configuration
├── RealEstate.Application/   # DTOs, Services, Interfaces
├── RealEstate.Domain/        # Entities, Domain Interfaces
└── RealEstate.Infrastructure/ # Repositories, Database Configuration
tests/
└── RealEstate.UnitTests/     # Unit Tests with NUnit
```

## 🛠️ Technologies Used

- **.NET 9** - Latest .NET framework
- **MongoDB** - NoSQL database
- **MongoDB.Driver** - Official MongoDB driver for .NET
- **Clean Architecture** - Architectural pattern
- **Repository Pattern** - Data access abstraction
- **Domain Driven Design (DDD)** - Domain modeling approach
- **SOLID Principles** - Design principles
- **NUnit** - Unit testing framework
- **Moq** - Mocking framework
- **Swagger/OpenAPI** - API documentation
- **ASP.NET Core** - Web API framework

## 🚀 Features

### API Endpoints

#### Properties
- `GET /api/properties` - Get paginated list of properties with filters
- `GET /api/properties/{id}` - Get property details by ID
- `POST /api/properties` - Create new property
- `PUT /api/properties/{id}` - Update existing property
- `DELETE /api/properties/{id}` - Soft delete property

### Filtering Options
- **Name** - Search by property name (partial match)
- **Address** - Search by address (partial match)
- **Price Range** - Filter by minimum and maximum price
- **Owner ID** - Filter by specific owner
- **Year** - Filter by construction year
- **Enabled** - Filter active/inactive properties

### Domain Entities
- **Property** - Main entity with name, address, price, year, owner reference
- **Owner** - Property owner with personal information
- **PropertyImage** - Images associated with properties
- **PropertyTrace** - Historical records and transactions

## 📋 Prerequisites

- .NET 9 SDK
- MongoDB (local or remote instance)
- VS Code or preferred IDE

## 🔧 Setup Instructions

### 1. Clone the Repository
```bash
git clone <repository-url>
cd realestate
```

### 2. Install MongoDB

#### macOS (using Homebrew):
```bash
brew tap mongodb/brew
brew install mongodb-community@7.0
brew services start mongodb/brew/mongodb-community@7.0
```

#### Docker:
```bash
docker run -d -p 27017:27017 --name mongodb mongo:7.0
```

### 3. Configure Database Connection

Update `appsettings.json` and `appsettings.Development.json`:
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB"
  }
}
```

### 4. Restore Dependencies
```bash
dotnet restore
```

### 5. Run Unit Tests
```bash
dotnet test
```

### 6. Run the Application
```bash
dotnet run --project src/RealEstate.Api
```

The API will be available at:
- **Swagger UI**: http://localhost:5000/swagger (or https://localhost:7262/swagger)
- **API Base URL**: http://localhost:5000/api (or https://localhost:7262/api)

## 📖 API Documentation

### Swagger/OpenAPI
Once the application is running, visit the root URL to access the interactive Swagger documentation where you can:
- View all available endpoints
- Test API calls directly from the browser
- See request/response schemas
- Download OpenAPI specification

### Sample API Calls

#### Get Properties with Filters
```bash
GET /api/properties?name=modern&priceMin=500000&priceMax=1000000&page=1&pageSize=10
```

#### Get Property Details
```bash
GET /api/properties/{propertyId}
```

#### Create Property
```bash
POST /api/properties
Content-Type: application/json

{
  "name": "Modern Apartment",
  "address": "123 Main St, City",
  "price": 750000,
  "codeInternal": "PROP001",
  "year": 2023,
  "ownerId": "owner-id-here"
}
```

## 🧪 Testing

### Unit Tests
The project includes comprehensive unit tests using NUnit and Moq:

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run tests with coverage (requires additional tools)
dotnet test --collect:"XPlat Code Coverage"
```

### Test Coverage
Tests cover:
- ✅ PropertyService business logic
- ✅ Success scenarios
- ✅ Error handling
- ✅ Validation logic
- ✅ Repository interaction

## 📊 Sample Data

The application automatically seeds the database with sample data in development mode:
- **5 Owners** with realistic information
- **8 Properties** with various types and price ranges
- **Property Images** from Unsplash
- **Property Traces** with historical records

## 🏛️ Architecture Principles

### Clean Architecture
- **Domain Layer**: Core business entities and interfaces
- **Application Layer**: Business logic and DTOs
- **Infrastructure Layer**: Data access and external services
- **API Layer**: Controllers and HTTP concerns

### SOLID Principles
- **Single Responsibility**: Each class has one reason to change
- **Open/Closed**: Open for extension, closed for modification
- **Liskov Substitution**: Derived classes are substitutable
- **Interface Segregation**: Clients depend only on needed interfaces
- **Dependency Inversion**: Depend on abstractions, not concretions

### Repository Pattern
- Abstracts data access logic
- Enables easy testing with mocks
- Provides consistent interface for data operations

### Domain Driven Design
- Rich domain models
- Ubiquitous language
- Clear bounded contexts

## 🚨 Error Handling

The API implements comprehensive error handling:
- **Global Exception Middleware** for unhandled exceptions
- **Validation Errors** with detailed messages
- **Structured Error Responses** with consistent format
- **HTTP Status Codes** following REST conventions

## 🔒 Security Considerations

- **Input Validation** on all endpoints
- **CORS Configuration** for frontend integration
- **MongoDB Injection Prevention** through parameterized queries
- **Soft Deletes** for data preservation

## 🚀 Performance Optimizations

- **Pagination** for large datasets
- **Indexed MongoDB Queries** for fast searches
- **Async/Await** patterns throughout
- **Efficient Data Loading** with projection
- **Caching-Ready** architecture

## 📈 Monitoring & Health Checks

- Health check endpoint: `GET /health`
- Comprehensive logging throughout the application
- Structured error responses for monitoring

## 🔄 Future Enhancements

Potential improvements for production deployment:
- Authentication and authorization (JWT, OAuth)
- Rate limiting and throttling
- Advanced caching (Redis)
- File upload for property images
- Real-time notifications (SignalR)
- Advanced search with Elasticsearch
- Audit logging
- Performance monitoring
- Docker containerization
- CI/CD pipeline

## 📞 Support

For questions or issues, please contact the development team or create an issue in the repository.

---

**Built with ❤️ for real estate management**