using MongoDB.Driver;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Seeders;

public class DatabaseSeeder
{
    private readonly IMongoDatabase _database;

    public DatabaseSeeder(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task SeedAsync()
    {
        await SeedOwnersAsync();
        await SeedPropertiesAsync();
        await SeedPropertyImagesAsync();
        await SeedPropertyTracesAsync();
    }

    private async Task SeedOwnersAsync()
    {
        var collection = _database.GetCollection<Owner>("owners");
        
        var count = await collection.CountDocumentsAsync(_ => true);
        if (count > 0) return;

        var owners = new List<Owner>
        {
            new Owner
            {
                Name = "John Smith",
                Address = "123 Main St, New York, NY",
                Photo = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150&h=150&fit=crop&crop=face",
                Birthday = new DateTime(1980, 5, 15)
            },
            new Owner
            {
                Name = "Maria Garcia",
                Address = "456 Oak Ave, Los Angeles, CA",
                Photo = "https://images.unsplash.com/photo-1494790108755-2616b612b786?w=150&h=150&fit=crop&crop=face",
                Birthday = new DateTime(1975, 8, 22)
            },
            new Owner
            {
                Name = "Robert Johnson",
                Address = "789 Pine St, Chicago, IL",
                Photo = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&h=150&fit=crop&crop=face",
                Birthday = new DateTime(1968, 12, 3)
            },
            new Owner
            {
                Name = "Emily Davis",
                Address = "321 Elm Dr, Houston, TX",
                Photo = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=150&h=150&fit=crop&crop=face",
                Birthday = new DateTime(1985, 3, 28)
            },
            new Owner
            {
                Name = "Michael Brown",
                Address = "654 Cedar Ln, Phoenix, AZ",
                Photo = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&h=150&fit=crop&crop=face",
                Birthday = new DateTime(1972, 7, 11)
            }
        };

        await collection.InsertManyAsync(owners);
    }

    private async Task SeedPropertiesAsync()
    {
        var propertiesCollection = _database.GetCollection<Property>("properties");
        var ownersCollection = _database.GetCollection<Owner>("owners");
        
        var count = await propertiesCollection.CountDocumentsAsync(_ => true);
        if (count > 0) return;

        var owners = await ownersCollection.Find(_ => true).ToListAsync();
        if (!owners.Any()) return;

        var properties = new List<Property>
        {
            new Property
            {
                Name = "Modern Downtown Apartment",
                Address = "555 Broadway, New York, NY 10012",
                Price = 850000,
                CodeInternal = "NYC001",
                Year = 2020,
                OwnerId = owners[0].Id,
                Enabled = true
            },
            new Property
            {
                Name = "Luxury Beachfront Villa",
                Address = "1234 Ocean Drive, Miami, FL 33139",
                Price = 2500000,
                CodeInternal = "MIA001",
                Year = 2019,
                OwnerId = owners[1].Id,
                Enabled = true
            },
            new Property
            {
                Name = "Cozy Suburban House",
                Address = "789 Maple Street, Austin, TX 78701",
                Price = 450000,
                CodeInternal = "AUS001",
                Year = 2018,
                OwnerId = owners[2].Id,
                Enabled = true
            },
            new Property
            {
                Name = "Historic Brownstone",
                Address = "101 Commonwealth Ave, Boston, MA 02116",
                Price = 1200000,
                CodeInternal = "BOS001",
                Year = 1920,
                OwnerId = owners[3].Id,
                Enabled = true
            },
            new Property
            {
                Name = "Mountain Cabin Retreat",
                Address = "456 Pine Ridge Trail, Aspen, CO 81611",
                Price = 3200000,
                CodeInternal = "ASP001",
                Year = 2021,
                OwnerId = owners[4].Id,
                Enabled = true
            },
            new Property
            {
                Name = "Urban Loft Studio",
                Address = "888 Industrial Blvd, Seattle, WA 98101",
                Price = 675000,
                CodeInternal = "SEA001",
                Year = 2017,
                OwnerId = owners[0].Id,
                Enabled = true
            },
            new Property
            {
                Name = "Victorian Style Home",
                Address = "222 Victorian Way, San Francisco, CA 94102",
                Price = 1800000,
                CodeInternal = "SF001",
                Year = 1895,
                OwnerId = owners[1].Id,
                Enabled = true
            },
            new Property
            {
                Name = "Desert Modern House",
                Address = "777 Cactus Drive, Phoenix, AZ 85001",
                Price = 725000,
                CodeInternal = "PHX001",
                Year = 2022,
                OwnerId = owners[2].Id,
                Enabled = true
            }
        };

        await propertiesCollection.InsertManyAsync(properties);
    }

    private async Task SeedPropertyImagesAsync()
    {
        var imagesCollection = _database.GetCollection<PropertyImage>("property_images");
        var propertiesCollection = _database.GetCollection<Property>("properties");
        
        var count = await imagesCollection.CountDocumentsAsync(_ => true);
        if (count > 0) return;

        var properties = await propertiesCollection.Find(_ => true).ToListAsync();
        if (!properties.Any()) return;

        var imageUrls = new[]
        {
            "https://images.unsplash.com/photo-1560518883-ce09059eeffa?w=800&h=600&fit=crop",
            "https://images.unsplash.com/photo-1570129477492-45c003edd2be?w=800&h=600&fit=crop",
            "https://images.unsplash.com/photo-1484154218962-a197022b5858?w=800&h=600&fit=crop",
            "https://images.unsplash.com/photo-1449844908441-8829872d2607?w=800&h=600&fit=crop",
            "https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800&h=600&fit=crop"
        };

        var images = new List<PropertyImage>();
        var random = new Random();

        foreach (var property in properties)
        {
            // Add 1-3 images per property
            var imageCount = random.Next(1, 4);
            for (int i = 0; i < imageCount; i++)
            {
                images.Add(new PropertyImage
                {
                    PropertyId = property.Id,
                    File = imageUrls[random.Next(imageUrls.Length)],
                    Enabled = true
                });
            }
        }

        await imagesCollection.InsertManyAsync(images);
    }

    private async Task SeedPropertyTracesAsync()
    {
        var tracesCollection = _database.GetCollection<PropertyTrace>("property_traces");
        var propertiesCollection = _database.GetCollection<Property>("properties");
        
        var count = await tracesCollection.CountDocumentsAsync(_ => true);
        if (count > 0) return;

        var properties = await propertiesCollection.Find(_ => true).ToListAsync();
        if (!properties.Any()) return;

        var traces = new List<PropertyTrace>();
        var random = new Random();
        var traceNames = new[] { "Purchase", "Sale", "Appraisal", "Tax Assessment", "Insurance Claim" };

        foreach (var property in properties)
        {
            // Add 1-2 traces per property
            var traceCount = random.Next(1, 3);
            for (int i = 0; i < traceCount; i++)
            {
                var baseValue = property.Price;
                var variation = random.Next(-100000, 100000);
                var value = Math.Max(baseValue + variation, 100000);
                
                traces.Add(new PropertyTrace
                {
                    PropertyId = property.Id,
                    DateSale = DateTime.UtcNow.AddDays(-random.Next(1, 365)),
                    Name = traceNames[random.Next(traceNames.Length)],
                    Value = value,
                    Tax = value * 0.05m // 5% tax
                });
            }
        }

        await tracesCollection.InsertManyAsync(traces);
    }
}