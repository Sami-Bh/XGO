using XGOModels;

namespace WebApplicationXGO.Services.Interfaces
{
    public interface ISubCategoryUnitOfWork
    {
        Task<IEnumerable<Product>> GetProducts(int subCategoryId);
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
    }
}