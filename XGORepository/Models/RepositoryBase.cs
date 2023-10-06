using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XGORepository.Interfaces;

namespace XGORepository.Models
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        #region Fields
        private readonly XGODbContext _xGODbContext;
        #endregion

        #region Properties
        protected XGODbContext dbContext => _xGODbContext;
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
            try
            {
                var newEntity = await _xGODbContext.Set<T>().AddAsync(entity);
                await _xGODbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task DeleteAsync(T entity)
        {
            _xGODbContext.Set<T>().Remove(entity);
            await _xGODbContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _xGODbContext.Set<T>().ToListAsync();
        }

        public async Task<IList<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _xGODbContext.Set<T>().Where(expression).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _xGODbContext.Set<T>().Update(entity);
            await _xGODbContext.SaveChangesAsync();
        }

        #endregion

    }
}
