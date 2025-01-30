using Microsoft.EntityFrameworkCore;
using WebApplicationXGO.Services.Interfaces;
using XGOModels;
using XGORepository.Interfaces.RepositoriesInterfaces;

namespace WebApplicationXGO.Services.Implementations
{
    public class ProductUnitOfWork(IProductRepository _productRepository) : IProductUnitOfWork
    {
        public async Task<ICollection<Product>> GetProductsIncludeSubCategoryAsync()
        {
            return await _productRepository.Include(x => x.SubCategory).ToListAsync();
        }
    }
}
