using XGOModels;

namespace XGORepository.Interfaces.RepositoriesInterfaces
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<IList<SubCategory>> GetSubCategories(int categoryId);
    }
}
