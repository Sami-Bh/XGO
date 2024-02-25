using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XGORepository.Interfaces
{
    public interface IRepositoryBase<T>
    {
        Task<List<T>> GetAllAsync();
        Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<T> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
