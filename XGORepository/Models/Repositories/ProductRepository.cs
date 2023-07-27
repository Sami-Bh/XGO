using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace XGORepository.Models.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(XGODbContext xGODbContext) : base(xGODbContext)
        {
        }
    }
}