using BuildingBlocks.DTOs;

namespace XGO.Store.Application.DTOs
{
    public class CategoryDto : BaseDto
    {
        public required string Name
        {
            get; set;
        }
        public bool HasChildren { get; set; }

    }
}
