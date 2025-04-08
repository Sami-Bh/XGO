using Microsoft.EntityFrameworkCore;
using XGO.Storage.Api.Storage.Domain;

namespace XGO.Storage.Api.Storage.Persistence
{
    public class XgoStorageDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<StorageLocation> StorageLocations { get; set; }
        public DbSet<StoredItem> StoredItems { get; set; }
    }
}
