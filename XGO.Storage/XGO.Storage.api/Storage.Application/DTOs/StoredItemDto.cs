using BuildingBlocks.DTOs;

namespace XGO.Storage.Api.Storage.Application.DTOs
{
    public class StoredItemDto : BaseDto
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public DateTime? ProductExpiryDate { get; set; }
        public int Quantity { get; set; }
    }
}
