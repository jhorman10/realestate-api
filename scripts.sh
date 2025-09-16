#!/bin/bash

# RealEstate API Development Scripts
# Run this script with: chmod +x scripts.sh && ./scripts.sh [command]

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${BLUE}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[SUCCESS]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[WARNING]${NC} $1"
}

print_error() {
    echo -e "${RED}[ERROR]${NC} $1"
}

# Check if MongoDB is running
check_mongodb() {
    print_status "Checking MongoDB connection..."
    if mongosh --eval "db.runCommand('ping').ok" --quiet > /dev/null 2>&1; then
        print_success "MongoDB is running"
        return 0
    else
        print_error "MongoDB is not running. Please start MongoDB first."
        print_status "To start MongoDB:"
        echo "  brew services start mongodb/brew/mongodb-community@7.0"
        echo "  OR"
        echo "  docker run -d -p 27017:27017 --name mongodb mongo:7.0"
        return 1
    fi
}

# Start MongoDB (if not running)
start_mongodb() {
    if ! check_mongodb; then
        print_status "Starting MongoDB with brew..."
        brew services start mongodb/brew/mongodb-community@7.0
        sleep 3
        check_mongodb
    fi
}

# Build the solution
build() {
    print_status "Building the solution..."
    dotnet build
    print_success "Build completed successfully"
}

# Run tests
test() {
    print_status "Running tests..."
    dotnet test --verbosity normal
    print_success "Tests completed"
}

# Restore dependencies
restore() {
    print_status "Restoring NuGet packages..."
    dotnet restore
    print_success "Packages restored successfully"
}

# Run the API
run() {
    print_status "Starting RealEstate API..."
    check_mongodb || exit 1
    print_status "Starting API on https://localhost:7165"
    print_status "Swagger UI available at: https://localhost:7165/swagger"
    dotnet run --project src/RealEstate.Api
}

# Run with hot reload
dev() {
    print_status "Starting RealEstate API in development mode with hot reload..."
    check_mongodb || exit 1
    print_status "Starting API on https://localhost:7165"
    print_status "Swagger UI available at: https://localhost:7165/swagger"
    print_status "Hot reload enabled - changes will be automatically reloaded"
    dotnet watch --project src/RealEstate.Api
}

# Clean build artifacts
clean() {
    print_status "Cleaning build artifacts..."
    dotnet clean
    find . -name "bin" -type d -exec rm -rf {} + 2>/dev/null || true
    find . -name "obj" -type d -exec rm -rf {} + 2>/dev/null || true
    print_success "Clean completed"
}

# Format code
format() {
    print_status "Formatting code..."
    dotnet format
    print_success "Code formatting completed"
}

# Run all quality checks
check() {
    print_status "Running all quality checks..."
    restore
    build
    format
    test
    print_success "All quality checks passed"
}

# Setup development environment
setup() {
    print_status "Setting up development environment..."
    
    # Check if .NET 9 is installed
    if ! dotnet --version | grep -q "^9\."; then
        print_error ".NET 9 SDK not found. Please install .NET 9 SDK"
        exit 1
    fi
    
    print_success ".NET 9 SDK found"
    
    # Check MongoDB
    if command -v mongosh >/dev/null 2>&1; then
        print_success "MongoDB Shell found"
        start_mongodb
    elif command -v mongo >/dev/null 2>&1; then
        print_success "MongoDB found"
        start_mongodb
    else
        print_warning "MongoDB not found. Please install MongoDB:"
        echo "  brew tap mongodb/brew"
        echo "  brew install mongodb-community@7.0"
    fi
    
    restore
    build
    test
    
    print_success "Development environment setup completed!"
    print_status "Use './scripts.sh run' to start the API"
    print_status "Use './scripts.sh dev' to start with hot reload"
}

# Show help
help() {
    echo "RealEstate API Development Scripts"
    echo ""
    echo "Usage: ./scripts.sh [command]"
    echo ""
    echo "Commands:"
    echo "  setup      - Setup development environment"
    echo "  build      - Build the solution"
    echo "  test       - Run all tests"
    echo "  run        - Start the API"
    echo "  dev        - Start the API with hot reload"
    echo "  restore    - Restore NuGet packages"
    echo "  clean      - Clean build artifacts"
    echo "  format     - Format code"
    echo "  check      - Run all quality checks (restore, build, format, test)"
    echo "  mongodb    - Check MongoDB status"
    echo "  help       - Show this help message"
    echo ""
    echo "Examples:"
    echo "  ./scripts.sh setup     # First time setup"
    echo "  ./scripts.sh dev       # Start development server"
    echo "  ./scripts.sh test      # Run tests"
    echo "  ./scripts.sh check     # Run all quality checks"
}

# Main script logic
case "${1:-help}" in
    setup)
        setup
        ;;
    build)
        build
        ;;
    test)
        test
        ;;
    run)
        run
        ;;
    dev)
        dev
        ;;
    restore)
        restore
        ;;
    clean)
        clean
        ;;
    format)
        format
        ;;
    check)
        check
        ;;
    mongodb)
        check_mongodb
        ;;
    help|--help|-h)
        help
        ;;
    *)
        print_error "Unknown command: $1"
        echo ""
        help
        exit 1
        ;;
esac