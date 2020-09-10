using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace deadlock.bl.Repositories.Base
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, string>> orderby, bool desceding = false);
        Task<T> FindSingleByAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindSingleByAsync(Expression<Func<T, bool>> predicate, List<string> includes);
        Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
        Task<int> GetRecordCount();
        Task<IEnumerable<T>> GetAllPagedAsync(int pageSize, int currentPage, Expression<Func<T, string>> orderby, bool desceding = false);
        IQueryable<T> GetAllAsQueryable();
    }
}
