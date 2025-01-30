using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using XGORepository.Interfaces;

namespace XGORepository.Models
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class 
    {
        #region Fields
        private readonly XGODbContext _xGODbContext;
        #endregion

        #region Properties
        protected XGODbContext DbContext => _xGODbContext;
        #endregion

        #region Constructors
        public RepositoryBase(XGODbContext xGODbContext)
        {
            _xGODbContext = xGODbContext ?? throw new ArgumentNullException(nameof(xGODbContext));
        }
        #endregion

        #region Methods
        public async Task<T> CreateAsync(T entity)
        {
                var newEntity = await _xGODbContext.Set<T>().AddAsync(entity);
                await _xGODbContext.SaveChangesAsync();
                return entity;           

        }

        public async Task DeleteAsync(T entity)
        {
            _xGODbContext.Set<T>().Remove(entity);
            await _xGODbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _xGODbContext.Set<T>()/*.AsNoTracking()*/.ToListAsync();
        }

        public async Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _xGODbContext.Set<T>().Where(expression).ToListAsync();
        }

        public IQueryable<T> Include<P>(Expression<Func<T, P>> navigationPropertyPath) where P : class
        {
            return _xGODbContext.Set<T>().Include(navigationPropertyPath: navigationPropertyPath);
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _xGODbContext.Set<T>().Where(expression);
        }
        public async Task UpdateAsync(T entity)
        {
            //_xGODbContext.Entry(entity).State = EntityState.Modified;
            _xGODbContext.Set<T>().Update(entity);
            await _xGODbContext.SaveChangesAsync();
        }
        #endregion

    }
}
