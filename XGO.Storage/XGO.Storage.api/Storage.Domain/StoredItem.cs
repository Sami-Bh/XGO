using BuildingBlocks.Models;

namespace XGO.Storage.Api.Storage.Domain
{
    public class StoredItem : BaseModel
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int StorageLocationId { get; set; }
        public StorageLocation? StorageLocation { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
