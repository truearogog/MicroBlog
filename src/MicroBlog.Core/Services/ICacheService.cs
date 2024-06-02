using Microsoft.Extensions.Caching.Memory;

namespace MicroBlog.Core.Services
{
    public interface ICacheService
    {
        Task<T?> GetOrCreateAsync<T>(object key, Func<ICacheEntry, Task<T>> factory);
        void Remove(object key);
    }
}
