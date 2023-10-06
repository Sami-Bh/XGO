using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace XGORepository.Models.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(XGODbContext xGODbContext) : base(xGODbContext)
        {
        }

        public async Task<IList<SubCategory>> GetSubCategories(int categoryId)
        {
            var dbCategory = await dbContext.Categories.Include(x => x.SubCategories).FirstOrDefaultAsync(x => x.Id == categoryId);

            return dbCategory is null ? new List<SubCategory>() { } : dbCategory.SubCategories.ToList();
        }
    }
}
