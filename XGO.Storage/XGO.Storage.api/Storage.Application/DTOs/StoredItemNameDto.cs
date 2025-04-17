using BuildingBlocks.DTOs;

namespace XGO.Storage.Api.Storage.Application.DTOs
{
    public class StoredItemNameDto:BaseDto
    {
        public required string Name { get; set; }
    }
}
