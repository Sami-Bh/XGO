namespace WebApplicationXGO.Models.Filters
{
    public class ProductsFilter
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? SearchText { get; set; } = "";
        public int PageSize { get; set; } = 5;
        public int PageIndex { get; set; } = 1;

        public int GetSkip()
        {
            var pageSize = PageSize <= 0 || PageSize > 5 ? 5 : PageSize;
            var pageIndex = PageIndex > 0 ? PageIndex-1 : 1;
            return pageSize * pageIndex;
        }

        public int GetPageCount(int count)
        {
            
            return count > PageSize ? (int)Math.Ceiling((decimal)count /PageSize) : 1;
        }
    }
}