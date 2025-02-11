namespace Ats_Demo.Application.Services
{
    public interface IRedisCacheService
    {
        Task<T?> GetCachedDataAsync<T>(string cacheKey);
        Task SetCacheDataAsync<T>(string cacheKey, T data, TimeSpan expiration);
        Task RemoveCacheDataAsync(string cacheKey);
    }
}
