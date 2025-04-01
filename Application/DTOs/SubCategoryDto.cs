using BuildingBlocks.DTOs;

namespace Application.DTOs
{
    public class SubCategoryDto : BaseDto
    {
        public required string Name
        {
            get; set;
        }
        public bool HasChildren { get; set; }


        public int CategoryId { get; set; }
    }
}
