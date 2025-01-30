using XGOModels;

namespace WebApplicationXGO.Services.Interfaces
{
    public interface IProductUnitOfWork
    {
        Task<ICollection<Product>> GetProductsIncludeSubCategoryAsync();
    }
}