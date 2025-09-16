# Contributing to RealEstate API

Thank you for your interest in contributing to the RealEstate API project! This document provides guidelines and information for contributors.

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- MongoDB 5.0+ (preferably 7.0)
- VS Code or Visual Studio
- Git

### Setup Development Environment

1. **Clone the repository**
   ```bash
   git clone https://github.com/username/realestate-api.git
   cd realestate-api
   ```

2. **Install MongoDB**
   ```bash
   # macOS
   brew tap mongodb/brew
   brew install mongodb-community@7.0
   brew services start mongodb/brew/mongodb-community@7.0
   
   # Or using Docker
   docker run -d -p 27017:27017 --name mongodb mongo:7.0
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Run tests**
   ```bash
   dotnet test
   ```

5. **Start the application**
   ```bash
   dotnet run --project src/RealEstate.Api
   ```

## 🏗️ Architecture Guidelines

### Clean Architecture Principles
- Keep dependencies pointing inward
- Domain layer should have no external dependencies
- Use interfaces for abstraction
- Implement Repository pattern for data access

### Project Structure
```
src/
├── RealEstate.Api/           # Controllers, Middleware
├── RealEstate.Application/   # Services, DTOs, Interfaces
├── RealEstate.Domain/        # Entities, Domain Interfaces
└── RealEstate.Infrastructure/ # Repositories, Database Config
tests/
└── RealEstate.UnitTests/     # Unit Tests
```

## 📝 Coding Standards

### C# Conventions
- Use PascalCase for public members
- Use camelCase for private fields and parameters
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Follow Microsoft's C# coding conventions

### Example:
```csharp
/// <summary>
/// Creates a new property in the system
/// </summary>
/// <param name="request">Property creation data</param>
/// <param name="ct">Cancellation token</param>
/// <returns>Created property information</returns>
public async Task<ApiResponse<PropertyDto>> CreatePropertyAsync(
    CreatePropertyRequest request, 
    CancellationToken ct = default)
{
    // Implementation
}
```

## 🧪 Testing Guidelines

### Unit Tests
- Write tests for all public methods
- Use descriptive test method names
- Follow Arrange-Act-Assert pattern
- Mock external dependencies with Moq
- Aim for high code coverage

### Test Naming Convention
```csharp
[Test]
public async Task MethodName_Scenario_ExpectedResult()
{
    // Arrange
    var input = new SampleInput();
    
    // Act
    var result = await _service.MethodName(input);
    
    // Assert
    Assert.That(result.Success, Is.True);
}
```

## 🔄 Development Workflow

### Branching Strategy
- `main` - Production ready code
- `develop` - Integration branch
- `feature/feature-name` - New features
- `bugfix/issue-description` - Bug fixes
- `hotfix/critical-fix` - Critical production fixes

### Commit Messages
Use conventional commit format:
```
type(scope): description

feat(api): add property filtering by owner
fix(repository): handle null reference in property query
docs(readme): update setup instructions
test(service): add property service validation tests
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`

## 📋 Pull Request Process

### Before Creating a PR
1. **Update from main**
   ```bash
   git checkout main
   git pull origin main
   git checkout your-feature-branch
   git rebase main
   ```

2. **Run all tests**
   ```bash
   dotnet test
   ```

3. **Build successfully**
   ```bash
   dotnet build
   ```

4. **Check code formatting**
   ```bash
   dotnet format
   ```

### PR Requirements
- [ ] All tests pass
- [ ] Code builds without warnings
- [ ] New features have corresponding tests
- [ ] Documentation updated if needed
- [ ] Follow coding standards
- [ ] Descriptive PR title and description

### PR Template
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests added/updated
- [ ] Manual testing completed
- [ ] All tests pass

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
- [ ] No breaking changes (or documented)
```

## 🐛 Reporting Issues

### Bug Reports
When reporting bugs, include:
- Clear description of the issue
- Steps to reproduce
- Expected vs actual behavior
- Environment details (.NET version, OS, etc.)
- Error logs or stack traces

### Feature Requests
For new features, provide:
- Use case description
- Proposed solution
- Alternative solutions considered
- Additional context

## 📚 Documentation

### API Documentation
- Update Swagger/OpenAPI documentation
- Include request/response examples
- Document error responses
- Add parameter descriptions

### Code Documentation
- XML documentation for public APIs
- Inline comments for complex logic
- README updates for new features
- Architecture diagrams when needed

## 🔒 Security Considerations

### Security Guidelines
- Validate all inputs
- Use parameterized queries
- Implement proper error handling
- Don't expose sensitive information
- Follow OWASP guidelines

### Reporting Security Issues
For security vulnerabilities:
1. **DO NOT** create public issues
2. Email: security@yourcompany.com
3. Include detailed description
4. Provide steps to reproduce

## 🎯 Performance Guidelines

### Best Practices
- Use async/await for I/O operations
- Implement proper pagination
- Optimize database queries
- Use appropriate caching strategies
- Monitor memory usage

### Performance Testing
- Profile critical paths
- Load test API endpoints
- Monitor database performance
- Use performance benchmarks

## 🤝 Code Review Guidelines

### As a Reviewer
- Be constructive and respectful
- Focus on code quality and standards
- Suggest improvements
- Approve when ready
- Test locally when needed

### As an Author
- Respond to feedback promptly
- Make requested changes
- Explain design decisions
- Keep PR scope focused
- Update based on reviews

## 📞 Getting Help

### Communication Channels
- GitHub Issues - Bug reports and feature requests
- GitHub Discussions - General questions and ideas
- Email - Direct communication for sensitive topics

### Resources
- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [MongoDB Documentation](https://docs.mongodb.com/)
- [Clean Architecture Guide](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [API Design Guidelines](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design)

## 🙏 Recognition

Contributors will be recognized in:
- CONTRIBUTORS.md file
- Release notes
- Project documentation

Thank you for contributing to RealEstate API! 🏡