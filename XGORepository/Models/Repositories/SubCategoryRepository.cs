using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace XGORepository.Models.Repositories
{
    public class SubCategoryRepository(XGODbContext xGODbContext) : RepositoryBase<SubCategory>(xGODbContext), ISubCategoryRepository
    {
        private readonly XGODbContext xGODbContext = xGODbContext;

        public async Task<SubCategory> CreateAsync(int categoryId, SubCategory subCategory)
        {
            var category = await xGODbContext.Categories.Include(x => x.SubCategories).FirstAsync(x => x.Id == categoryId);

            category.SubCategories.Add(subCategory);
            xGODbContext.Update(category);

            await xGODbContext.SaveChangesAsync();

            return subCategory;
        }
    }
}
