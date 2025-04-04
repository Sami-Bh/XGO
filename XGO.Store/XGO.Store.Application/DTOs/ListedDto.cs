using BuildingBlocks.DTOs;

namespace XGO.Store.Application.DTOs
{
    public class ListedDto<T> where T : BaseDto
    {
        public int PageCount { get; set; }
        public IList<T> Items { get; set; } = [];
    }
}
