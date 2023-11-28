using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Caching.Memory;

namespace PostRetriever.WorkerService;

public interface ICache<T>
{
    Task<T?> GetOrCreate(string key, Func<ICacheEntry, Task<T?>> createFunc);
}

public class OAuthTokenCache : ICache<OAuthTokenResponse>
{
    private readonly IMemoryCache _memoryCache;

    public OAuthTokenCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<OAuthTokenResponse?> GetOrCreate(string key, Func<ICacheEntry, Task<OAuthTokenResponse?>> createFunc)
    {
        return  await _memoryCache.GetOrCreateAsync(key, createFunc);
    }
}