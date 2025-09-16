#!/bin/bash

# Test script for RealEstate API
echo "=== TESTING REALESTATE API ==="
echo "API URL: http://localhost:5126"
echo ""

# Wait for API to be ready
echo "Waiting for API to be ready..."
sleep 3

# Test 1: Get all properties
echo "1. Testing GET /api/Properties"
echo "=================================="
curl -s -X GET "http://localhost:5126/api/Properties" \
  -H "accept: application/json" | python3 -m json.tool

echo ""
echo ""

# Test 2: Get properties with filters
echo "2. Testing GET /api/Properties with filters (minPrice=100000)"
echo "=============================================================="
curl -s -X GET "http://localhost:5126/api/Properties?minPrice=100000" \
  -H "accept: application/json" | python3 -m json.tool

echo ""
echo ""

# Test 3: Create a new property
echo "3. Testing POST /api/Properties (Create new property)"
echo "====================================================="
curl -s -X POST "http://localhost:5126/api/Properties" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Property via API",
    "address": "123 Test Street, Test City",
    "price": 250000,
    "codeInternal": "TEST001",
    "year": 2023,
    "ownerName": "Test Owner"
  }' | python3 -m json.tool

echo ""
echo ""

# Test 4: Health check (if exists)
echo "4. Testing API Health"
echo "===================="
curl -s -X GET "http://localhost:5126/health" \
  -H "accept: application/json" || echo "Health endpoint not available"

echo ""
echo ""
echo "=== API TESTING COMPLETED ==="