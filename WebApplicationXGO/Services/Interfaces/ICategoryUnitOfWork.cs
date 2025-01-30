using XGOModels;

namespace WebApplicationXGO.Services.Interfaces
{
    public interface ICategoryUnitOfWork
    {
        Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
    }
}