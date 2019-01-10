using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbContext dbContext;
        public GenericRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Return all items
        /// </summary>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Return all items
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        public async virtual Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Get required item
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        public async virtual Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="item">item to add</param>
        public virtual void Create(T item)
        {
            dbContext.Set<T>().Add(item);
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">item to update</param>
        public virtual void Update(T item)
        {
            dbContext.Set<T>().Update(item);
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="item">item to delete</param>
        public virtual void Delete(T item)
        {
            dbContext.Set<T>().Remove(item);
        }

        /// <summary>
        /// Detach item
        /// </summary>
        /// <param name="item">item to detach</param>
        public virtual void DetachEntity(T item)
        {
            dbContext.Entry(item).State = EntityState.Detached;
        }
    }
}
