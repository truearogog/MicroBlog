using System.Linq.Expressions;

namespace MicroBlog.Core.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
        Task<T> Find(params object[] keys);
        Task Create(T model);
        Task CreateRange(IEnumerable<T> models);
        Task Update(T model);
        Task UpdateRange(IEnumerable<T> models);
        Task Delete(T model);
        Task DeleteRange(IEnumerable<T> models);
    }
}
