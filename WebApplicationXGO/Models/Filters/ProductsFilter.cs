namespace WebApplicationXGO.Models.Filters
{
    public class ProductsFilter
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? SearchText { get; set; } = "";
    }
}