using BuildingBlocks.DTOs;

namespace BuildingBlocks.Core
{
    public class ListedDto<T> where T : BaseDto
    {
        public int PageCount { get; set; }
        public IList<T> Items { get; set; } = [];
    }
}
