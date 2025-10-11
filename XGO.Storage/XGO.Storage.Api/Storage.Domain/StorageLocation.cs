using BuildingBlocks.Models;

namespace XGO.Storage.Api.Storage.Domain
{
    public class StorageLocation : BaseModel
    {
        public required string Location { get; set; }
        public virtual ICollection<StoredItem> StoredItems { get; set; }
        public StorageLocation()
        {
            StoredItems = [];
        }
    }
}
