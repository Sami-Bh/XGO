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
                Location = "Warehouse A", StoredItems = new List<StoredItem>
                {
                    new StoredItem { ProductId = 10,ProductName="aaaa", Quantity = 50, ProductExpiryDate = DateTime.UtcNow.AddMonths(6) },
                    new StoredItem { ProductId = 15,ProductName="aaaa", Quantity = 100, ProductExpiryDate = null },
                    new StoredItem { ProductId = 14,ProductName="aaaa", Quantity = 75, ProductExpiryDate = DateTime.UtcNow.AddMonths(9) }
                }
            },
            new StorageLocation
            {
                Location = "Warehouse B", StoredItems = new List<StoredItem>
                {
                    new StoredItem { ProductId = 12,ProductName="aaaa", Quantity = 30, ProductExpiryDate = DateTime.UtcNow.AddMonths(12) },
                    new StoredItem { ProductId = 17,ProductName="aaaa", Quantity = 40, ProductExpiryDate = null },
                    new StoredItem { ProductId = 19, ProductName="aaaa",Quantity = 60, ProductExpiryDate = DateTime.UtcNow.AddMonths(8) }
                }
            },
            new StorageLocation
            {
                Location = "Cold Storage", StoredItems = new List<StoredItem>
                {
                    new StoredItem { ProductId = 18, ProductName = "aaaa", Quantity = 20, ProductExpiryDate = DateTime.UtcNow.AddMonths(3) },
                    new StoredItem { ProductId = 11, ProductName = "aaaa", Quantity = 55, ProductExpiryDate = DateTime.UtcNow.AddMonths(5) },
                    new StoredItem { ProductId = 16,ProductName="aaaa", Quantity = 35, ProductExpiryDate = null }
                }
            }
            };
        }
    }
}
