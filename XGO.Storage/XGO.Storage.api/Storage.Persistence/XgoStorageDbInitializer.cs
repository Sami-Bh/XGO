using XGO.Storage.Api.Storage.Domain;

namespace XGO.Storage.Api.Storage.Persistence
{
    public static class XgoStorageDbInitializer
    {
        public static async Task SeedDataAsync(XgoStorageDbContext dbContext)
        {
            dbContext.AddRange(GetData());
            await dbContext.SaveChangesAsync();
        }

        public static IList<StorageLocation> GetData()
        {
            return new List<StorageLocation>()
            {
                new StorageLocation
            {
                Location = "Warehouse A", StoredItems = new List<StoredItem>
                {
                    new StoredItem { ProductId = 10, Quantity = 50, ExpiryDate = DateTime.UtcNow.AddMonths(6) },
                    new StoredItem { ProductId = 15, Quantity = 100, ExpiryDate = null },
                    new StoredItem { ProductId = 14, Quantity = 75, ExpiryDate = DateTime.UtcNow.AddMonths(9) }
                }
            },
            new StorageLocation
            {
                Location = "Warehouse B", StoredItems = new List<StoredItem>
                {
                    new StoredItem { ProductId = 12, Quantity = 30, ExpiryDate = DateTime.UtcNow.AddMonths(12) },
                    new StoredItem { ProductId = 17, Quantity = 40, ExpiryDate = null },
                    new StoredItem { ProductId = 19, Quantity = 60, ExpiryDate = DateTime.UtcNow.AddMonths(8) }
                }
            },
            new StorageLocation
            {
                Location = "Cold Storage", StoredItems = new List<StoredItem>
                {
                    new StoredItem { ProductId = 18, Quantity = 20, ExpiryDate = DateTime.UtcNow.AddMonths(3) },
                    new StoredItem { ProductId = 11, Quantity = 55, ExpiryDate = DateTime.UtcNow.AddMonths(5) },
                    new StoredItem { ProductId = 16, Quantity = 35, ExpiryDate = null }
                }
            }
            };
        }
    }
}
