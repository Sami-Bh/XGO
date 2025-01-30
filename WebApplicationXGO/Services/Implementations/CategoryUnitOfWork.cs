using Microsoft.EntityFrameworkCore;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Services.Implementations
{
    public class CategoryUnitOfWork(ICategoryRepository categoryRepository) : ICategoryUnitOfWork
    {
        public async Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            return await categoryRepository.Include(x => x.SubCategories).ToListAsync();
        }
    }
}
