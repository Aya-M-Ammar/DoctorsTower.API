
using Microsoft.Extensions.Caching.Memory;

namespace DoctorsTower.Application.Caching
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T? value);

            return Task.FromResult(value);
        }

        public Task SetAsync<T>(
            string key,
            T value,
            TimeSpan expiration)
        {
            _memoryCache.Set(
                key,
                value,
                expiration);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);

            return Task.CompletedTask;
        }
    }
}