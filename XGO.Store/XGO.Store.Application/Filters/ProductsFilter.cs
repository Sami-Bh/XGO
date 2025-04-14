using BuildingBlocks.Core;

namespace XGO.Store.Application.Filters
{
    public class ProductsFilter: FilterBase
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? SearchText { get; set; } = "";
    }
}