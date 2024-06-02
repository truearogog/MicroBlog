using MicroBlog.Core.Services;
using Microsoft.Extensions.Caching.Memory;

namespace MicroBlog.Services.Services
{
    public class CacheService(IMemoryCache memoryCache) : ICacheService
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        public async Task<T?> GetOrCreateAsync<T>(object key, Func<ICacheEntry, Task<T>> factory)
        {
            return await _memoryCache.GetOrCreateAsync(key, factory);
        }

        public void Remove(object key)
        {
            _memoryCache.Remove(key);
        }
    }
}
