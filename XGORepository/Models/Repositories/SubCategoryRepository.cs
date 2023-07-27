using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace XGORepository.Models.Repositories
{
    public class SubCategoryRepository : RepositoryBase<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(XGODbContext xGODbContext) : base(xGODbContext)
        {
        }
    }
}
