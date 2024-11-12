using Microsoft.EntityFrameworkCore;
using MVC.DataAccess.Data;
using MVC.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> expresion = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includPropertys = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (expresion != null)
            {
                query = query.Where(expresion);
            }
            if( includPropertys != null)
            {
                foreach (var incluidProp in includPropertys.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluidProp);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();

        }

        public async Task<T> GetFirts(Expression<Func<T, bool>> expresion = null, 
            string includPropertys = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (expresion != null)
            {
                query = query.Where(expresion);
            }
            if (includPropertys != null)
            {
                foreach (var incluidProp in includPropertys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluidProp);
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
