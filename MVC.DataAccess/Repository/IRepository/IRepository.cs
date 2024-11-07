using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expresion = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includPropertys = null,
            bool isTracking = true
            );
        Task<T> GetFirts(Expression<Func<T, bool>> expresion = null,
            string includPropertys = null,
            bool isTracking = true
            );
        Task Create(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
