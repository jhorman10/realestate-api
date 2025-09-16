#!/bin/bash

# RealEstate Project Cleanup Script
echo "🧹 Cleaning RealEstate project..."

# Clean .NET build artifacts
echo "📦 Cleaning .NET build artifacts..."
dotnet clean

# Remove bin and obj folders
echo "🗂️  Removing bin and obj folders..."
find . -name "bin" -type d -exec rm -rf {} + 2>/dev/null || true
find . -name "obj" -type d -exec rm -rf {} + 2>/dev/null || true

# Remove log files
echo "📝 Removing log files..."
find . -name "*.log" -delete 2>/dev/null || true

# Remove VS Code settings if present
echo "🔧 Removing VS Code settings..."
rm -rf .vscode 2>/dev/null || true

# Remove temporary files
echo "🗑️  Removing temporary files..."
find . -name "*.tmp" -delete 2>/dev/null || true
find . -name "*~" -delete 2>/dev/null || true

# Remove macOS specific files
echo "🍎 Removing macOS specific files..."
find . -name ".DS_Store" -delete 2>/dev/null || true

echo "✅ Project cleanup completed!"
echo "📁 Clean project structure:"
tree -I 'bin|obj|node_modules|.git' --dirsfirst -L 3 . 2>/dev/null || ls -la