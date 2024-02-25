using Microsoft.EntityFrameworkCore;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Services.Implementations
{
    public class SubCategoryUnitOfWork(ICategoryRepository categoryRepository) : ISubCategoryUnitOfWork
    {

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId)
        {
            var category = await categoryRepository.GetByCondition(x => x.Id == categoryId).Include(x => x.SubCategories).FirstOrDefaultAsync();

            return category?.SubCategories.ToList() ?? [];
        }
    }
}
