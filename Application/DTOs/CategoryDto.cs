using BuildingBlocks.DTOs;

namespace Application.DTOs
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
