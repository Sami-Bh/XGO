using BuildingBlocks.DTOs;

namespace Application.DTOs
{
    public class ListedDto<T> where T :BaseDto
    {
        public int PageCount { get; set; }
        public IList<T> Items { get; set; } = [];
    }
}
