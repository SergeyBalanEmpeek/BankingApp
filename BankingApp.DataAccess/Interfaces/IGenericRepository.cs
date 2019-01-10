using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BankingApp.DataAccess
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Return all items
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll();

        /// <summary>
        /// Return all items
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Get required item
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns></returns>
        Task<T> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="item">item to add</param>
        void Create(T item);

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">item to update</param>
        void Update(T item);

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="item">item to delete</param>
        void Delete(T item);

        /// <summary>
        /// Detach item
        /// </summary>
        /// <param name="item">item to detach</param>
        void DetachEntity(T item);
    }
}
