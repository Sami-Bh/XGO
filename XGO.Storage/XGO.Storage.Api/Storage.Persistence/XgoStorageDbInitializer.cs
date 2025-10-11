using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Domain;

namespace XGO.Storage.Api.Storage.Persistence
{
    public static class XgoStorageDbInitializer
    {
        public static async Task SeedDataAsync(XgoStorageDbContext dbContext)
        {
            if (!await dbContext.StoredItems.AnyAsync())
            {
                dbContext.AddRange(GetData());
                await dbContext.SaveChangesAsync();
            }
        }

        public static IList<StorageLocation> GetData()
        {
            return new List<StorageLocation>()
            {
                new StorageLocation
                {
                    Location = "Warehouse A",
                    StoredItems = new List<StoredItem>
                    {
                        new StoredItem { ProductId = 1, ProductName = "iPhone 15", Quantity = 50, ProductExpiryDate = DateTime.UtcNow.AddMonths(6) },
                        new StoredItem { ProductId = 2, ProductName = "Samsung Galaxy S23", Quantity = 100, ProductExpiryDate = null },
                        new StoredItem { ProductId = 3, ProductName = "Google Pixel 7", Quantity = 75, ProductExpiryDate = DateTime.UtcNow.AddMonths(9) },
                        new StoredItem { ProductId = 4, ProductName = "OnePlus 11", Quantity = 60, ProductExpiryDate = DateTime.UtcNow.AddMonths(12) },
                        new StoredItem { ProductId = 5, ProductName = "Xiaomi 13 Pro", Quantity = 80, ProductExpiryDate = null },
                        new StoredItem { ProductId = 6, ProductName = "Leather Sofa", Quantity = 30, ProductExpiryDate = DateTime.UtcNow.AddMonths(12) },
                        new StoredItem { ProductId = 7, ProductName = "Fabric Sofa", Quantity = 40, ProductExpiryDate = null },
                        new StoredItem { ProductId = 8, ProductName = "Recliner Sofa", Quantity = 60, ProductExpiryDate = DateTime.UtcNow.AddMonths(8) },
                        new StoredItem { ProductId = 9, ProductName = "L-Shaped Sofa", Quantity = 20, ProductExpiryDate = DateTime.UtcNow.AddMonths(6) },
                        new StoredItem { ProductId = 10, ProductName = "Futon Sofa", Quantity = 50, ProductExpiryDate = null }
                    }
                },
                new StorageLocation
                {
                    Location = "Warehouse B",
                    StoredItems = new List<StoredItem>
                    {
                        new StoredItem { ProductId = 11, ProductName = "Spider-Man Figure", Quantity = 20, ProductExpiryDate = DateTime.UtcNow.AddMonths(3) },
                        new StoredItem { ProductId = 12, ProductName = "Batman Figure", Quantity = 55, ProductExpiryDate = DateTime.UtcNow.AddMonths(5) },
                        new StoredItem { ProductId = 13, ProductName = "Superman Figure", Quantity = 35, ProductExpiryDate = null },
                        new StoredItem { ProductId = 14, ProductName = "Iron Man Figure", Quantity = 45, ProductExpiryDate = DateTime.UtcNow.AddMonths(7) },
                        new StoredItem { ProductId = 15, ProductName = "Captain America Figure", Quantity = 25, ProductExpiryDate = DateTime.UtcNow.AddMonths(4) },
                        new StoredItem { ProductId = 16, ProductName = "The Great Gatsby", Quantity = 30, ProductExpiryDate = DateTime.UtcNow.AddMonths(12) },
                        new StoredItem { ProductId = 17, ProductName = "1984", Quantity = 40, ProductExpiryDate = null },
                        new StoredItem { ProductId = 18, ProductName = "To Kill a Mockingbird", Quantity = 60, ProductExpiryDate = DateTime.UtcNow.AddMonths(8) },
                        new StoredItem { ProductId = 19, ProductName = "The Catcher in the Rye", Quantity = 20, ProductExpiryDate = DateTime.UtcNow.AddMonths(6) },
                        new StoredItem { ProductId = 20, ProductName = "Brave New World", Quantity = 50, ProductExpiryDate = null }
                    }
                },
                new StorageLocation
                {
                    Location = "Cold Storage",
                    StoredItems = new List<StoredItem>
                    {
                        new StoredItem { ProductId = 21, ProductName = "Milk", Quantity = 30, ProductExpiryDate = DateTime.UtcNow.AddMonths(12) },
                        new StoredItem { ProductId = 22, ProductName = "Cheese", Quantity = 40, ProductExpiryDate = null },
                        new StoredItem { ProductId = 23, ProductName = "Yogurt", Quantity = 60, ProductExpiryDate = DateTime.UtcNow.AddMonths(8) },
                        new StoredItem { ProductId = 24, ProductName = "Butter", Quantity = 20, ProductExpiryDate = DateTime.UtcNow.AddMonths(6) },
                        new StoredItem { ProductId = 25, ProductName = "Cream", Quantity = 50, ProductExpiryDate = null },
                        new StoredItem { ProductId = 26, ProductName = "Frozen Peas", Quantity = 20, ProductExpiryDate = DateTime.UtcNow.AddMonths(3) },
                        new StoredItem { ProductId = 27, ProductName = "Ice Cream", Quantity = 55, ProductExpiryDate = DateTime.UtcNow.AddMonths(5) },
                        new StoredItem { ProductId = 28, ProductName = "Frozen Pizza", Quantity = 35, ProductExpiryDate = null }
                    }
                }
            };
        }
    }
}
