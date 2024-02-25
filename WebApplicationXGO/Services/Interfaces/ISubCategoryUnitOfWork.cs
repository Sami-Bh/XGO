using XGOModels;

namespace WebApplicationXGO.Services.Interfaces
{
    public interface ISubCategoryUnitOfWork
    {
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId);
    }
}