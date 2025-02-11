using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ats_Demo.Services
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetCachedDataAsync<T>(string cacheKey)
        {
            var cachedData = await _cache.GetStringAsync(cacheKey);
            return cachedData is not null ? JsonSerializer.Deserialize<T>(cachedData) : default;
        }

        public async Task SetCacheDataAsync<T>(string cacheKey, T data, TimeSpan expiration)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var serializedData = JsonSerializer.Serialize(data);
            await _cache.SetStringAsync(cacheKey, serializedData, options);
        }

        public async Task RemoveCacheDataAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }
    }
}
