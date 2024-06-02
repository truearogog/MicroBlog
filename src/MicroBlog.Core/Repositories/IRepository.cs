using System.Linq.Expressions;

namespace MicroBlog.Core.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<T> Find(params object[] keys);
        Task<bool> Any(Expression<Func<T, bool>> predicate);
        Task<bool> All(Expression<Func<T, bool>> predicate);
        Task<T> Create(T model);
        Task<IEnumerable<T>> CreateRange(IEnumerable<T> models);
        Task Update(T model);
        Task UpdateRange(IEnumerable<T> models);
        Task Delete(T model);
        Task Delete(params object[] keys);
        Task DeleteRange(IEnumerable<T> models);
    }
}
