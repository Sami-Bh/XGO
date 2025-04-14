using BuildingBlocks.Core;

namespace XGO.Storage.Api.Storage.Application.Models
{
    public class ProductsFilter : FilterBase
    {
        public int? CategoryId { get; set; }
        public int? SubcategoryId { get; set; }
        public int StorageId { get; set; }
        public string? OrderBy { get; set; } = "";
        public string? OrderDirection { get; set; } = "";
        public string? ProductNameSearchText { get; set; } = "";
    }
}
