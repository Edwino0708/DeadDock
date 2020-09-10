using deadlock.data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace deadlock.bl.Repositories.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DeadLockDbContext _context;

        public Repository(DeadLockDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> FindSingleByAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<T> FindSingleByAsync(Expression<Func<T, bool>> predicate, List<string> includes)
        {
            if (predicate != null)
            {
                IQueryable<T> q = _context.Set<T>();

                if (includes.Count > 0)
                {
                    foreach (var i in includes)
                    {
                        q = q.Include(i);
                    }
                }
                return await q.Where(predicate).FirstOrDefaultAsync();
            }

            throw new ArgumentNullException("Predicate value must be passed to FindAllByAsync<predicate,includes>.");
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            IQueryable<T> query = _context.Set<T>();         
            return query;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, string>> orderby, bool desceding = false)
        {
            IQueryable<T> list = _context.Set<T>();
            list = desceding ? list.OrderByDescending(orderby) : list.OrderBy(orderby);
            return await list.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllPagedAsync(int size, int page, Expression<Func<T, string>> orderby, bool desceding = false)
        {
            IQueryable<T> list = _context.Set<T>();
            list = desceding ? list.OrderByDescending(orderby) : list.OrderBy(orderby);

            return await list
                .Skip(size * (page - 1))
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetRecordCount()
        {
            return await _context.Set<T>().CountAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
